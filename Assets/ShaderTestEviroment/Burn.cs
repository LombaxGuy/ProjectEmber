using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour {

    [SerializeField]
    [Range(0, 0.5f)]
    float burntscale = 0.5f;
    [SerializeField]
    float currentfloat;
    [SerializeField]
    [Range(0, 0.5f)]
    float newval;
    float f = 1;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        currentfloat = this.GetComponent<Renderer>().material.GetFloat("_DissolveAmount");
        if (Input.GetKeyUp(KeyCode.G))
        {
            StartCoroutine("Decay");
            Debug.Log("spand");
        }

        

    }

    private IEnumerator Decay()
    {
        float t = 0;

        
        while (currentfloat > 0)
        {
            Debug.Log("hej");
            t += Time.deltaTime / 4f;
            
            this.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", Mathf.Lerp(0.5f,0,t));
            yield return null;
        }


        yield return null;
    }
}
