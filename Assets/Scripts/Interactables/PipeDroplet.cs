using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDroplet : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField]
    private const float LIFETIME = 2f;

    private Rigidbody dropletBody;

    private Coroutine fadeCoroutine;

    // Use this for initialization
    private void Start()
    {
        dropletBody = GetComponent<Rigidbody>();
        
        startPos = gameObject.transform.position;
    }

    /// <summary>
    /// Used to start the timer coroutine when the droplet is activated.
    /// </summary>
    private void OnEnable()
    {
        fadeCoroutine = StartCoroutine(CoroutineTimer());
    }

    /// <summary>
    /// Used to reset the droplet when it is disabled.
    /// </summary>
    private void OnDisable()
    {
        Reset();
    }

    /// <summary>
    /// If the droplet collides with any other objects in the scene the timer is stoped and the droplet is disabled.
    /// </summary>
    /// <param name="other">The gameobject the droplet collides with.</param>
    private void OnCollisionEnter(Collision other)
    {
        // If the fade coroutine is set
        if (fadeCoroutine != null)
        {
            // The coroutine is stopped and the variable is set to null.
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // Disables the droplet.
        gameObject.SetActive(false);
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

    /// <summary>
    /// Resets the droplets position and velocity.
    /// </summary>
    private void Reset()
    {
        // If the dropletBody is set...
        if (dropletBody)
        {
            //... reset position and velocity.
            gameObject.transform.position = startPos;
            dropletBody.velocity = Vector3.zero;
        }
    }
}
