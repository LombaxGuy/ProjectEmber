using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combustion : Powerup {

    private GameObject combustionPrefab;

    private GameObject player;

    private bool canUsePowerup;

    // Use this for initialization
    void Start()
    {
        canUsePowerup = false;
        player = GameObject.FindGameObjectWithTag("Projectile");
        combustionPrefab = Resources.Load("Powerups/CombustionEffect") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(canUsePowerup == true)
        {
            PowerupInUse();
        }
    }

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += PowerupCanBeUsed;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= PowerupCanBeUsed;
    }

    private void PowerupCanBeUsed(Vector3 direction, float forceStrength)
    {
        canUsePowerup = true;
    }

    //When the player touches the screen, if canUsePower == true, then the powerup can be used.
    private void PowerupInUse()
    {
        //Get touch
        if (Input.touchCount >= 1 && canUsePowerup == true)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // use powerup
                InstantiatePrefabs();
                ResetValues();
            }
        }

        if(Input.GetKey(KeyCode.Keypad2) && canUsePowerup == true)
        {
            InstantiatePrefabs();
            ResetValues();
        }
    }

    //Getting the combusion prefab and instantiate it at the players position.
    private void InstantiatePrefabs()
    {
        GameObject temp = Instantiate(combustionPrefab);
        temp.transform.position = player.transform.position;
    }

    public override void ResetValues()
    {
        //Do shit before destroying?
        GameObject.Destroy(gameObject);
    }

    public override void UsePowerup()
    {
        canUsePowerup = false;
        player = GameObject.FindGameObjectWithTag("Projectile");
        combustionPrefab = Resources.Load("Powerup/CombustionEffect") as GameObject;
    }

    public override void NextTurn()
    {
        throw new NotImplementedException();
    }
}
