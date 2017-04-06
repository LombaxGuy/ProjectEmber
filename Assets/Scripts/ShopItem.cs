using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem {

    private bool unlocked;
    private int itemPosition;
    private string price;
    private bool equipped;

    public bool Unlocked
    {
        get{ return unlocked;}
        set{ unlocked = value;}
    }

    public int ItemPosition
    {
        get{ return itemPosition;}
        set{ itemPosition = value;}
    }

    public string Price
    {
        get{ return price;}
        set{ price = value;}
    }

    public bool Equipped
    {
        get{ return equipped;}
        set{ equipped = value;}
    }
}
