using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRising : MonoBehaviour
{

    [SerializeField]
    private GameObject currentCheckpoint;

    private Vector3 startPosition;
    private Vector3 goalPosition;
    private Vector3 currentPosition;

    [SerializeField]
    private int travelSteps = 3;

    [SerializeField]
    private float[] stepPositions;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float calculatedDistance;

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

    private void OnDeath(int health)
    {
        // move one step

        travelSteps = travelSteps - 1;
        StartCoroutine(MoveIt());
    }

    private void OnIgnite(int health, Vector3 newCheckpoint)
    {
        // set new target pos
        // get current amount of lives
        // calculate increments
        // move water one step

        travelSteps = travelSteps + 1;
        goalPosition = newCheckpoint;
        SetTarget();
        udregn();
        StartCoroutine(MoveIt());

    }


    // Use this for initialization
    private void Start()
    {
        startPosition = gameObject.transform.position;
        goalPosition = currentCheckpoint.transform.position;
        travelSteps = 3;
        stepPositions = new float[travelSteps + 2];
        distance = Vector3.Distance(new Vector3(startPosition.x, startPosition.y, startPosition.z), new Vector3(startPosition.x, goalPosition.y, startPosition.z));
        udregn();
    }

    // Update is called once per frame
    private void Update()
    {
        currentPosition = gameObject.transform.position;
    }


    //All of this needs a rework hlyshit this is bad



    private void SetTarget()
    {
        startPosition = gameObject.transform.position;
        stepPositions = new float[travelSteps + 2];
        distance = Vector3.Distance(new Vector3(startPosition.x, startPosition.y, startPosition.z), new Vector3(startPosition.x, goalPosition.y, startPosition.z));
    }

    private void udregn()
    {
        calculatedDistance = (distance / travelSteps);
        int f = 0;
        for (int i = (travelSteps + 1); i >= 0; i--)
        {

            stepPositions[i] = startPosition.y + (calculatedDistance * f);

            f++;
        }
    }

    private IEnumerator MoveIt()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 2f;
            gameObject.transform.position = new Vector3(startPosition.x, Mathf.Lerp(currentPosition.y, stepPositions[travelSteps], t), startPosition.z);
            yield return null;
        }
    }
}
