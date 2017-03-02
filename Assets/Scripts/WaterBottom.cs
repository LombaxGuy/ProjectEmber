using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Projectile")
            other.gameObject.GetComponent<ProjectileLife>().Fizzle(false,1);
    }
}
