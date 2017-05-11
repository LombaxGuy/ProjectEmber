using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {

    public enum Powerups { Test, Supernova, Combustion, Glue }

    private int turns;
    private Powerups powerUps;
    //Variabel med det forskellige objecter der skal ændres på

    public int Turns
    {
        get { return turns; }
        set { turns = value; }
    }

    public Powerups PowerUps
    {
        get { return powerUps; }
        set { powerUps = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   

    public abstract void UsePowerup();

    public abstract void NextTurn();

    public abstract void ResetValues();


}
