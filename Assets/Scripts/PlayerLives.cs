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

    private void OnEnable()
    {
        EventManager.OnProjectileDead += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileDead -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }



    private void OnDeath()
    {
        HandleLife(false, 1);
    }

    private void OnIgnite()
    {
        HandleLife(true, 1);
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
