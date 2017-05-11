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

    #region Level
    [Header("Level Setup")]

    [Tooltip("The y-coordinate of the highest point of the map.")]
    [SerializeField]
    private float topOfLevelYCoordinate = 20;

    [Tooltip("The number of rounds befor the water starts rising.")]
    [SerializeField]
    private int roundsBeforeWaterRise = 3;

    [Tooltip("The number of rounds it takes for the water to rise to the top of the map. (After the above numer of rounds)")]
    [SerializeField]
    private int roundsAfterWaterRise = 5;

    [ReadOnly]
    [SerializeField]
    private int roundsPassed = 0;

    private bool levelEnded = false;

    public float TopOfLevelYCoordinate
    {
        get { return topOfLevelYCoordinate; }
        set { topOfLevelYCoordinate = value; }
    }

    public int RoundsBeforeWaterRising
    {
        get { return roundsBeforeWaterRise; }
        set { roundsBeforeWaterRise = value; }
    }

    public int RoundsAfterWaterRising
    {
        get { return roundsAfterWaterRise; }
        set { roundsAfterWaterRise = value; }
    }

    public int RoundsPassed
    {
        get { return roundsPassed; }
        set { roundsPassed = value; }
    }

    public bool LevelEnded
    {
        get { return levelEnded; }
        set { levelEnded = value; }
    }
    #endregion

    private void OnEnable()
    {
        EventManager.OnStartOfTurn += OnStartOfTurn;
        EventManager.OnEndOfTurn += OnEndOfTurn;
        EventManager.OnGameWorldReset += OnReset;
    }

    private void OnDisable()
    {
        EventManager.OnStartOfTurn -= OnStartOfTurn;
        EventManager.OnEndOfTurn -= OnEndOfTurn;
        EventManager.OnGameWorldReset -= OnReset;
    }

    private void OnStartOfTurn()
    {
        
    }

    private void OnEndOfTurn()
    {

    }

    private void OnReset()
    {
        levelEnded = false;
        RoundsPassed = 0;
    }
}
