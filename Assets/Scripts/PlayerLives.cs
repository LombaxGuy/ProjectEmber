using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour {

    int lives = 3;


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
}
