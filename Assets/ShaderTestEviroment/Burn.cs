using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour {

    [SerializeField]
    float burntscale = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", burntscale);
	}
}
