using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    private bool wasShot = false;

    [SerializeField]
    private int maxCollisions = 6; // Should be 1 more than expected as the object collides with the ground the frame it is launched.
    private int currentCollisions = 0;

    private float idleVelocityThreshold = 0.5f;
    private float maxIdleTimeBeforeDeath = 1;
    private float idleTimeBeforeDeath = 0;

    // The time in seconds the camera is locked before the OnRespawn evnet is called.
    private float extinguishTimer = 1;

    private Vector3 spawnPoint;
    private Rigidbody projetileBody;

    #region Reset values
    private bool wasShot_R = false;
    private float extTimer_R = 0;
    private Vector3 spawnPoint_R;
    #endregion

    public Vector3 SpawnPoint
    {
        get { return spawnPoint; }
    }

    /// <summary>
    /// Subscribes to events
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnLaunch;
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnProjectileRespawn += OnRespawn;
        EventManager.OnGameWorldReset += OnWorldReset;
    }

    /// <summary>
    /// Unsubscribes from events
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnLaunch;
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnProjectileRespawn -= OnRespawn;
        EventManager.OnGameWorldReset -= OnWorldReset;
    }

    /// <summary>
    /// Called when projectile is shot
    /// </summary>
    private void OnLaunch(Vector3 dir, float force)
    {
        wasShot = true;
    }

    /// <summary>
    /// Called when projectile hits killer
    /// </summary>
    private void OnDeath()
    {
        //isAlive = false;
        wasShot = false;

        DeathSequence();

        currentCollisions = 0;
    }

    /// <summary>
    /// Called when projectile hits flammable
    /// </summary>
    private void OnIgnite(Vector3 newCheckpoint)
    {
        wasShot = false;

        DeathSequence();

        currentCollisions = 0;
    }

    /// <summary>
    /// Called when the world is reset
    /// </summary>
    private void OnWorldReset()
    {
        //isAlive = isAlive_R;
        wasShot = wasShot_R;
        extinguishTimer = extTimer_R;
        spawnPoint = spawnPoint_R;
        gameObject.transform.position = spawnPoint_R;
    }

    /// <summary>
    /// Used to move the flame and reset everything for a new shot
    /// </summary>
    void OnRespawn()
    {
        projetileBody.Sleep();

        gameObject.transform.position = spawnPoint;
    }

    // Use this for initialization
    void Start()
    {
        spawnPoint_R = gameObject.transform.position;
        spawnPoint = spawnPoint_R;
        projetileBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle when not shot
        if (!wasShot)
        {
            projetileBody.Sleep();
        }

        // If the projectile was shot and the magnitude of the velocity is below idle velocity
        if (projetileBody.velocity.magnitude <= 0.5f && wasShot)
        {
            // Increase the idle timer
            idleTimeBeforeDeath += Time.deltaTime;
        }
        // If the speed is above the threshold
        else
        {
            // And the idel timer is not 0
            if (idleTimeBeforeDeath != 0)
            {
                // The idle timer is set to 0
                idleTimeBeforeDeath = 0;
            }
        }

        // If the number of collision exceed the maximum number of allowed collision or if the idle timer exceeds the max idle time
        if (currentCollisions >= maxCollisions || idleTimeBeforeDeath >= maxIdleTimeBeforeDeath)
        {
            // The projectile is killed
            wasShot = false;

            DeathSequence();

            currentCollisions = 0;
        }
    }

    /// <summary>
    /// Used for collision and interaction with other objects
    /// </summary>
    /// <param name="other">The object we are colliding with.</param>
    void OnCollisionEnter(Collision other)
    {
        //Checkpoints
        if (other.gameObject.tag == "FlammableObject")
        {
            Flammable flammableObject = null;

            try
            {
                flammableObject = other.gameObject.GetComponent<Flammable>();
            }
            catch
            {
                Debug.LogWarning("ProjectileLife.cs: Collision object does not have a FlammableObject component even though it is tagged as a FlammableObject.");
            }

            try
            {
                spawnPoint = other.gameObject.transform.GetChild(0).transform.position;

            }
            catch
            {
                Debug.LogWarning("ProjectileLife.cs: Collision object does not have a LaunchPoint child object even though it is tagged as a FlammableObject.");
            }

            if (flammableObject != null)
            {
                EventManager.InvokeOnProjectileIgnite(spawnPoint);

                // Not sure if this is a good idea.
                other.gameObject.GetComponent<Flammable>().FlameHitTransition();
            }
        }

        //Killers
        if (other.gameObject.tag == "KillerObject")
        {
            EventManager.InvokeOnProjectileDeath();
        }

        // If neither a KillerObject or a FlammableObject was hit the collision count is increased.
        if (other.gameObject.tag != "KillerObject" && other.gameObject.tag != "FlammableObject")
        {
            currentCollisions++;
        }
    }

    /// <summary>
    /// Starts the death sequence coroutine.
    /// </summary>
    private void DeathSequence()
    {
        StartCoroutine(CoroutineDeathSequence());
    }

    /// <summary>
    /// Coroutine for the death sequence.
    /// </summary>
    private IEnumerator CoroutineDeathSequence()
    {
        // Play sound and animation here

        yield return new WaitForSeconds(extinguishTimer);

        EventManager.InvokeOnProjectileRespawn();
    }
}
