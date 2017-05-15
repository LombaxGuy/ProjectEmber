using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField]
    private GameObject breakObject;
    private GameObject[] breakObjects = new GameObject[4];

    private Vector3 startPos;

    private Rigidbody rockBody;

    private float explosionForce;
    private Vector3 explosionPoint;


    // Use this for initialization
    private void Start()
    {
        breakObject = Instantiate(breakObject, Vector3.zero, Quaternion.identity);
        breakObject.transform.SetParent(transform.parent);
        breakObject.SetActive(false);

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

    /// <summary>
    /// Spawns the small rocks and creates an explosion.
    /// </summary>
    private void Split()
    {
        breakObject.transform.position = transform.position;
        breakObject.SetActive(true);

        int childCount = breakObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            breakObject.transform.GetChild(i).gameObject.transform.position = breakObject.transform.position;
            breakObject.transform.GetChild(i).gameObject.SetActive(true);
            breakObject.transform.GetChild(i).GetComponent<MiniRock>().Explosion(explosionForce, explosionPoint);
        }

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            explosionPoint = other.contacts[0].point;
            CalculateExplosiveForce(explosionPoint);
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

    /// <summary>
    /// Sets the explosiveforce to a value depending on the distance the rock has fallen
    /// </summary>
    /// <param name="collisionPoint"></param>
    private void CalculateExplosiveForce(Vector3 collisionPoint)
    {
        explosionForce = (startPos.y - collisionPoint.y) * 25;
        Debug.Log(startPos.y);
        Debug.Log(collisionPoint.y);
        Debug.Log(explosionForce);
    }
}
