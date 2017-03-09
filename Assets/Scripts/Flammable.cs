using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {

    [SerializeField]
    private int health;

    public int Health
    {
        get
        {
            return health;
        }
        
    }

    private void OnEnable()
    {

        EventManager.OnGameWorldReset += OnWorldReset;
    }

    private void OnDisable()
    {

        EventManager.OnGameWorldReset -= OnWorldReset;
    }

    private void OnWorldReset()
    {
        Reset();
    }

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Reset()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

}
