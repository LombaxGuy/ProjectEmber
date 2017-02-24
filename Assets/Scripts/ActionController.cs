using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {
    
    private LineRenderer line;
    private Vector3 flamePos;
    private Vector3 touchPos;
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    private Vector3 direction;
    private Vector3 clippingPlane = new Vector3(0, 0, 0.05f);
    private Rigidbody rb;
    private float forceStrength;
    private float initialForce;

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
    void Start ()
    {
        initialForce = 10;
        playerDragging = false;
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (touchBallToShoot == true)
        {
            if (Input.touchCount == 1)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            line.enabled = true;
                            playerDragging = true;
                            touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5));
                            direction = transform.position - touchPos;
                            touchPos = transform.position + direction;
                            line.SetPosition(0, transform.position);
                            line.SetPosition(1, touchPos);
                        }
                    }
                }

                if (playerDragging == true)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5));
                        direction = transform.position - touchPos;
                        touchPos = transform.position + direction;
                        line.SetPosition(0, transform.position);
                        line.SetPosition(1, touchPos);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        line.enabled = false;
                        playerDragging = false;
                        touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5));
                        direction = transform.position - touchPos;
                        Debug.Log("Magnitude : " + direction.magnitude);
                        if (direction.magnitude < 3 && direction.magnitude > 1)
                        {
                            forceStrength = initialForce * direction.magnitude / 2;
                        }
                        else if (direction.magnitude > 3)
                        {
                            forceStrength = initialForce;
                        }
                        else
                        {
                            forceStrength = 0;
                        }

                        rb.AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                    }
                }
            }
        }
        else
        {
            if (Input.touchCount == 1)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        line.enabled = true;
                        playerDragging = true;
                        touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5));
                        direction = transform.position - touchPos;
                        touchPos = transform.position + direction;
                        line.SetPosition(0, transform.position);
                        line.SetPosition(1, touchPos);
                        break;
                    case TouchPhase.Moved:
                        if (playerDragging == true)
                        {
                            touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5));
                            direction = transform.position - touchPos;
                            touchPos = transform.position + direction;
                            line.SetPosition(0, transform.position);
                            line.SetPosition(1, touchPos);
                        }

                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:
                        if (playerDragging == true)
                        {
                            line.enabled = false;
                            playerDragging = false;
                            touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5));
                            direction = transform.position - touchPos;
                            Debug.Log("Magnitude : " + direction.magnitude);
                            if (direction.magnitude < 3 && direction.magnitude > 1)
                            {
                                forceStrength = initialForce * direction.magnitude / 2;
                            }
                            else if (direction.magnitude > 3)
                            {
                                forceStrength = initialForce;
                            }
                            else
                            {
                                forceStrength = 0;
                            }

                            rb.AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
                        }
                        break;
                    case TouchPhase.Canceled:
                        break;
                    default:
                        break;
                }
            }
        }

		
	}
}
