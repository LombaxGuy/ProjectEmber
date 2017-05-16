using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField]
    private GameObject breakObjectPrefab;
    private GameObject[] breakObjects;
    private MiniRock[] miniRocks;

    private Vector3 startPos;

    private Rigidbody rockBody;

    // Use this for initialization
    private void Start()
    {
        breakObjectPrefab = Instantiate(breakObjectPrefab, Vector3.zero, Quaternion.identity);
        breakObjectPrefab.transform.SetParent(transform.parent);
        breakObjectPrefab.SetActive(false);

        rockBody = GetComponent<Rigidbody>();

        startPos = gameObject.transform.position;

        int childCount = breakObjectPrefab.transform.childCount;

        breakObjects = new GameObject[childCount];
        miniRocks = new MiniRock[childCount];

        for (int i = 0; i < childCount; i++)
        {
            breakObjects[i] = breakObjectPrefab.transform.GetChild(i).gameObject;
            miniRocks[i] = breakObjectPrefab.transform.GetChild(i).GetComponent<MiniRock>();
        }
    }

    private void OnEnable()
    {
        Reset();
    }

    /// <summary>
    /// Spawns the small rocks and creates an explosion.
    /// </summary>
    private void Split(float explosionForce, Vector3 explosionPoint)
    {
        gameObject.SetActive(false);

        breakObjectPrefab.transform.position = transform.position;
        breakObjectPrefab.SetActive(true);

        for (int i = 0; i < breakObjectPrefab.transform.childCount; i++)
        {
            breakObjects[i].SetActive(true);
            miniRocks[i].Explosion(explosionForce, explosionPoint);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Vector3 point;

        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            point = other.contacts[0].point;
            Split(CalculateExplosiveForce(point), point);
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
    private float CalculateExplosiveForce(Vector3 collisionPoint)
    {
        float explosiveForce = (startPos.y - collisionPoint.y) * 10;

        return explosiveForce;
    }
}
