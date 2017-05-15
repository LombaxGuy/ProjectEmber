using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3 spawnPoint;
    private GameObject[] spawnObjects;

    //Settings
    [Header("Settings")]

    //Can be used to turn spawning on and off if that is needed in future map creation
    [Tooltip("Is the spawner turned on from the start.")]
    [SerializeField]
    private bool spawnerEnabled = true;

    [Tooltip("The maximum number of objects in the array.")]
    [SerializeField]
    private int maxObjectCount = 5;

    [Tooltip("The interval in seconds at which the objects are spawned.")]
    [SerializeField]
    private float interval = 0.5f;

    private int currentProjectile = 0;

    [Tooltip("The prefab the spawner should spawn.")]
    [SerializeField]
    private GameObject spawnObject;

    private Coroutine spawnerCoroutine;
    private bool isRunning = false;

    // Use this for initialization
    private void Start()
    {
        // Finds the spawn point.
        spawnPoint = transform.Find("SpawnPoint").position;

        // Initializes the array.
        spawnObjects = new GameObject[maxObjectCount];

        // Adds objects to the array.
        for (int i = 0; i < spawnObjects.Length; i++)
        {
            spawnObjects[i] = Instantiate(spawnObject, spawnPoint, Quaternion.identity);
            spawnObjects[i].transform.SetParent(transform);
            spawnObjects[i].SetActive(false);
        }

        // Starts the spawner if 'spawnerEnabled' is true.
        if (spawnerEnabled)
        {
            StartSpawning();
        }
    }

    /// <summary>
    /// Checks if 'spawnerEnabled' is different from 'isRunning'
    /// </summary>
    private void Update()
    {
        // If the spawner is not enabled but the spawners coroutine is still running...
        if (!spawnerEnabled && isRunning)
        {
            //... the coroutine is stopped.
            StopCoroutine(spawnerCoroutine);
            isRunning = false;
        }
        // If the spawner is enabled but the spawners coroutine is not running...
        else if (spawnerEnabled && !isRunning)
        {
            //... the coroutine is started.
            spawnerCoroutine = StartCoroutine(CoroutineStartSpawning());
        }
    }

    /// <summary>
    /// Turns on the spawner.
    /// </summary>
    public void StartSpawning()
    {
        spawnerEnabled = true;

        if (!isRunning)
        {
            spawnerCoroutine = StartCoroutine(CoroutineStartSpawning());
        }
    }

    /// <summary>
    /// Turns off the spawner.
    /// </summary>
    public void StopSpawning()
    {
        spawnerEnabled = false;

        if (isRunning)
        {
            StopCoroutine(spawnerCoroutine);
            isRunning = false;
        }
    }

    /// <summary>
    /// Coroutine that spawns the objectes.
    /// </summary>
    private IEnumerator CoroutineStartSpawning()
    {
        // Is running is set to true.
        isRunning = true;

        // Starts an infinite loop that can only be stopped by stopping the coroutine.
        while (true)
        {
            if (currentProjectile >= maxObjectCount)
            {
                currentProjectile = 0;
            }

            // The object with the specified index is turned on.
            spawnObjects[currentProjectile].SetActive(true);
            currentProjectile++;

            // Waits for a specified amount of time before spawning a new object.
            yield return new WaitForSeconds(interval);
        }
    }
}
