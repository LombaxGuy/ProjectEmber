using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {

    Vector3 spawnPoint;
    

    // Use this for initialization
    void Start () {
		spawnPoint = gameObject.transform.GetChild(0).transform.position;
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Projectile")
        {             
            other.gameObject.GetComponent<ProjectileLife>().Fizzle(spawnPoint,true, 1);
            other.gameObject.GetComponent<ProjectileLife>().setPos(spawnPoint);
            gameObject.GetComponent<Collider>().enabled = false;
        }


    }
}
