using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLife : MonoBehaviour
{
    //Currently unused
    //float aliveTime = 6;
    //bool isAlive = true;

    private bool wasShot = false;

    // The time in seconds the camera is locked before the OnRespawn evnet is called.
    private float extinguishTimer = 1;

    private Vector3 spawnPos;
    private Rigidbody projetileBody;

    #region Reset values
    //private bool isAlive_R = true;

    private bool isShot_R = false;
    private float extTimer_R = 0;
    private Vector3 spawnPos_R;
    #endregion

    /// <summary>
    /// Subscribes to events
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;
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
        EventManager.OnProjectileLaunched -= OnShot;
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnProjectileRespawn -= OnRespawn;
        EventManager.OnGameWorldReset -= OnWorldReset;
    }

    /// <summary>
    /// Called when projectile is shot
    /// </summary>
    private void OnShot(Vector3 dir, float force)
    {
        wasShot = true;
    }

    /// <summary>
    /// Called when projectile hits killer
    /// </summary>
    private void OnDeath(int amount)
    {
        //isAlive = false;
        wasShot = false;

        DeathSequence();
    }

    /// <summary>
    /// Called when projectile hits flammable
    /// </summary>
    private void OnIgnite(int health, Vector3 newCheckpoint)
    {
        wasShot = false;

        DeathSequence();
    }

    /// <summary>
    /// Called when the world is reset
    /// </summary>
    private void OnWorldReset()
    {
        //isAlive = isAlive_R;
        wasShot = isShot_R;
        extinguishTimer = extTimer_R;
        spawnPos = spawnPos_R;
        gameObject.transform.position = spawnPos_R;
    }

    /// <summary>
    /// Used to move the flame and reset everything for a new shot
    /// </summary>
    void OnRespawn()
    {
        projetileBody.Sleep();

        gameObject.transform.position = spawnPos;
        //isAlive = true;
    }

    // Use this for initialization
    void Start()
    {
        spawnPos_R = gameObject.transform.position;
        spawnPos = spawnPos_R;
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
                spawnPos = other.gameObject.transform.GetChild(0).transform.position;

            }
            catch
            {
                Debug.LogWarning("ProjectileLife.cs: Collision object does not have a LaunchPoint child object even though it is tagged as a FlammableObject.");
            }

            if (flammableObject != null)
            {
                EventManager.InvokeOnProjectileIgnite(flammableObject.Health, spawnPos);
                
                // Not sure if this is a good idea.
                other.gameObject.GetComponent<Collider>().enabled = false;
            }
        }

        //Killers
        if (other.gameObject.tag == "KillerObject")
        {
            EventManager.InvokeOnProjectileDeath(-1);
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
