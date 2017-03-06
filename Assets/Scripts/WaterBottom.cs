using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottom : MonoBehaviour {

    Transform go;

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;

    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShot;

    }

    private void OnShot(Vector3 dir, float force)
    {
        Creep(6);
    }

    // Use this for initialization
    void Start () {
        go = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Creep(int time)
    {
        for (int i = 0; i < time; i++)
        {
            go.Translate(Vector3.up * Time.deltaTime);
        }
        
    }
}
