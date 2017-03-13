using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    private LineRenderer line;

    // Temporarily a serialized field. Should be changed later. The flame object should be specified automatically.
    [SerializeField]
    private GameObject flameObject;

    [SerializeField]
    private int maxSegmentCount = 250;
    private int segmentCount = 250;

    [SerializeField]
    private float segmentScale = 0.1f;

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnLaunch;
        EventManager.OnProjectileUpdated += OnUpdate;
        EventManager.OnGameWorldReset += OnReset;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnLaunch;
        EventManager.OnProjectileUpdated -= OnUpdate;
        EventManager.OnGameWorldReset -= OnReset;
    }

    /// <summary>
    /// Used to remove the path on reset.
    /// </summary>
    private void OnReset()
    {
        segmentCount = 0;
    }

    /// <summary>
    /// Event handler for the OnLaunch event. Simulates the path of the projectile.
    /// </summary>
    /// <param name="direction">The direction the projectile was launched in.</param>
    /// <param name="forceStrenght">The force with which the projectile was launched.</param>
    private void OnLaunch(Vector3 direction, float forceStrenght)
    {
        SimulatePath(direction, forceStrenght);
    }

    /// <summary>
    /// Event handler for the OnLaunch event. Simulates the path of the projectile.
    /// </summary>
    /// <param name="direction">The direction the projectile was launched in.</param>
    /// <param name="forceStrenght">The force with which the projectile was launched.</param>
    private void OnUpdate(Vector3 direction, float forceStrenght)
    {
        SimulatePath(direction, forceStrenght);
    }

    private void Start()
    {
        // Gets the LineRenderer component.
        line = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Simulate the path of the launched projectile.
    /// </summary>
    /// <param name="direction">The direction the projectile was launched in.</param>
    /// <param name="forceStrenght">The force with which the projectile was launched.</param>
    private void SimulatePath(Vector3 direction, float forceStrenght)
    {
        // Creates an array
        Vector3[] segments = new Vector3[maxSegmentCount];

        // The first line point is set to the position of the launch point.
        segments[0] = flameObject.transform.position;

        // The initial velocity is calculated.
        Vector3 segmentVelocity = direction * forceStrenght;

        for (int i = 1; i < maxSegmentCount; i++)
        {
            // Time it takes to traverse one segment of length segmentScale (careful if velocity is zero)
            float segmentTime = (segmentVelocity.sqrMagnitude != 0) ? segmentScale / segmentVelocity.magnitude : 0;

            // Add velocity from gravity for this segment's timestep
            segmentVelocity = segmentVelocity + Physics.gravity * segmentTime;

            // Check to see if we're going to hit a physics object
            RaycastHit hit;
            if (Physics.Raycast(segments[i - 1], segmentVelocity, out hit, segmentScale, LayerMask.GetMask("Environment")))
            {
                // The number of segments is set.
                segmentCount = i;

                // The for-loop is broken.
                break;
            }
            // If our raycast hit no objects, then set the next position to the last one plus v*t
            else
            {
                segments[i] = segments[i - 1] + segmentVelocity * segmentTime;
            }
        }

        // At the end, apply our simulations to the LineRenderer
        line.numPositions = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            line.SetPosition(i, segments[i]);
        }
    }
}