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
    [Header("Momspagetti")]
    [SerializeField]
    private int p_MaxP;
    [SerializeField]
    private float p_LifeTime = 2;
    [SerializeField]
    private float interval;

    private bool spawnerOn = false;

    private int currentProjectile = 0;
    private int currentProjectileCount = 0;

    //Projetile
    [SerializeField]
    private GameObject projectile;
        
    private float p_Dur;

	// Use this for initialization
	void Start () {

        createPos = spawnMark.transform.position;
        targetPos = targetMark.transform.position;
        dir = createPos + targetPos;
        projectiles = new GameObject[p_MaxP];

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.G))
        {
            spawnerOn = !spawnerOn;
            Launch();
            
        }
        
    }

    private void Prep()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            
        }
    }


    private void Launch()
    {

        //projectiles[0] = Instantiate(projectile, createPos, Quaternion.identity);
        StartCoroutine("Drip");
        
    }

    private IEnumerator move()
    {
        float t = 0;
        Debug.Log("called");
        while (t < 1)
        {
            t += Time.deltaTime / 2;
            projectiles[0].GetComponent<Rigidbody>().AddForce(dir.normalized,ForceMode.Acceleration);
            yield return null;
        }
       
    }

    private IEnumerator Fade(GameObject go)
    {
        float t = 0;
        
            while (go != null)
            {
                t += Time.deltaTime / 2;
                if (t > p_LifeTime)
                {
                Destroy(go);
                currentProjectileCount--;
                }
                yield return null;
            }        
        yield return null;
    }

    private IEnumerator Drip()
    {
        float t = 0;

        while (spawnerOn)
        {
            t += Time.deltaTime / 2;

            //Spawn
            if (currentProjectileCount < p_MaxP)
            {
                if (currentProjectile == p_MaxP)
                {
                    currentProjectile = 0;
                    projectiles[currentProjectile] = Instantiate(projectile, createPos, Quaternion.identity);
                    StartCoroutine(Fade(projectiles[currentProjectile]));
                    currentProjectileCount++;
                    
                    yield return new WaitForSeconds(interval);
                }
                else
                {
                    projectiles[currentProjectile] = Instantiate(projectile, createPos, Quaternion.identity);
                    StartCoroutine(Fade(projectiles[currentProjectile]));
                    currentProjectile++;
                    currentProjectileCount++;
                    yield return new WaitForSeconds(interval);
                }
                yield return null;
            }

            yield return null;
        }

    }

}
