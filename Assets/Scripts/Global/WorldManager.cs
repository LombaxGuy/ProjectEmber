using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region Flame Management
    private GameObject activeFlame;

    public GameObject ActiveFlame
    {
        get { return activeFlame; }
        set { activeFlame = value; }
    }
    #endregion

}
