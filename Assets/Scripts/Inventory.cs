using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    //Players inventory that saves powerups count and coins that the player owns

    private static int powerupOne;

    private static int powerupTwo;

    private static int powerupThree;

    private static int coins;

    public static int PowerupOne
    {
        get { return powerupOne; }
        set { powerupOne = value; }
    }

    public static int PowerupTwo
    {
        get { return powerupTwo; }
        set { powerupTwo = value; }
    }

    public static int PowerupThree
    {
        get { return powerupThree; }
        set { powerupThree = value; }
    }
   
    public static int Coins
    {
        get { return coins; }
        set { coins = value; }
    }
    

    //Metoder til fremtiden. Som skal sørge for at gemme inventory variablerne til et sted, enten lokalt eller i skyen.
    public void SaveInventoryLocal()
    {

    }

    public void SaveInvenotryCloud()
    {

    }

    public void LoadInventoryLocal()
    {

    }

    public void LoadInventoryCloud()
    {

    }




}
