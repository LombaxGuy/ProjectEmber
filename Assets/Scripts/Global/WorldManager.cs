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

    #region Player Lives
    [Tooltip("The number of lives the player has at the start of the level.")]
    [SerializeField]
    private int lives = 3;

    // The current number of lives the player has.
    [SerializeField]
    private int currentLives;

    public int CurrentLives
    {
        get { return currentLives; }
        set { currentLives = value; }
    }
    #endregion

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnGameWorldReset += OnWorldReset;
    }
    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnGameWorldReset -= OnWorldReset;
    }

    private void Awake()
    {
        currentLives = lives;
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// Resets the lives.
    /// </summary>
    private void OnWorldReset()
    {
        currentLives = lives;
    }

    /// <summary>
    /// Called when a projectile dies
    /// </summary>
    private void OnDeath(int amount)
    {
        HandleLife(amount);
    }

    /// <summary>
    /// Called when a projectile ignites a flammable surface.
    /// </summary>
    private void OnIgnite(int amount, Vector3 newCheckpoint)
    {
        HandleLife(amount);
    }

    /// <summary>
    /// Contains the kill condition for the whole of the game not made to work yet
    /// </summary>
    void Update()
    {
        if (currentLives <= 0)
        {
            Debug.Log("WorldManager.cs: GAMEOVER! Resetting level...");

            // Invokes the on GameWorldReset event.
            EventManager.InvokeOnGameWorldReset();
        }
    }

    /// <summary>
    /// Used to change the players current lives (shot attempts)
    /// </summary>
    /// <param name="amount">Amount of lives to increase or decrease with.</param>
    public void HandleLife(int amount)
    {
        currentLives = currentLives + amount;
    }
}
