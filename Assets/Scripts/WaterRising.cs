using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRising : MonoBehaviour
{

    [SerializeField]
    GameObject currentCheckpoint;

    Vector3 startPosition;
    Vector3 goalPosition;
    Vector3 currentPosition;

    [SerializeField]
    int travelSteps = 3;



    [SerializeField]
    float[] stepPositions;
    [SerializeField]
    float distance;
    [SerializeField]
    float calculatedDistance;

    public bool go;

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnProjectileRespawn += OnProRespawn;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShot;
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnProjectileRespawn -= OnProRespawn;
    }

    private void OnShot(Vector3 dir, float force)
    {

    }

    private void OnDeath(int health)
    {
        travelSteps = travelSteps - 1;
        StartCoroutine(MoveIt());

    }

    private void OnIgnite(int health, Vector3 newCheckpoint)
    {
        travelSteps = travelSteps + 1;
        goalPosition = newCheckpoint;
        SetTarget();
        udregn();
        StartCoroutine(MoveIt());

    }

    private void OnProRespawn()
    {

    }


    // Use this for initialization
    void Start()
    {
        startPosition = gameObject.transform.position;
        goalPosition = currentCheckpoint.transform.position;
        travelSteps = 3;
        stepPositions = new float[travelSteps + 2];
        distance = Vector3.Distance(new Vector3(startPosition.x, startPosition.y, startPosition.z), new Vector3(startPosition.x, goalPosition.y, startPosition.z));
        udregn();
    }

    // Update is called once per frame
    void Update()
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
