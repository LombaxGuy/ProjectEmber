using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

    //UI skal have noget information om hvilken slags powerup der er valgt som powerupmanager kan benyttes af.
    //Instianteres af empty gameobjects i CreatePowerup metoden, muligvis er det en event som giver information og ikke UI?


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            CreatePowerup(Powerup.Powerups.Combustion);
        }
	}
    /// <summary>
    /// Instatiate powerup after the player choice
    /// </summary>
    /// <param name="powerups"></param>
    public void CreatePowerup(Powerup.Powerups powerups)
    {
        GameObject temp = null;
        switch (powerups)
        {
            case Powerup.Powerups.Test:
                temp = Instantiate(Resources.Load("Powerups/TestPowerup") as GameObject);
                temp.transform.position = transform.position;
                break;
            case Powerup.Powerups.Supernova:
                temp = Instantiate(Resources.Load("Powerups/SupernovaPowerup") as GameObject);
                temp.transform.position = transform.position;
                break;

            case Powerup.Powerups.Combustion:
                temp = Instantiate(Resources.Load("Powerups/CombustionPowerup") as GameObject);
                temp.transform.position = transform.position;
                break;
            case Powerup.Powerups.Glue:
                temp = Instantiate(Resources.Load("Powerups/GluePowerup") as GameObject);
                temp.transform.position = transform.position;
                break;
            default:
                break;
        }

        if(temp != null)
        {
            temp.GetComponent<Powerup>().UsePowerup();
        }
        

    }
}
