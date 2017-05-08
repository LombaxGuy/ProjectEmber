using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region Flame Management
    private GameObject activeFlame;

    private int roundsBeforeWaterRising = 5;
    private int roundsAfterWaterRising = 3;
    private int roundsPassed = 0;

    public GameObject ActiveFlame
    {
        get { return activeFlame; }
        set { activeFlame = value; }
    }

    public int RoundsBeforeWaterRising
    {
        get
        {
            return roundsBeforeWaterRising;
        }

        set
        {
            roundsBeforeWaterRising = value;
        }
    }

    public int RoundsAfterWaterRising
    {
        get
        {
            return roundsAfterWaterRising;
        }

        set
        {
            roundsAfterWaterRising = value;
        }
    }

    public int RoundsPassed
    {
        get
        {
            return roundsPassed;
        }

        set
        {
            roundsPassed = value;
        }
    }

    private void OnEnable()
    {
        EventManager.OnGameWorldReset += ResetValues;
    }

    private void OnDisable()
    {
        EventManager.OnGameWorldReset -= ResetValues;
    }

    private void ResetValues()
    {
        RoundsPassed = 0; 
    }

    #endregion

    #region Level
    private float topOfLevelYCoordinate = 20;

    public float TopOfLevelYCoordinate
    {
        get { return topOfLevelYCoordinate; }
        set { topOfLevelYCoordinate = value; }
    }

    #endregion

}
