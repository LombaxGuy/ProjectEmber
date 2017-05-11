using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private WorldManager worldManager;

    private Flame activeFlame;
    private GameObject selectionSphere;
    private Rigidbody flameRigidbody;

    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    private Vector3 direction;

    private float defaultForceStrength = 10;

    private float maxShootMagnitude = 3;
    private float minShootMagnitude = 0.5f;

    private bool shootMode = false;
    private bool canShoot = true;

    private bool gameHasEnded = false;

    [SerializeField]
    private bool touchFlameToShoot = false;

    private bool playerShooting = false;

    public bool PlayerShooting
    {
        get { return playerShooting; }
        set { playerShooting = value; }
    }

    private void Awake()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();
        worldManager.ActiveFlame = gameObject;
    }

    // Use this for initialization
    private void Start()
    {
        activeFlame = GetComponent<Flame>();
        selectionSphere = transform.GetChild(0).gameObject;

        // Gets the rigidbody component on the active flame
        flameRigidbody = activeFlame.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EventManager.OnGameWorldReset += OnGameReset;
        EventManager.OnProjectileRespawn += OnRespawn;
        EventManager.OnProjectileLaunched += OnLaunch;
        EventManager.OnLevelCompleted += OnLevelCompleted;
        EventManager.OnLevelLost += OnLevelLost;
    }

    private void OnDisable()
    {
        EventManager.OnGameWorldReset -= OnGameReset;
        EventManager.OnProjectileRespawn -= OnRespawn;
        EventManager.OnProjectileLaunched -= OnLaunch;
        EventManager.OnLevelCompleted -= OnLevelCompleted;
        EventManager.OnLevelLost -= OnLevelLost;
    }

    /// <summary>
    /// Sets the gameHasEnded bool to false making it possible for the player to shoot the flame
    /// </summary>
    private void OnGameReset()
    {
        canShoot = true;
        gameHasEnded = false;
    }

    private void OnLaunch(Vector3 dir, float strength)
    {
        canShoot = false;

        // Adds a force impulse to the rigidbody of the active flame.
        flameRigidbody.AddForce(dir.normalized * strength, ForceMode.Impulse);
    }

    private void OnRespawn()
    {
        canShoot = true;
    }

    private void OnLevelCompleted()
    {
        gameHasEnded = true;
    }

    private void OnLevelLost()
    {
        gameHasEnded = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // If the one finger is touching the screen
        if (Input.touchCount == 1 && canShoot && !gameHasEnded)
        {
            // If the touch phase is began...
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                EventManager.InvokeOnShootingStarted();
                //... the HandleInputBegan method is called.
                HandleInputBegan();
            }

            // If the player is currently making a shot
            if (playerShooting == true)
            {
                // If the touch phase is moved...
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    //... the UpdateDirection method is called.
                    UpdateDirection();
                }

                // If the touch phase is ended...
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    //... the UpdateDirection method is called.
                    UpdateDirection();

                    // The HandleInputEnded method is called.
                    HandleInputEnded();
                }
            }
        }
        #region MouseDebugging
#if (DEBUG)
        Input.simulateMouseWithTouches = false;

        if (Input.GetMouseButtonDown(0) && canShoot && !gameHasEnded)
        {
            if (touchFlameToShoot)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Uses the above created Ray and RaycastHit to cast a ray and get the hit information.
                if (Physics.Raycast(ray, out hit))
                {
                    // If the raycast hits the active flames gameobject...
                    if (hit.collider.gameObject == activeFlame.gameObject)
                    {
                        //... the touchStartPos is set and the playerShooting variable is set to true.
                        touchStartPos = activeFlame.transform.position;
                        playerShooting = true;
                    }
                }
            }
            else
            {
                // If shootMode is true...
                if (shootMode)
                {
                    EventManager.InvokeOnShootingStarted();
                    //... the touchStartPos is set and the playerShooting variable is set to true.
                    touchStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, activeFlame.transform.position.z));
                    playerShooting = true;
                }
                // If shootMode is false
                else
                {
                    // Creates a Ray and a RaycastHit
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // Uses the above created Ray and RaycastHit to cast a ray and get the hit information.
                    if (Physics.Raycast(ray, out hit))
                    {
                        // If the ray hits the active flame...
                        if (hit.collider.gameObject == activeFlame.gameObject)
                        {
                            //... the shootMode is set to true.
                            shootMode = true;
                        }
                    }

                    // If the ray hits a flammable object...
                    if (hit.collider.gameObject.tag == "FlammableObject")
                    {
                        try
                        {
                            //... and the object is on fire the the OnSetNewSpawnPoint event is invoked.
                            Flammable flammableObject = hit.transform.GetComponent<Flammable>();

                            if (flammableObject.OnFire)
                            {
                                EventManager.InvokeOnSetNewSpawnPoint(flammableObject.SpawnPoint);
                            }
                        }
                        catch
                        {
                            Debug.LogError("ProjectileLife.cs: Collision object does not have a FlammableObject component even though it is tagged as a FlammableObject.");
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButton(0) && playerShooting)
        {
            touchEndPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, activeFlame.transform.position.z));
            direction = touchStartPos - touchEndPos;

            EventManager.InvokeOnProjectileUpdated(direction.normalized, CalculateForceStrenght());
        }

        if (Input.GetMouseButtonUp(0) && playerShooting)
        {
            // Resets the playerShooting and the shootMode variables to false.
            playerShooting = false;
            shootMode = false;

            float forceStrength = CalculateForceStrenght();

            // If the forceStrength is greater than 0...
            if (forceStrength > 0)
            {
                //... the projectile is launched by calling the LaunchProjectile method.
                LaunchProjectile(forceStrength);
            }
        }
#endif
        #endregion
        
        // Update the selection marker by calling the method UpdateSelectionSphere.
        UpdateSelectionSphere();
    }

    /// <summary>
    /// Handles touch input. Should be used for TouchPhase.Began.
    /// </summary>
    private void HandleInputBegan()
    {
        // If the touchFlameToShoot option is true
        if (touchFlameToShoot)
        {
            // Creates a Ray and a RaycastHit
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            // Uses the above created Ray and RaycastHit to cast a ray and get the hit information.
            if (Physics.Raycast(ray, out hit))
            {
                // If the raycast hits the active flames gameobject...
                if (hit.collider.gameObject == activeFlame.gameObject)
                {
                    //... the touchStartPos is set and the playerShooting variable is set to true.
                    touchStartPos = activeFlame.transform.position;
                    playerShooting = true;
                }
            }
        }
        // If the touchFlameToShoot option is false
        else
        {
            // If shootMode is true...
            if (shootMode)
            {
                //... the touchStartPos is set and the playerShooting variable is set to true.
                touchStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, activeFlame.transform.position.z));
                playerShooting = true;
            }
            // If shootMode is false
            else
            {
                // Creates a Ray and a RaycastHit
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                // Uses the above created Ray and RaycastHit to cast a ray and get the hit information.
                if (Physics.Raycast(ray, out hit))
                {
                    // If the ray hits the active flame...
                    if (hit.collider.gameObject == activeFlame.gameObject)
                    {
                        //... the shootMode is set to true.
                        shootMode = true;
                    }

                    // If the ray hits a flammable object...
                    if (hit.collider.gameObject.tag == "FlammableObject")
                    {
                        try
                        {
                            //... and the object is on fire the the OnSetNewSpawnPoint event is invoked.
                            Flammable flammableObject = hit.transform.GetComponent<Flammable>();

                            if (flammableObject.OnFire)
                            {
                                EventManager.InvokeOnSetNewSpawnPoint(flammableObject.SpawnPoint);
                            }
                        }
                        catch
                        {
                            Debug.LogError("ProjectileLife.cs: Collision object does not have a FlammableObject component even though it is tagged as a FlammableObject.");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates the variables touchEndPos and direction. Should be used for TouchPhase.Moved and TouchPhase.Ended.
    /// </summary>
    private void UpdateDirection()
    {
        // Updates the two variables.
        touchEndPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, activeFlame.transform.position.z));
        direction = touchStartPos - touchEndPos;

        EventManager.InvokeOnProjectileUpdated(direction.normalized, CalculateForceStrenght());
    }

    /// <summary>
    /// Handles touch input. Should be used for TouchPhase.Ended.
    /// </summary>
    private void HandleInputEnded()
    {
        // Resets the playerShooting and the shootMode variables to false.
        playerShooting = false;
        shootMode = false;

        float forceStrength = CalculateForceStrenght();

        // If the forceStrength is greater than 0...
        if (forceStrength > 0)
        {
            //... the projectile is launched by calling the LaunchProjectile method.
            LaunchProjectile(forceStrength);
        }
    }

    /// <summary>
    /// Used to launch the projectile.
    /// </summary>
    private void LaunchProjectile(float forceStrength)
    {
        EventManager.InvokeOnProjectileLaunched(direction.normalized, forceStrength);
    }

    /// <summary>
    /// Used to update the selection marker.
    /// </summary>
    private void UpdateSelectionSphere()
    {
        // If the selectionSphere is a gameobject...
        if (selectionSphere)
        {
            //... and if the shoot mode is true...
            if (shootMode)
            {
                //... set the sphere to active.
                selectionSphere.SetActive(true);
            }
            //... and if the shoot mode is false...
            else
            {
                //... set the sphere to not active.
                selectionSphere.SetActive(false);
            }
        }
    }

    private float CalculateForceStrenght()
    {
        float forceStrength = 0;

        // If the length of the pull is between the max and min pull distance...
        if (direction.magnitude < maxShootMagnitude && direction.magnitude > minShootMagnitude)
        {
            //... the forceStrength is calculated from the length of the pull.
            forceStrength = defaultForceStrength * ((direction.magnitude - minShootMagnitude) / (maxShootMagnitude - minShootMagnitude));
        }
        // If the length of the pull is greater than the max pull distance...
        else if (direction.magnitude > maxShootMagnitude)
        {
            //... the forceStrength is set to the defaultForceStrength
            forceStrength = defaultForceStrength;
        }
        // If the length of the pull is lower than the min pull distance...
        else
        {
            //... the forceStrength is set to 0.
            forceStrength = 0;
        }

        return forceStrength;
    }
}