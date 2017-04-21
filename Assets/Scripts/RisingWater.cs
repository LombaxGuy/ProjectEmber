using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private WorldManager worldManager;

    [SerializeField]
    private float[] steps;

    private float startGoal;
    private float currentGoal;
    private float offset = 50;
    private float distanceToGoal;
    private float distanceToStep;

    private int currentIndex = 0;

    private void OnEnable()
    {
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnGameWorldReset += OnReset;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnGameWorldReset -= OnReset;
    }

    private void OnDeath(int amount)
    {
        //Move
        MoveWater();
        
        // other stuff
    }

    private void OnIgnite(int amount, Vector3 checkpoint)
    {
        //Move
        MoveWater();

        currentGoal = checkpoint.y + offset;

        CalculateSteps();
    }

    private void OnReset()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        currentGoal = startGoal;

        CalculateSteps();
    }

    // Use this for initialization
    private void Start()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();

        startGoal = worldManager.ActiveFlame.transform.position.y;

        InitializeVariables(); 
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void MoveWater()
    {
        if (steps.Length >= 1)
        {
            transform.position = new Vector3(transform.position.x, steps[worldManager.CurrentLives - 1], transform.position.z);
        }
    }

    private void CalculateSteps()
    {
        steps = new float[worldManager.CurrentLives];

        distanceToGoal = currentGoal - transform.position.y;
        distanceToStep = distanceToGoal / worldManager.CurrentLives;

        for (int i = 0; i < worldManager.CurrentLives; i++)
        {
            steps[i] = transform.position.y + distanceToStep * (i + 1);
        }
    }
}
