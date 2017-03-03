using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLife : MonoBehaviour {

    [SerializeField]
    bool isAlive = true;
    [SerializeField]
    bool isShot = false;
    float aliveTime = 6;
    [SerializeField]
    Vector3 spawnPos;
    Rigidbody projetileBody;


    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;
        EventManager.OnProjectileDead += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShot;
        EventManager.OnProjectileDead -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }

    private void OnShot(Vector3 dir,float force)
    {
        Shoot();
    }

    private void OnDeath()
    {
        Death();
    }

    private void OnIgnite()
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
        
        
        //Handle when not shot
        if(!isShot)
        {
            projetileBody.Sleep();
        }

        //Handle when lit
        if(isShot)
        {
            aliveTime = aliveTime - Time.deltaTime;

            //Change to idle time(ie gets stuck) what about max lifetime
            if (aliveTime <= 0)
            {
                isAlive = false;
            }

            if (!isAlive)
            {
                Respawn();
            }
        }
        		
	}

    public void Shoot()
    {
        isShot = true;
       
    }

    void Death()
    {
        isAlive = false;
        isShot = false;                       
        Respawn();

    }

    void Ignite()
    {
        isShot = false;        
        Respawn();
    }

    void Respawn()
    {
        aliveTime = 5;
        projetileBody.Sleep();
        gameObject.transform.position = spawnPos;
        isAlive = true;
    }


    //Collision handling
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "FlammableObject")
        {

            spawnPos = other.gameObject.transform.GetChild(0).transform.position;
            other.gameObject.GetComponent<Collider>().enabled = false;
            EventManager.InvokeOnProjectileIgnite();
            

        }

        if (other.gameObject.tag == "KillerObject")
        {
            EventManager.InvokeOnProjectileDead();
        }

    }




}
