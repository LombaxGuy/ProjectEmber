using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private GameObject spawnMark;
    [SerializeField]
    private Vector3 createPos;
    [SerializeField]
    private GameObject[] projectiles;

    //Settings
    [Header("Settings")]
    [SerializeField]
    private int p_MaxP;
    [SerializeField]
    private float interval;

    //Can be used to turn spawning on and off if that is needed in future map creation
    private bool spawnerOn = false;

    [SerializeField]
    private int currentProjectile = 0;

    //Projetile
    [SerializeField]
    private GameObject projectile;

	// Use this for initialization
	void Start () {

        createPos = spawnMark.transform.position;
        projectiles = new GameObject[p_MaxP];
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = Instantiate(projectile, createPos, Quaternion.identity);
            projectiles[i].SetActive(false);
        }
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
    private IEnumerator Drip()
    {
        float t = 0;
        while (spawnerOn)
        {
            t += Time.deltaTime / 2;

                if (currentProjectile == p_MaxP)
                {
                    currentProjectile = 0;
                    projectiles[currentProjectile].SetActive(true);
                    projectiles[currentProjectile].GetComponent<Droplet>().StartCoroutine("Fade");
                    currentProjectile++;
    
                }
                else
                {
                projectiles[currentProjectile].SetActive(true);
                projectiles[currentProjectile].GetComponent<Droplet>().StartCoroutine("Fade");
                currentProjectile++;

                }
                yield return new WaitForSeconds(interval);
        }
    }
}
