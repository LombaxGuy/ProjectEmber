using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour {

    //private bool alive;
    private Vector3 startPos;

	// Use this for initialization
	void Start () {
        //alive = true;
        //StartCoroutine("Disperse");

        startPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = startPos;
    }

    //private IEnumerator Disperse()
    //{
    //    while (alive)
    //    {
           

    //        yield return new WaitForSeconds(0.5f);
    //    }

    //    yield return null;
    //}
}
