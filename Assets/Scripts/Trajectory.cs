using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    

    // Reference to the LineRenderer we will use to display the simulated path
    private LineRenderer sightLine;

    [SerializeField]
    private GameObject flameObject;

    // Number of segments to calculate - more gives a smoother line
    public int segmentCount = 250;

    // Length scale for each segment
    public float segmentScale = 0.1f;

    // gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
    private Collider hitObject;
    public Collider HitObject { get { return hitObject; } }

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnLaunch;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnLaunch;
    }

    private void OnLaunch(Vector3 direction, float forceStrenght)
    {
        SimulatePath(direction, forceStrenght);
    }

    private void Start()
    {
        sightLine = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        //SimulatePath();
    }

    /// <summary>
    /// Simulate the path of a launched ball.
    /// Slight errors are inherent in the numerical method used.
    /// </summary>
    void SimulatePath(Vector3 direction, float forceStrenght)
    {
        Vector3[] segments = new Vector3[segmentCount];

        // The first line point is wherever the player's cannon, etc is
        segments[0] = flameObject.transform.position;

        // The initial velocity
        Vector3 segmentVelocity = direction * forceStrenght;

        // reset our hit object
        hitObject = null;

        for (int i = 1; i < segmentCount; i++)
        {
            // Time it takes to traverse one segment of length segScale (careful if velocity is zero)
            float segTime = (segmentVelocity.sqrMagnitude != 0) ? segmentScale / segmentVelocity.magnitude : 0;

            // Add velocity from gravity for this segment's timestep
            segmentVelocity = segmentVelocity + Physics.gravity * segTime;

            // Check to see if we're going to hit a physics object
            RaycastHit hit;
            if (Physics.Raycast(segments[i - 1], segmentVelocity, out hit, segmentScale))
            {
                // remember who we hit
                hitObject = hit.collider;

                // set next position to the position where we hit the physics object
                segments[i] = segments[i - 1] + segmentVelocity.normalized * hit.distance;
                // correct ending velocity, since we didn't actually travel an entire segment
                segmentVelocity = segmentVelocity - Physics.gravity * (segmentScale - hit.distance) / segmentVelocity.magnitude;
                // flip the velocity to simulate a bounce
                segmentVelocity = Vector3.Reflect(segmentVelocity, hit.normal);

                if (hit.transform.tag == "FlammableObject")
                {
                    break;
                }
                else if (hit.transform.tag == "KillerObject")
                {
                    break;
                }


                /*
				 * Here you could check if the object hit by the Raycast had some property - was 
				 * sticky, would cause the ball to explode, or was another ball in the air for 
				 * instance. You could then end the simulation by setting all further points to 
				 * this last point and then breaking this for loop.
				 */
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
        sightLine.SetColors(startColor, endColor);

        sightLine.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            sightLine.SetPosition(i, segments[i]);
    }
}

