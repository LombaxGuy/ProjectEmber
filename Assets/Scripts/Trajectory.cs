using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    private LineRenderer line;

    [SerializeField]
    private GameObject flameObject;
    private Transform rotator;

    [SerializeField]
    private int segmentCount = 250;

    [SerializeField]
    private float segmentScale = 0.1f;

    private Collider hitCollider;
    public Collider HitCollider { get { return hitCollider; } }

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnLaunch;
        EventManager.OnProjectileUpdated += OnUpdate;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnLaunch;
        EventManager.OnProjectileUpdated -= OnUpdate;
    }

    private void OnLaunch(Vector3 direction, float forceStrenght)
    {
        SimulatePath(direction, forceStrenght);
    }

    private void OnUpdate(Vector3 direction, float forceStrenght)
    {
        SimulatePath(direction, forceStrenght);
    }

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        rotator = flameObject.transform.Find("Rotater");
    }

    /// <summary>
    /// Simulate the path of a launched ball.
    /// Slight errors are inherent in the numerical method used.
    /// </summary>
    void SimulatePath(Vector3 direction, float forceStrenght)
    {
        Vector3[] segments = new Vector3[segmentCount];

        Vector3[] segments1 = new Vector3[segmentCount];
        Vector3[] segments2 = new Vector3[segmentCount];

        // The first line point is wherever the player's cannon, etc is
        segments[0] = flameObject.transform.position;

        // The initial velocity
        Vector3 segmentVelocity = direction * forceStrenght;

        // reset our hit object
        hitCollider = null;

        bool breakLoop = false;

        for (int i = 1; i < segmentCount; i++)
        {
            // Time it takes to traverse one segment of length segScale (careful if velocity is zero)
            float segTime = (segmentVelocity.sqrMagnitude != 0) ? segmentScale / segmentVelocity.magnitude : 0;

            // Add velocity from gravity for this segment's timestep
            segmentVelocity = segmentVelocity + Physics.gravity * segTime;

            //Vector3 dir;

            //for (int j = 0; j < 8; j++)
            //{
            //    dir = new Vector3(Mathf.Cos((2 * Mathf.PI) / 8 * j), Mathf.Sin((2 * Mathf.PI) / 8 * j)).normalized;

            //    Debug.DrawRay(flameObject.transform.position, dir * 0.5f * 0.5f); // 0.5 from radius and 0.5 from scale factor

            //    RaycastHit hit;
            //    if (Physics.Raycast(segments[i - 1], segmentVelocity, out hit, 0.5f * 0.5f)) // 0.5 from radius and 0.5 from scale factor
            //    {
            //        hitCollider = hit.collider;

            //        float bounceFactor = 1;
            //        if (hitCollider.sharedMaterial != null)
            //        {
            //            hitCollider = hit.collider;

            //            bounceFactor = hitCollider.sharedMaterial.bounciness;
            //        }

            //        // flip the velocity to simulate a bounce
            //        segmentVelocity = Vector3.Reflect(segmentVelocity * bounceFactor, hit.normal);

            //        if (hit.transform.tag == "FlammableObject")
            //        {
            //            breakLoop = true;
            //            break;
            //        }
            //        else if (hit.transform.tag == "KillerObject")
            //        {
            //            breakLoop = true;
            //            break;
            //        }
            //    }
            //    else
            //    {
            //        segments[i] = segments[i - 1] + segmentVelocity * segTime;
            //    }

            //    if (breakLoop)
            //    {
            //        for (int k = i; k < segmentCount; k++)
            //        {
            //            segments[k] = segments[i];
            //        }

            //        break;
            //    }
            //}

            // Check to see if we're going to hit a physics object
            RaycastHit hit;
            if (Physics.Raycast(segments[i - 1], segmentVelocity, out hit, segmentScale))
            {
                // remember who we hit
                hitCollider = hit.collider;

                // set next position to the position where we hit the physics object
                segments[i] = segments[i - 1] + segmentVelocity.normalized * hit.distance;
                // correct ending velocity, since we didn't actually travel an entire segment
                segmentVelocity = segmentVelocity - Physics.gravity * (segmentScale - hit.distance) / segmentVelocity.magnitude;

                float bounceFactor = 1;
                if (hitCollider.sharedMaterial != null)
                {
                    bounceFactor = hitCollider.sharedMaterial.bounciness;
                }

                // flip the velocity to simulate a bounce
                segmentVelocity = Vector3.Reflect(segmentVelocity * bounceFactor, hit.normal);

                if (hit.transform.tag == "FlammableObject")
                {
                    break;
                }
                else if (hit.transform.tag == "KillerObject")
                {
                    break;
                }

                // Here you could check if the object hit by the Raycast had some property -was
                // sticky, would cause the ball to explode, or was another ball in the air for
                // instance.You could then end the simulation by setting all further points to
                // this last point and then breaking this for loop.
            }
            // If our raycast hit no objects, then set the next position to the last one plus v*t
            else
            {
                segments[i] = segments[i - 1] + segmentVelocity * segTime;
            }
        }

        // At the end, apply our simulations to the LineRenderer

        // Set the colour of our path to the colour of the next ball
        Color startColor = Color.red;
        Color endColor = startColor;
        startColor.a = 1;
        endColor.a = 0;

        line.startColor = startColor;
        line.endColor = endColor;

        line.numPositions = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            line.SetPosition(i, segments[i]);
        }
    }
}

