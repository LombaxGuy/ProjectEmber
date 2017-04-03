using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour {

    //private bool alive;
    private Vector3 startPos;

    [SerializeField]
    private float lifeTime = 2;

	// Use this for initialization
	void Start () {
        startPos = gameObject.transform.position;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = startPos;
    }

    private IEnumerator Fade()
    {
        float t = 0;

        while(gameObject.activeInHierarchy)
        {
            t += Time.deltaTime / 2;

            if (t > lifeTime)
            {
                gameObject.SetActive(false);
                gameObject.transform.position = startPos;
            }
            yield return null;
        }

    }
}
