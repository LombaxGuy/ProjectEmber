using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3 spawnPosition;
    private GameObject[] spawnObjects;

    //Settings
    [Header("Settings")]

    [Tooltip("The maximum number of objects in the array.")]
    [SerializeField]
    private int maxObjectCount = 5;

    [SerializeField]
    private float interval = 0.5f;

    //Can be used to turn spawning on and off if that is needed in future map creation
    private bool spawnerOn = false;

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

        spawnerCoroutine = StartCoroutine(CoroutineSpawn());


        Launch();
    }

    /// <summary>
    /// USed to launch the coroutine
    /// </summary>
    private void Launch()
    {
        spawnerOn = !spawnerOn;
        StartCoroutine("Drip");
    }


    /// <summary>
    /// Handles the spawning of the projectiles
    /// </summary>
    /// <returns>Waits for the interval</returns>
    private IEnumerator CoroutineSpawn()
    {
        float t = 0;
        while (spawnerOn)
        {
            t += Time.deltaTime / 2;

            if (currentProjectile == maxObjectCount)
            {
                currentProjectile = 0;
                spawnObjects[currentProjectile].SetActive(true);
                spawnObjects[currentProjectile].GetComponent<Droplet>().StartCoroutine("Fade");
                currentProjectile++;

            }
            else
            {
                spawnObjects[currentProjectile].SetActive(true);
                spawnObjects[currentProjectile].GetComponent<Droplet>().StartCoroutine("Fade");
                currentProjectile++;

            }
            yield return new WaitForSeconds(interval);
        }
    }
}
