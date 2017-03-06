using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLife : MonoBehaviour {

    [SerializeField]
    bool isAlive = true;
    [SerializeField]
    bool isShot = false;

    //Currently unused
    //float aliveTime = 6;

    [SerializeField]
    bool extinguishState = false;
    float extinguishTimer = 0;

    [SerializeField]
    Vector3 spawnPos;
    Rigidbody projetileBody;


    /// <summary>
    /// Subscribed events
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;
        EventManager.OnProjectileDead += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    /// <summary>
    /// Subscribed events
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShot;
        EventManager.OnProjectileDead -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }

    /// <summary>
    /// Called when projectile is shot
    /// </summary>
    /// <param name="dir">none</param>
    /// <param name="force">none</param>
    private void OnShot(Vector3 dir,float force)
    {
        Shoot();
    }

    /// <summary>
    /// Called when projectile hits killer
    /// </summary>
    private void OnDeath(int amount)
    {
        Death();
    }

    /// <summary>
    /// Called when projectile hits flammable
    /// </summary>
    private void OnIgnite(int amount)
    {
        Ignite();
    }

    // Use this for initialization
    void Start () {       
        spawnPos = gameObject.transform.position;
        projetileBody = gameObject.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        //General looking to see if extenguish needs to happen
        DeathSequence(extinguishState);
        extinguishTimer = extinguishTimer - Time.deltaTime;
        
        //Handle when not shot
        if(!isShot)
        {
            projetileBody.Sleep();
        }


        //Remnant Can be used in future
        //Handle when lit
        //if(isShot)
        //{
        //    aliveTime = aliveTime - Time.deltaTime;
        //    //Change to idle time(ie gets stuck) what about max lifetime
        //    //if (aliveTime <= 0)
        //    //{
        //    //    isAlive = false;
        //    //}

        //}
        		
	}

    /// <summary>
    /// Called when projectile is shot
    /// </summary>
    public void Shoot()
    {
        isShot = true;
       
    }

    /// <summary>
    /// Called when killer is hit
    /// </summary>
    void Death()
    {
        extinguishTimer = 1;
        isAlive = false;
        isShot = false;
        extinguishState = true;

    }
    
    /// <summary>
    /// Called when flammable is hit
    /// </summary>
    void Ignite()
    {
        extinguishTimer = 1;       
        extinguishState = true;
        isShot = false;
    }

    /// <summary>
    /// Used to move the flame and reset everything for a new shot
    /// </summary>
    void Respawn()
    {
        //aliveTime = 5;
        projetileBody.Sleep();
        gameObject.transform.position = spawnPos;
        isAlive = true;

    }


    /// <summary>
    /// Used for collision and interaction with other objects
    /// </summary>
    /// <param name="other">other object</param>
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
            EventManager.InvokeOnProjectileDead(1);
        }
    }

    /// <summary>
    /// The Extenguish sequence for when fire hist water or a flammable object.
    /// </summary>
    /// <param name="on">True to run the extenguish sequence</param>
    private void DeathSequence(bool on)
    {
        if (on)
        {
            //Make extinguish or death animation or whatever here
            
            if (extinguishTimer <= 0)
            {
                Respawn();
                extinguishState = false;
            }
        }
    }

}
