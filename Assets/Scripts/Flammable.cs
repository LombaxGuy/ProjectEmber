using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Projectile")
        {
            other.gameObject.GetComponent<ProjectileLife>().FirePos = gameObject.transform.GetChild(0).transform.position;
            other.gameObject.GetComponent<ProjectileLife>().Fizzle(true, 1);
        }


    }
}
