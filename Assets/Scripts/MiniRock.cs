using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRock : MonoBehaviour
{

    [SerializeField]
    private const float LIFETIME = 2f;
    private Rigidbody rb;

    private Vector3 startPosition;

    // Use this for initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    /// <summary>
    /// Used to start the timer coroutine when the droplet is activated.
    /// </summary>
    private void OnDisable()
    {
        transform.position = startPosition;
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            StartCoroutine(CoroutineTimer());
        }
    }

    /// <summary>
    /// Coroutine that waits for a set amount of time and then disables the droplet.
    /// </summary>
    /// <param name="time">Time in seconds. Set this to use another time than the lifeTime.</param>
    private IEnumerator CoroutineTimer(float time = LIFETIME)
    {
        // Waits for a specified amount of time.
        yield return new WaitForSeconds(time);

        // Disables the droplet gameobject.
        gameObject.SetActive(false);
    }

    public void Explosion(float explosionForce, Vector3 explosionPoint)
    {
        rb.AddExplosionForce(explosionForce, explosionPoint, 2f);
    }

}
