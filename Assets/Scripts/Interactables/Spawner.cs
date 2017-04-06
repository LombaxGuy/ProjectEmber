using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3 spawnPosition;
    private GameObject[] spawnObjects;

    //Settings
    [Header("Settings")]

    //Can be used to turn spawning on and off if that is needed in future map creation
    [Tooltip("Is the spawner turned on from the start.")]
    [SerializeField]
    private bool spawnerOn = true;

    [Tooltip("The maximum number of objects in the array.")]
    [SerializeField]
    private int maxObjectCount = 5;

    [SerializeField]
    private float interval = 0.5f;

    private bool isRunning = false;

    private int currentProjectile = 0;

    [Tooltip("The prefab that the spawner should spawn.")]
    [SerializeField]
    private GameObject spawnObject;

    private Coroutine spawnerCoroutine;

    // Use this for initialization
    void Start()
    {
        spawnPosition = transform.Find("SpawnPoint").position;

        spawnObjects = new GameObject[maxObjectCount];

        for (int i = 0; i < spawnObjects.Length; i++)
        {
            spawnObjects[i] = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            spawnObjects[i].SetActive(false);
        }

        if (spawnerOn)
        {
            StartSpawning();
        }
    }

    private void Update()
    {
        if (!spawnerOn && isRunning)
        {
            StopCoroutine(spawnerCoroutine);
            isRunning = false;
        }
        else if (spawnerOn && !isRunning)
        {
            spawnerCoroutine = StartCoroutine(CoroutineStartSpawning());
        }
    }

    /// <summary>
    /// Used to launch the coroutine
    /// </summary>
    public void StartSpawning()
    {
        spawnerOn = true;

        if (!isRunning)
        {
            spawnerCoroutine = StartCoroutine(CoroutineStartSpawning());
        }
    }

    public void StopSpawning()
    {
        spawnerOn = false;

        if (isRunning)
        {
            StopCoroutine(spawnerCoroutine);
            isRunning = false;
        }
    }

    /// <summary>
    /// Handles the spawning of the projectiles
    /// </summary>
    /// <returns>Waits for the interval</returns>
    private IEnumerator CoroutineStartSpawning()
    {
        isRunning = true;

        while (true)
        {
            if (currentProjectile >= maxObjectCount)
            {
                currentProjectile = 0;
            }

            spawnObjects[currentProjectile].SetActive(true);
            spawnObjects[currentProjectile].GetComponent<PipeDroplet>().StartCoroutine("Fade");
            currentProjectile++;

            yield return new WaitForSeconds(interval);
        }
    }
}
