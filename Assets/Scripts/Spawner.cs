using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private GameObject spawnMark;
    [SerializeField]
    private GameObject targetMark;

    [SerializeField]
    private Vector3 createPos;
    [SerializeField]
    private Vector3 targetPos;
    [SerializeField]
    private Vector3 dir;

    [SerializeField]
    private GameObject[] projectiles;

    //Settings
    private float launchSpeed;
    [Header("Settings")]
    [SerializeField]
    private int p_MaxP;
    [SerializeField]
    private float interval;

    private bool spawnerOn = false;

    [SerializeField]
    private int currentProjectile = 0;

    //Projetile
    [SerializeField]
    private GameObject projectile;

	// Use this for initialization
	void Start () {

        createPos = spawnMark.transform.position;
        targetPos = targetMark.transform.position;
        dir = createPos + targetPos;
        projectiles = new GameObject[p_MaxP];
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = Instantiate(projectile, createPos, Quaternion.identity);
            projectiles[i].SetActive(false);
        }
        Launch();
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyUp(KeyCode.G))
        //{
        //    spawnerOn = !spawnerOn;
        //    Launch();
            
        //}        
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
    /// This is for if we need to fling water in a direction
    /// </summary>
    /// <returns></returns>
    private IEnumerator move()
    {
        float t = 0;
        Debug.Log("called");
        while (t < 1)
        {
            t += Time.deltaTime / 2;
            projectiles[0].GetComponent<Rigidbody>().AddForce(dir.normalized, ForceMode.Acceleration);
            yield return null;
        }
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
