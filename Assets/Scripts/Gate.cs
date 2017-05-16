using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private HingeJoint[] hingeArray = new HingeJoint[2];

    private IEnumerator corutine;

    private bool closeGate = true;

    [SerializeField]
    private float closedWaitTimer = 3f;
    [SerializeField]
    private float openWaitTimer = 3f;

    [SerializeField]
    private bool closeAtStart = false;

    private bool isRunning = false;

    // Use this for initialization
    void Start()
    {
        hingeArray[0] = transform.GetChild(0).GetComponent<HingeJoint>();
        hingeArray[1] = transform.GetChild(1).GetComponent<HingeJoint>();
        corutine = GateTimer(closedWaitTimer, openWaitTimer);

        for (int i = 0; i < hingeArray.Length; i++)
        {
            JointLimits limits = hingeArray[i].limits;
            if (!closeAtStart)
            {
                limits.bounciness = 0;
                limits.bounceMinVelocity = 0;
                limits.min = 0;
                limits.max = -90;
                hingeArray[i].limits = limits;
                hingeArray[i].useLimits = true;
            }
            else
            {
                limits.bounciness = 0;
                limits.bounceMinVelocity = 0;
                limits.min = 0;
                hingeArray[i].limits = limits;
                hingeArray[i].useLimits = true;
            }
        }
        if (!closeAtStart)
        {
            StartCoroutine(corutine);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (closeAtStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OpenGate();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                for (int i = 0; i < hingeArray.Length; i++)
                {
                    hingeArray[i].useMotor = true;
                }
            }
        }
    }

    private IEnumerator GateTimer(float closedwaitTimer, float openWaitTimer)
    {
        bool running = false;
        Debug.Log("Blargh");
        Debug.Log(hingeArray.Length);
        while (true)
        {
            if (running)
            {
                Debug.Log("Bum");
                running = false;
                for (int i = 0; i < hingeArray.Length; i++)
                {
                    hingeArray[i].useMotor = false;
                }
                yield return new WaitForSeconds(closedWaitTimer);
            }
            else
            {
                Debug.Log("melum");
                running = true;
                for (int i = 0; i < hingeArray.Length; i++)
                {
                    hingeArray[i].useMotor = true;
                }
                float time = Time.time;
                yield return new WaitForSeconds(openWaitTimer);
                Debug.Log(Time.time - time);
            }

            //for (int i = 0; i < hingeArray.Length; i++)
            //{
            //    if (!isRunning)
            //    {
            //        hingeArray[i].useMotor = true;
            //        isRunning = true;
            //        Debug.Log("YDrk");
            //    }
            //    else
            //    {
            //        hingeArray[i].useMotor = false;
            //        isRunning = false;
            //        Debug.Log("ÆDrk");
            //    }
            //    Debug.Log(isRunning);

            //}
            //if (isRunning)
            //{
            //    Debug.Log("Hulu");
            //    yield return new WaitForSeconds(closedWaitTimer);
            //}
            //else
            //{
            //    Debug.Log("Bulu");
            //    yield return new WaitForSeconds(openWaitTimer);
            //}

        }

    }

    public void OpenGate()
    {
        for (int i = 0; i < hingeArray.Length; i++)
        {
            hingeArray[i].useLimits = false;
        }
    }
}
