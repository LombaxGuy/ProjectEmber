using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {



	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

    }

  private IEnumerator CloseGates()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime / 4f;

        }  

    }
}
