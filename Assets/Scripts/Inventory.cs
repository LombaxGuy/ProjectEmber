using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    //Players inventory that saves powerups count and coins that the player owns

    private static int supernova;

    private static int combustion;

    private static int glue;

    private static int glitter;

    private static int bombardment;

    private static int napalm;

    private static int flamethrower;

    private static int coins;

    public static int Supernova
    {
        get { return supernova; }
        set { supernova = value; }
    }

    public static int Combustion
    {
        get { return combustion; }
        set { combustion = value; }
    }

    public static int Glue
    {
        get { return glue; }
        set { glue = value; }
    }
   
    public static int Glitter
    {
        get { return glitter; }
        set { glitter = value; }
    }

    public static int Bombardment
    {
        get { return bombardment; }
        set { bombardment = value; }
    }

    public static int Napalm
    {
        get { return napalm; }
        set { napalm = value; }
    }

    public static int Flamethrower
    {
        get { return flamethrower; }
        set { flamethrower = value; }
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
