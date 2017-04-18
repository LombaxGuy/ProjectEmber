using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDroplet : MonoBehaviour
{
    //private bool alive;
    private Vector3 startPos;
    private float lifeTime = 2;

    // Use this for initialization
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    /// <summary>
    /// This resets the gameobject whevener it hits another gamepbject
    /// </summary>
    /// <param name="other">any gameobject</param>
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = startPos;
    }

    /// <summary>
    /// This coroutine makes the object dissapear after a set time.
    /// This gets called in the script that spawns this object o´r handles placements
    /// </summary>
    /// <returns>null</returns>
    private IEnumerator Fade()
    {
        float t = 0;

        while (gameObject.activeInHierarchy)
        {
            t += Time.deltaTime / 2;

            if (t > lifeTime)
            {
                gameObject.SetActive(false);
                gameObject.transform.position = startPos;
            }

            yield return null;
        }
    }

    /// <summary>
    /// This coroutine makes the object dissapear after a set time.
    /// This gets called in the script that spawns this object or handles placements, can take a custom time
    /// </summary>
    /// <param name="lifeTime">The time it takes for the droplet to get disabled</param>
    /// <returns>null</returns>
    private IEnumerator Fade(float lifeTime)
    {
        float t = 0;

        while (gameObject.activeInHierarchy)
        {
            t += Time.deltaTime / 2;

            if (t > lifeTime)
            {
                gameObject.SetActive(false);
                gameObject.transform.position = startPos;
            }
            yield return null;
        }

    }
}
