using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    private HingeJoint hinge;
    private JointLimits limits;

    private IEnumerator corutine;

    private bool closeGate = true;

    [SerializeField]
    private float closedWaitTimer = 3f;
    [SerializeField]
    private float openWaitTimer = 3f;

    [SerializeField]
    private bool closeAtStart = false;

    private bool isClosing = false;

    // Use this for initialization
    void Start()
    {
        corutine = GateTimer(closedWaitTimer, openWaitTimer);

        hinge = GetComponent<HingeJoint>();
        limits = hinge.limits;
        if (!closeAtStart)
        {
        limits.bounciness = 0;
        limits.bounceMinVelocity = 0;
        limits.min = 0;
        limits.max = -90;
        hinge.limits = limits;
        hinge.useLimits = true;
            StartCoroutine(corutine);
        }
        else
        {
            limits.bounciness = 0;
            limits.bounceMinVelocity = 0;
            limits.min = 0;
            hinge.limits = limits;
            hinge.useLimits = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
       if(closeAtStart)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OpenGate();
            }
        }
    }

    private IEnumerator GateTimer(float closedwaitTimer, float openWaitTimer)
    {
        Debug.Log("Blargh");
        while (true)
        {
            if (!hinge.useMotor)
            {
                hinge.useMotor = true;
                isClosing = true;
            }
            else
            {
                hinge.useMotor = false;
                isClosing = false;
            }
            if (isClosing)
            {
                yield return new WaitForSeconds(closedWaitTimer);
            }
            else
            {
                yield return new WaitForSeconds(openWaitTimer);
            }

        }

    }

    public void OpenGate()
    {
        hinge.useLimits = false;
    }
}
