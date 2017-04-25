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

    [SerializeField]
    private float topOfMap = 20;

    [SerializeField]
    private int roundsBeforeStart = 3;

    [SerializeField]
    private int roundsAfterStart = 5;

    [SerializeField]
    private int currentRoundsInTotal = 0;

    [SerializeField]
    float deltaY;
    [SerializeField]
    float increment;
    [SerializeField]
    float currentY;

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShoot;
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnGameWorldReset += OnReset;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShoot;
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnGameWorldReset -= OnReset;
    }

    private void OnShoot(Vector3 direction, float force)
    {
        currentRoundsInTotal++;
    }

    private void OnDeath(int amount)
    {
        //Move
        if (currentRoundsInTotal > roundsBeforeStart)
        {
            MoveWater();
        }
        
        // other stuff
    }

    private void OnIgnite(int amount, Vector3 checkpoint)
    {
        //Move
        if (currentRoundsInTotal > roundsBeforeStart)
        {
            MoveWater();
        }

        //currentGoal = checkpoint.y + offset;

        //CalculateSteps();
    }

    private void OnReset()
    {
        //InitializeVariables();
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

        deltaY = Vector3.Distance(transform.position, new Vector3(transform.position.x, topOfMap, transform.position.z));
        increment = deltaY / roundsAfterStart;

        //startGoal = worldManager.ActiveFlame.transform.position.y;

        //InitializeVariables(); 

    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void MoveWater()
    {
        currentY = increment * (currentRoundsInTotal - roundsBeforeStart - 1);

        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

        //if (steps.Length >= 1)
        //{
        //    transform.position = new Vector3(transform.position.x, steps[worldManager.CurrentLives - 1], transform.position.z);
        //}
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
