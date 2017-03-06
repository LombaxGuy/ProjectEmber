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
        EventManager.OnProjectileDead += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }
    /// <summary>
    /// Subscribed events
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnProjectileDead -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }


    /// <summary>
    /// Called when a projectile dies
    /// </summary>
    /// <param name="amount">none</param>
    private void OnDeath(int amount)
    {
        HandleLife(false, amount);
    }
    /// <summary>
    /// Called when a projectile ignites
    /// </summary>
    /// <param name="amount">none</param>
    private void OnIgnite(int amount)
    {
        HandleLife(true, amount);
    }

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// Contains the kill condition for the whole of the game not made to work yet
    /// </summary>
    void Update()
    {
        if (lives <= 0)
        {
            Debug.Log("Gameover");
            //(Application.Quit();
        }
    }

    /// <summary>
    /// Used to change the players current lives(shot attempts)
    /// </summary>
    /// <param name="life">True for addition, false for deduction</param>
    /// <param name="amount">amount to increase og decrease</param>
    public void HandleLife(bool life, int amount)
    {
        if (life)
            lives = lives + amount;

        if (!life)
            lives = lives - amount;
    }

}
