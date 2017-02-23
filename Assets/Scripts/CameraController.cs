using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Camera move fields.

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 0.01f;

    [SerializeField]
    private float moveLerpTime = 0.1f;

    [SerializeField]
    private int cameraLockState = 1;

    [SerializeField]
    private GameObject cameraLockTarget;

    private float margin = 2;

    private float xMaxSoft = 2;
    private float xMinSoft = -2;
    private float yMaxSoft = 2;
    private float yMinSoft = -2;

    private float xMinHard;
    private float xMaxHard;
    private float yMinHard;
    private float yMaxHard;

    private float xDynamicSpeed = 1;
    private float yDynamicSpeed = 1;

    private Vector3 cameraPosition;
    #endregion

    [Space(10)]
    [Header("Zoom")]

    #region Zoom
    [SerializeField]
    private float zoomSpeed = 0.01f;

    [SerializeField]
    private float zoomLerpTime = 0.1f;

    private float zoomMargin = 1;

    private float zoomMinSoft = -1.5f;
    private float zoomMaxSoft = 2.5f;

    private float zoomMinHard;
    private float zoomMaxHard;

    private float dynamicZoomSpeed = 1;
    #endregion

    // Use this for initialization
    void Start()
    {
        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;
        yMinHard = yMinSoft - margin;
        yMaxHard = yMaxSoft + margin;

        zoomMinHard = zoomMinSoft - zoomMargin;
        zoomMaxHard = zoomMaxSoft + zoomMargin;

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraMovement();
        HandleCameraZoom();
    }

    private void HandleCameraMovement()
    {
        switch (cameraLockState)
        {
            case 1:
                {
                    if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        #region Border snask
                        if (transform.position.x > xMinSoft && transform.position.x < xMaxSoft)
                        {
                            xDynamicSpeed = 1;
                        }
                        else if (transform.position.x < xMinSoft)
                        {
                            CalculateDynamicSpeed(xMinSoft, xMinHard, ref xDynamicSpeed, 'x');
                        }
                        else if (transform.position.x > xMaxSoft)
                        {
                            CalculateDynamicSpeed(xMaxSoft, xMaxHard, ref xDynamicSpeed, 'x');
                        }

                        if (transform.position.y > yMinSoft && transform.position.y < yMaxSoft)
                        {
                            yDynamicSpeed = 1;
                        }
                        else if (transform.position.y < yMinSoft)
                        {
                            CalculateDynamicSpeed(yMinSoft, yMinHard, ref yDynamicSpeed, 'y');
                        }
                        else if (transform.position.y > yMaxSoft)
                        {
                            CalculateDynamicSpeed(yMaxSoft, yMaxHard, ref yDynamicSpeed, 'y');
                        }
                        #endregion
                        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                        transform.Translate(-touchDeltaPosition.x * moveSpeed * xDynamicSpeed, -touchDeltaPosition.y * moveSpeed * yDynamicSpeed, 0);

                        cameraPosition.x = Mathf.Clamp(transform.position.x, xMinSoft - margin, xMaxSoft + margin);
                        cameraPosition.y = Mathf.Clamp(transform.position.y, yMinSoft - margin, yMaxSoft + margin);
                        cameraPosition.z = transform.position.z;

                        transform.position = cameraPosition;
                    }

                    if (Input.touchCount == 0)
                    {
                        if (transform.position.x > xMaxSoft)
                        {
                            transform.position = new Vector3(Mathf.Lerp(transform.position.x, xMaxSoft, moveLerpTime), transform.position.y, transform.position.z);
                        }
                        if (transform.position.x < xMinSoft)
                        {
                            transform.position = new Vector3(Mathf.Lerp(transform.position.x, xMinSoft, moveLerpTime), transform.position.y, transform.position.z);
                        }
                        if (transform.position.y > yMaxSoft)
                        {
                            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yMaxSoft, moveLerpTime), transform.position.z);
                        }
                        if (transform.position.y < yMinSoft)
                        {
                            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yMinSoft, moveLerpTime), transform.position.z);
                        }
                    }
                }
                break;

            case 2:
                {
                    cameraPosition.x = cameraLockTarget.transform.position.x;
                    cameraPosition.y = cameraLockTarget.transform.position.y;
                    cameraPosition.z = transform.position.z;

                    transform.position = Vector3.Lerp(transform.position, cameraPosition, moveLerpTime);
                }
                break;

            default:
                break;
        }
    }

    private void HandleCameraZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            //Touch distancen i den forrige frame
            float oldDistance = Vector2.Distance((touch0.position - touch0.deltaPosition), (touch1.position - touch1.deltaPosition));
            //Touch distancen i den nuværende frame
            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            //Difference mellem de to udregnede distancer
            float deltaDistance = oldDistance - currentDistance;

            if (transform.position.z > zoomMinSoft && transform.position.z < zoomMaxSoft)
            {
                dynamicZoomSpeed = 1;
            }
            else if (transform.position.z < zoomMinSoft)
            {
                CalculateZoomSpeed(zoomMinSoft, zoomMinHard, ref dynamicZoomSpeed);
            }
            else if (transform.position.z > zoomMaxSoft)
            {
                CalculateZoomSpeed(zoomMaxSoft, zoomMaxHard, ref dynamicZoomSpeed);
            }

            if (deltaDistance < 0 && transform.position.z < zoomMaxHard || deltaDistance > 0 && transform.position.z > zoomMinHard)
            {
                transform.Translate(0, 0, -deltaDistance * zoomSpeed * dynamicZoomSpeed);

                cameraPosition.z = Mathf.Clamp(transform.position.z, zoomMinHard, zoomMaxHard);

                transform.position = new Vector3(transform.position.x, transform.position.y, cameraPosition.z);
            }
        }

        if (Input.touchCount == 0)
        {
            if (transform.position.z > zoomMaxSoft)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, zoomMaxSoft, zoomLerpTime));
            }
            else if (transform.position.z < zoomMinSoft)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, zoomMinSoft, zoomLerpTime));
            }
        }
    }

    private void CalculateDynamicSpeed(float softCap, float hardCap, ref float dynamicSpeed, char axis)
    {
        axis = axis.ToString().ToLower().ToCharArray()[0];

        float percent = 0;

        switch (axis)
        {
            case 'x':
                percent = 1 - (softCap - transform.position.x) / (softCap - hardCap);
                dynamicSpeed = Mathf.Pow(percent, 2);
                break;

            case 'y':
                percent = 1 - (softCap - transform.position.y) / (softCap - hardCap);
                dynamicSpeed = Mathf.Pow(percent, 2);

                break;

            default:
                Debug.Log("Could not find Axis: " + axis);
                break;
        }
    }

    private void CalculateZoomSpeed(float softCap, float hardCap, ref float dynamicSpeed)
    {
        float percent = 1 - (softCap - transform.position.z) / (softCap - hardCap);
        dynamicSpeed = Mathf.Pow(percent, 2);
    }
}
