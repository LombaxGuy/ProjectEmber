using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{

    // private LineRenderer line;
    [SerializeField]
    private GameObject activeFlame;

    private Vector3 flamePos;
    private Vector3 touchPos;
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    private Vector3 direction;
    private Rigidbody flameRigidbody;
    private float forceStrength;
    private float defaultForce;

    private float maxShootMagnitude;
    private float minShootMagnitude;

    [SerializeField]
    private bool touchBallToShoot;

    //Til at få kameraet til ikke at bevæge sig når man skal skyde.
    private bool playerDragging;

    public bool PlayerDragging
    {
        get { return playerDragging; }
        set { playerDragging = value; }
    }

    //Diskuteres.. 
    private bool flameIsMoving;

    public bool FlameIsMoving
    {
        get { return flameIsMoving; }
        set { flameIsMoving = value; }
    }
    // Use this for initialization
    void Start()
    {
        defaultForce = 10;
        playerDragging = false;
        flameRigidbody = GetComponent<Rigidbody>();

        // line = GetComponent<LineRenderer>();
    }

    private void HandleInputBegan()
    {
        playerDragging = true;

        if (touchBallToShoot)
        {
            touchStartPos = activeFlame.transform.position;
        }
        else
        {
            touchStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, activeFlame.transform.position.z));
        }
    }

    private void UpdateTouchPosAndDirection()
    {
        touchEndPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, activeFlame.transform.position.z));
        direction = touchStartPos - touchEndPos;
    }

    private void HandleInputEnded()
    {
        playerDragging = false;

        if (direction.magnitude < 3 && direction.magnitude > 1)
        {
            // ---------------------------------------------------------
            forceStrength = defaultForce * direction.magnitude / 2;
            Debug.Log("Medium force: " + forceStrength);
            // ---------------------------------------------------------
        }
        else if (direction.magnitude > 3)
        {
            forceStrength = defaultForce;
            Debug.Log("Max force: " + forceStrength);
        }
        else
        {
            forceStrength = 0;
            Debug.Log("No force: " + forceStrength);
        }

        flameRigidbody.AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (touchBallToShoot)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == this.gameObject)
                        {
                            HandleInputBegan();
                        }
                    }
                }
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    HandleInputBegan();
                }
            }

            if (playerDragging == true)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    UpdateTouchPosAndDirection();
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    UpdateTouchPosAndDirection();

                    HandleInputEnded();
                }
            }
        }
    }
}
