using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private WorldManager worldManager;

    [SerializeField]
    private float topOfMap = 20;

    [SerializeField]
    private int roundsBeforeStart = 3;

    [SerializeField]
    private int roundsAfterStart = 5;

    [SerializeField]
    private int currentRoundsInTotal = 0;

    private float waterRiseTime = 1.5f;

    [SerializeField]
    float deltaY;
    [SerializeField]
    float increment;
    [SerializeField]
    float currentY;

    float oldY;

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
        oldY = transform.position.y;
        currentY = increment * (currentRoundsInTotal - roundsBeforeStart - 1);

        StartCoroutine(CoroutineMove());

        //transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

        //if (steps.Length >= 1)
        //{
        //    transform.position = new Vector3(transform.position.x, steps[worldManager.CurrentLives - 1], transform.position.z);
        //}
    }

    private IEnumerator CoroutineMove()
    {
        float t = 0;
        float s = 0;

        while (t < 1)
        {
            t += Time.deltaTime / waterRiseTime;

            s = 0.5f * Mathf.Sin((t - 0.5f) / (1 / Mathf.PI)) + 0.5f;

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(oldY, currentY, s), transform.position.z);

            yield return null;
        }
    }
}
