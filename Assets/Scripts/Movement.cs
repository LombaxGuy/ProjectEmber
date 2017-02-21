using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float speed = 0.01f;

    [SerializeField]
    private int cam_State;

    [SerializeField]
    private GameObject target;

    private float xpos = 2;
    private float xneg = -2;
    private float ypos = 2;
    private float yneg = -2;

    private float margin = 2;


    
    Vector3 camPos;
    
    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        camPos = transform.position;
        //Touch touch = Input.GetTouch(0);

        switch (cam_State)
        {
            default:
            break;

            case 1:
                {
                    if (Input.touchCount > 0 && Input.touchCount != 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        
                        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                        transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);    
                                            
                        camPos.x = Mathf.Clamp(transform.position.x, xneg - margin, xpos + margin);
                        camPos.y = Mathf.Clamp(transform.position.y, yneg - margin, ypos + margin);
                        camPos.z = transform.position.z;

                        transform.position = camPos;
                        
                    }

                    if(transform.position.x > xpos)
                    {
                        transform.position = new Vector3(Mathf.Lerp(transform.position.x, xpos, 0.1f), transform.position.y, transform.position.z);
                    }
                    if (transform.position.x < xneg)
                    {
                        transform.position = new Vector3(Mathf.Lerp(transform.position.x, xneg, 0.1f), transform.position.y, transform.position.z);
                    }
                    if (transform.position.y > ypos)
                    {
                        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, ypos, 0.1f), transform.position.z);
                    }
                    if (transform.position.y < yneg)
                    {
                        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yneg, 0.1f), transform.position.z);
                    }

                }
                break;

            case 2:
                {
                    camPos.x = target.transform.position.x;
                    camPos.y = target.transform.position.y;
                    camPos.z = transform.position.z;
                    

                    transform.position = Vector3.Lerp(transform.position, camPos, 0.1f);
                }
                break;
        }

    }
}
