using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRising : MonoBehaviour
{
    private WorldManager worldManager;

    [SerializeField]
    [Tooltip("The time in seconds it takes for the water to reach the next step.")]
    private float waterMoveTime = 2f;

    private GameObject currentFlameCheckpoint;

    private Vector3 startPosition;

    private Vector3 goalPosition;
    private Vector3 currentPosition;

    [SerializeField]
    private Vector3[] steps;

    private float distanceToGoal;
    private float distanceToNextStep;



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

    private void OnDeath(int health)
    {
        // move one step
        currentPosition = transform.position;

        StartCoroutine(MoveOneStep());
    }

    private void OnIgnite(int health, Vector3 newCheckpoint)
    {
        // set new target pos
        // get current amount of lives
        // calculate increments
        // move water one step

        goalPosition = newCheckpoint;
        currentPosition = transform.position;

        CalculateSteps(worldManager.CurrentLives);
        Debug.Log(worldManager.CurrentLives);

        StartCoroutine(MoveOneStep());
    }

    private void OnReset()
    {
        currentPosition = startPosition;
        goalPosition = currentFlameCheckpoint.transform.position;

        CalculateSteps(worldManager.CurrentLives);
    }


    // Use this for initialization
    private void Start()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();

        currentFlameCheckpoint = worldManager.ActiveFlame;

        startPosition = gameObject.transform.position;
        currentPosition = startPosition;

        goalPosition = currentFlameCheckpoint.transform.position;

        CalculateSteps(worldManager.CurrentLives);

        //distanceToGoal = Vector3.Distance(new Vector3(startPosition.x, startPosition.y, startPosition.z), new Vector3(startPosition.x, goalPosition.y, startPosition.z));
        //udregn();
    }

    // Update is called once per frame
    private void Update()
    {
        //currentPosition = gameObject.transform.position;
    }


    //All of this needs a rework hlyshit this is bad



    //private void SetTarget()
    //{
    //    distanceToGoal = Vector3.Distance(currentPosition, new Vector3(startPosition.x, goalPosition.y, startPosition.z));
    //}

    //private void udregn()
    //{
    //    distanceToNextPoint = (distanceToGoal / travelSteps);
    //    int f = 0;
    //    for (int i = (travelSteps + 1); i >= 0; i--)
    //    {

    //        steps[i] = startPosition.y + (distanceToNextPoint * f);

    //        f++;
    //    }
    //}

    private void CalculateSteps(int numberOfLives)
    {
        distanceToGoal = Vector3.Distance(currentPosition, new Vector3(currentPosition.x, goalPosition.y, currentPosition.z));

        distanceToNextStep = distanceToGoal / numberOfLives;

        //steps = new Vector3[numberOfLives];

        //for (int i = 0; i < numberOfLives; i++)
        //{
        //    steps[i] = currentPosition + new Vector3(0, distanceToNextStep)
        //}
    }

    private IEnumerator MoveOneStep()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / 2f;

            //gameObject.transform.position = new Vector3(startPosition.x, Mathf.Lerp(currentPosition.y, steps[travelSteps], t), startPosition.z);

            transform.position = Vector3.Lerp(currentPosition, currentPosition + new Vector3(0, distanceToNextStep), t);

            yield return null;
        }
    }
}
