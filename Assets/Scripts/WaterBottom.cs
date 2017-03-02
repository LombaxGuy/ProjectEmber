using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottom : MonoBehaviour {

    Transform go;

	// Use this for initialization
	void Start () {
        go = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Projectile")
            other.gameObject.GetComponent<ProjectileLife>().Fizzle(false,1);
        
    }

    public void Creep(int time)
    {
        for (int i = 0; i < time; i++)
        {
            go.Translate(Vector3.up * Time.deltaTime);
        }
        
    }
}
