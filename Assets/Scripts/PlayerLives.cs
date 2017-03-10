using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    //The Total lives left of the player
    [SerializeField]
    int lives = 3;

    //the bottom water object this might need to be moved
    [SerializeField]
    GameObject water;

    /// <summary>
    /// Subscribed events
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnGameWorldReset += OnWorldReset;
    }
    /// <summary>
    /// Subscribed events
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnGameWorldReset += OnWorldReset;
    }

    /// <summary>
    /// Resets the lives.
    /// </summary>
    private void OnWorldReset()
    {
        lives = 3;
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
    private void OnIgnite(int amount)
    {
        HandleLife(amount);
    }

    /// <summary>
    /// Contains the kill condition for the whole of the game not made to work yet
    /// </summary>
    void Update()
    {
        if (lives <= 0)
        {
            Debug.Log("PLayerLives.cs: GAMEOVER! Resetting level...");

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
        lives = lives + amount;
    }
}
