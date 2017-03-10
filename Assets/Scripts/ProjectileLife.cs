using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLife : MonoBehaviour
{
    [SerializeField]
    bool isAlive = true;
    [SerializeField]
    bool isShot = false;

    //Currently unused
    //float aliveTime = 6;

    [SerializeField]
    bool extinguishState = false;
    float extinguishTimer = 1;

    [SerializeField]
    Vector3 spawnPos;
    Rigidbody projetileBody;

    //All Reset spesific values
    [SerializeField]
    bool isAlive_R = true;
    bool isShot_R = false;
    bool extState_R = false;
    float extTimer_R = 0;
    Vector3 spawnPos_R;


    /// <summary>
    /// Subscribed events
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
    /// Subscribed events
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
        isShot = true;
    }

    /// <summary>
    /// Called when projectile hits killer
    /// </summary>
    private void OnDeath(int amount)
    {
        isAlive = false;
        isShot = false;
        extinguishState = true;
    }

    /// <summary>
    /// Called when projectile hits flammable
    /// </summary>
    private void OnIgnite(int amount)
    {
        extinguishState = true;
        isShot = false;
    }

    /// <summary>
    /// Called when the world is reset
    /// </summary>
    private void OnWorldReset()
    {
        isAlive = isAlive_R;
        isShot = isShot_R;
        extinguishState = extState_R;
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
        isAlive = true;
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

        // General looking to see if extenguish needs to happen
        if (extinguishState)
        {
            DeathSequence();
        }

        // Handle when not shot
        if (!isShot)
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
            spawnPos = other.gameObject.transform.GetChild(0).transform.position;
            other.gameObject.GetComponent<Collider>().enabled = false;
            EventManager.InvokeOnProjectileIgnite(other.gameObject.GetComponent<Flammable>().Health);
        }

        //Killers
        if (other.gameObject.tag == "KillerObject")
        {
            EventManager.InvokeOnProjectileDeath(1);
        }
    }

    /// <summary>
    /// The Extenguish sequence for when fire hits water or a flammable object.
    /// </summary>
    private void DeathSequence()
    {
        StartCoroutine(CoroutineDeathSequence());
    }

    private IEnumerator CoroutineDeathSequence()
    {
        yield return new WaitForSeconds(extinguishTimer);

        EventManager.InvokeOnProjectileRespawn();

        extinguishState = false;
    }

}
