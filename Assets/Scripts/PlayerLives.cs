using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour {

    [SerializeField]
    int lives = 3;
    [SerializeField]
    GameObject water;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(lives <= 0)
        {
            Debug.Log("Gameover");
            //(Application.Quit();
        }
	}

    public void HandleLife(bool life, int amount)
    {
        if(life)
        lives = lives + amount;

        if(!life)
        lives = lives - amount;      
    }

    public void Shot()
    {
        water.GetComponent<WaterBottom>().Creep(10);
    }
}
