using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    [SerializeField]
    private GameObject gate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Projectile")
        {
            for (int i = 0; i < gate.transform.childCount; i++)
            {
                if(gate.transform.GetChild(i).transform.name.Contains("Door"))
                {
                    gate.transform.GetChild(i).GetComponent<Gate>().OpenGate();

                }
            }
        }
    }
}
