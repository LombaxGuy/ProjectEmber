using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPowerUp : Powerup {

    private GameObject player;

    private float oldValue;
    private float newValue;

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void PreStart()
    {
        Turns = 3;
        PowerUps = Powerups.PowerShot;
        player = GameObject.FindGameObjectWithTag("Projectile");
        Debug.Log("player name : " + player.name);
        newValue = 0;
    }

    public override void UsePowerup()
    {
        PreStart();
        oldValue = player.GetComponent<AudioSource>().volume;
        player.GetComponent<AudioSource>().volume = newValue;       
    }

    private void OnEnable()
    {
        EventManager.OnProjectileRespawn += NextTurn;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileRespawn -= NextTurn;
    }


    public override void NextTurn()
    {

        if (Turns > 0)
        {
            Turns--;
            if (Turns == 0)
            {
                ResetValues();
            }
        }
        Debug.Log("Turns : " + Turns);
    }

    public override void ResetValues()
    {
        player.GetComponent<AudioSource>().volume = oldValue;
        //Inventory fjerner man en af powerups
        GameObject.Destroy(gameObject);
    }
}
