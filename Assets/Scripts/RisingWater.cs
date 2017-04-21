using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private WorldManager worldManager;
    private Vector3[] steps;

    private Vector3 currentGoal;
    private float distanceToGoal;
    
    private void OnEnable()
    {
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
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

        currentGoal = checkpoint;

        CalculateSteps();
    }

    // Use this for initialization
    private void Start()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();

        CalculateSteps();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void MoveWater()
    {
        transform.position = steps[worldManager.CurrentLives];
    }

    private void CalculateSteps()
    {
        steps = new Vector3[worldManager.CurrentLives];

        distanceToGoal = Vector3.Distance(transform.position, currentGoal);

        for (int i = 0; i <= worldManager.CurrentLives; i++)
        {
            steps[i - 1] = transform.position + new Vector3(0, distanceToGoal * i);
        }

        steps[worldManager.CurrentLives] += new Vector3(0, 50);
    }
}
