using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField]
    private GameObject breakObject;
    private GameObject[] breakObjects = new GameObject[4];

    private Vector3[] spawnPointArray = new Vector3[4];
    private Vector3 spawnPoint1;
    private Vector3 spawnPoint2;
    private Vector3 spawnPoint3;
    private Vector3 spawnPoint4;

    private bool canBreak = true;

    private Vector3 startPos;

    private Rigidbody rockBody;

    private float explosionForce;
    private Vector3 explosionPoint;


    // Use this for initialization
    private void Start()
    {

        for (int i = 0; i < breakObjects.Length; i++)
        {
            breakObjects[i] = Instantiate(breakObject, new Vector3(0, 0, 0), Quaternion.identity);
            breakObjects[i].SetActive(false);
        }
        rockBody = GetComponent<Rigidbody>();

        startPos = gameObject.transform.position;

    }


    /// <summary>
    /// Used to reset the droplet when it is disabled.
    /// </summary>
    private void OnDisable()
    {
        Reset();
    }

    private void Split()
    {
        spawnPointArray[0] = new Vector3(transform.position.x - 0.20f, transform.position.y - 0.20f, transform.position.z);
        spawnPointArray[1] = new Vector3(transform.position.x + 0.20f, transform.position.y - 0.20f, transform.position.z);
        spawnPointArray[2] = new Vector3(transform.position.x - 0.20f, transform.position.y + 0.20f, transform.position.z);
        spawnPointArray[3] = new Vector3(transform.position.x + 0.20f, transform.position.y + 0.20f, transform.position.z);

        for (int i = 0; i < 4; i++)
        {
            breakObjects[i].SetActive(true);
            breakObjects[i].transform.position = spawnPointArray[i];
            breakObjects[i].GetComponent<MiniRock>().Explosion(explosionForce, explosionPoint);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint cP in other.contacts)
        {
            explosionPoint = cP.point;
            CalculateExplosiveForce(explosionPoint);

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            Split();
        }

    }




    /// <summary>
    /// Resets the droplets position and velocity.
    /// </summary>
    private void Reset()
    {
        // If the dropletBody is set...
        if (rockBody)
        {
            //... reset position and velocity.
            gameObject.transform.position = startPos;
            rockBody.velocity = Vector3.zero;
        }
    }

    private void CalculateExplosiveForce(Vector3 collisionPoint)
    {

        explosionForce = (startPos.y - collisionPoint.y) * 25;
    }
}
