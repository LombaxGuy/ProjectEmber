using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerRope : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            CoroutineLevelEnd();
        }
    }

    private IEnumerator CoroutineLevelEnd()
    {
        EventManager.InvokeOnLevelCompleted();

        //Do some fading, make the levelselect appear or do an animation
        yield return null;
    }
}
