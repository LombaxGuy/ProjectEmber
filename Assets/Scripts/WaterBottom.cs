using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottom : MonoBehaviour {

    Transform go;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    int health = 3;

    Vector3 currentPossition;
    Vector3 targetPossition;
    [SerializeField]
    Vector3 checkpointPossition;

    [SerializeField]
    private bool traveling;

    float t = 0.0f;

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;
        EventManager.OnProjectileDead += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShot;
        EventManager.OnProjectileDead -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }

    private void OnShot(Vector3 dir, float force)
    {
       
    }

    private void OnDeath(int health)
    {
        Creep();
    }

    private void OnIgnite(int health)
    {

    }

    // Use this for initialization
    void Start () {
        go = gameObject.transform;
        checkpointPossition = projectile.transform.position;
        targetPossition = checkpointPossition;


	}
	
	// Update is called once per frame
	void Update () {

        //Updating currentPos;
        currentPossition = go.position;

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
        go.position = new Vector3(go.position.x, Mathf.Lerp(currentPossition.y, targetPossition.y, t), go.position.z);
        if (go.position.y >= targetPossition.y - 0.01f)
        {
            traveling = false;
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
}
