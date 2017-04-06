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

    /// <summary>
    /// Sets the values of the powerup before using it.
    /// </summary>
    private void PreStart()
    {
        Turns = 3;
        PowerUps = Powerups.PowerShot;
        player = GameObject.FindGameObjectWithTag("Projectile");
        Debug.Log("player name : " + player.name);
        newValue = 0;
    }

    /// <summary>
    /// This method makes the powerup do its thing
    /// </summary>
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

    /// <summary>
    /// This will make the powerup stop after there is no more turns.
    /// </summary>
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

    /// <summary>
    /// This method is runned as the last method before destroying the object. It will reset values back to the origin.
    /// </summary>
    public override void ResetValues()
    {
        player.GetComponent<AudioSource>().volume = oldValue;
        //Inventory fjerner man en af powerups
        GameObject.Destroy(gameObject);
    }
}
