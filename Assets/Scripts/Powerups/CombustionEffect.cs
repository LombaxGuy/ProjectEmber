using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombustionEffect : MonoBehaviour {
    
    private List<GameObject> flames;

    private float time;

    private WorldManager worldmanager;

    private Flame flame;

	// Use this for initialization
	void Start ()
    {
        worldmanager = GameObject.Find("World").GetComponent<WorldManager>();
        flame = worldmanager.ActiveFlame.GetComponent<Flame>();
        flames = new List<GameObject>();
        time = 2;
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (time <= 0)
        {
            if(flames.Count != 0)
            {
                GameObject tempFlammableSpawn = null;

                for (int i = 0; i < flames.Count; i++)
                {

                    EventManager.InvokeOnProjectileIgnite(flames[i].GetComponent<Flammable>());

                    if (i >= 1)
                    {
                        if (flames[i].transform.position.y > flames[i - 1].transform.position.y)
                        {
                            tempFlammableSpawn = flames[i];
                        }
                    }
                    else
                    {
                        tempFlammableSpawn = flames[i];
                    }
                }

                if (tempFlammableSpawn != null)
                {
                    flame.SpawnPoint = tempFlammableSpawn.transform.position;
                }
            }
            
            GameObject.Destroy(gameObject);

        }
        else
        {
            time -= Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "FlammableObject")
        {
            Debug.Log("HELLO");
            Flammable temp = null;
            try
            {
                temp = other.gameObject.GetComponent<Flammable>();
            }
            catch
            {
                Debug.LogError("CombustionEffect.cs: Collision object does not have a SpawnPoint child object even though it is tagged as a FlammableObject.");
            }
            if(temp != null)
            {
                flames.Add(other.gameObject);
            }
            
        }
    }
}
