using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottom : MonoBehaviour {

    Transform go;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    int hp = 3;

    Vector3 startPosition;

    Vector3 currentPossition;

    Vector3 currentTargetPossition;

    [SerializeField]
    Vector3 checkpointPossition;

    [SerializeField]
    private bool traveling;

    float t = 0.0f;


    [SerializeField]
    float point;

    float[] points;
    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShot;
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }

    private void OnShot(Vector3 dir, float force)
    {
        
    }

    private void OnDeath(int health)
    {
        Creep();
        //pos();
        hp = hp - 1;
    }

    private void OnIgnite(int health, Vector3 newCheckpoint)
    {
        PossitionRecalc();
        //pos();
    }

    // Use this for initialization
    void Start () {
        go = gameObject.transform;
        checkpointPossition = projectile.transform.position;
        startPosition = go.position;
        
        //targetPossition = checkpointPossition;


	}
	
	// Update is called once per frame
	void Update () {

        //Updating currentPos;
        currentPossition = go.position;
        currentTargetPossition = new Vector3(go.position.x, point, go.position.z);



        //Movement
        if (traveling)
        {
            Rise();
        }
    }

    /// <summary>
    /// Handles water rising whenever traveling is true
    /// </summary>
    private void Rise()
    {

        t += 0.1f * Time.deltaTime;
        go.position = new Vector3(go.position.x, Mathf.Lerp(currentPossition.y, point, t), go.position.z);
        if (currentPossition.y >= currentTargetPossition.y - 0.01f)
        {
            traveling = false;
            Debug.Log("moved");
        }
    }

    /// <summary>
    /// Called when projectile dies enables traveling sequence
    /// </summary>
    private void Creep()
    {
        t = 0;
        traveling = true;

    }

    private void PossitionRecalc()
    {

        for (int i = 0; i < hp; i++)
        {
            if (i > 0)
            {
                points[i] = ((startPosition.y + checkpointPossition.y) / i);
            }
            else
            {
                points[i] = checkpointPossition.y;
            }

        }

    }

    private void pos()
    {
        point = points[hp];
    }
}
