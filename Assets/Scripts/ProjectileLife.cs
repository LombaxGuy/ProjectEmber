using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLife : MonoBehaviour {


    bool isAlive = true;
    [SerializeField]
    bool isShot = false;
    float aliveTime = 6;
    GameObject handler;

    Vector3 firePos;

    public Vector3 FirePos
    {
        get
        {
            return firePos;
        }
        set
        {
            firePos = value;
        }
    }

	// Use this for initialization
	void Start () {
        handler = GameObject.FindGameObjectWithTag("Handler");
        firePos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        
        //Handle when not shot
        if(!isShot)
        {
            gameObject.GetComponent<Rigidbody>().Sleep();
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
                Respawn(firePos,false,1);
            }
        }
        		
	}

    public void Fizzle(bool life, int ammount)
    {

        Respawn(firePos, life, ammount);

    }


    public void Fizzle(Vector3 spawnPos, bool life, int ammount)
    {

        Respawn(spawnPos, life, ammount);
        
    }

    void Respawn(Vector3 spawnPos,bool life, int ammount)
    {
        isShot = false;
        aliveTime = 5;
        gameObject.GetComponent<Rigidbody>().Sleep();
        gameObject.transform.position = spawnPos;
        handler.GetComponent<PlayerLives>().HandleLife(life, ammount);
        isAlive = true;
    }



    public void setPos(Vector3 pos)
    {
        //Needs to change to fire point
        firePos = pos;
    }

    public void Shoot()
    {
        isShot = true;
        handler.GetComponent<PlayerLives>().Shot();
    }
        

}
