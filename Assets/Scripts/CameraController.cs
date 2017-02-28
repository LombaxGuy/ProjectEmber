using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    #region Movement

    [SerializeField]
    private float moveSpeed = 0.01f;

    [SerializeField]
    private float moveLerpTime = 0.1f;

    [SerializeField]
    private CameraLockState cameraLockState = CameraLockState.FreeMove;

    private enum CameraLockState {Disabled, Follow, FreeMove};

    [SerializeField]
    private GameObject cameraLockTarget;

    private float margin = 2;

    private float xMaxSoftRaw = 2;
    private float xMinSoftRaw = -2;
    private float yMaxSoftRaw = 2;
    private float yMinSoftRaw = -2;

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

    private float moveZoomCapExtender = 1;
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

    private float currentZoomPercentage;
    #endregion

    // Use this for initialization
    void Start()
    {
        // Sets the soft cap values for the movement
        xMaxSoft = xMaxSoftRaw + moveZoomCapExtender * currentZoomPercentage;
        xMinSoft = xMinSoftRaw - moveZoomCapExtender * currentZoomPercentage;
        yMaxSoft = yMaxSoftRaw + moveZoomCapExtender * currentZoomPercentage;
        yMinSoft = yMinSoftRaw - moveZoomCapExtender * currentZoomPercentage;

        // Sets the hard cap values for the movement
        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;
        yMinHard = yMinSoft - margin;
        yMaxHard = yMaxSoft + margin;

        // Sets the hard cap values for the zoom
        zoomMinHard = zoomMinSoft - zoomMargin;
        zoomMaxHard = zoomMaxSoft + zoomMargin;

        // Calculate currentZoomPercentage
        currentZoomPercentage = 1 - (zoomMaxSoft - transform.position.z) / (zoomMaxSoft - zoomMinSoft);
    }

    // Update is called once per frame
    void Update()
    {
        // Updates the soft cap values for the movement
        xMaxSoft = xMaxSoftRaw + moveZoomCapExtender * currentZoomPercentage;
        xMinSoft = xMinSoftRaw - moveZoomCapExtender * currentZoomPercentage;
        yMaxSoft = yMaxSoftRaw + moveZoomCapExtender * currentZoomPercentage;
        yMinSoft = yMinSoftRaw - moveZoomCapExtender * currentZoomPercentage;

        // Updates the hard cap values for the movement
        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;
        yMinHard = yMinSoft - margin;
        yMaxHard = yMaxSoft + margin;

        // Handle move function
        HandleCameraMovement();
        // Handle zoom function
        HandleCameraZoom();
    }

    /// <summary>
    /// Used to move the camera based on input from a touch device.
    /// </summary>
    private void HandleCameraMovement()
    {
        // Checks which state the camera is in. Is it locked or not?
        switch (cameraLockState)
        {
            case CameraLockState.FreeMove:
                {
                    if (!ActionController.PlayerShooting)
                    {
                        // If a touch input is received and the touch phase is "Moved"
                        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            #region xDynamicSpeed
                            // If the camera is located within the x-bounds of the map...
                            if (transform.position.x > xMinSoft && transform.position.x < xMaxSoft)
                            {
                                //... the dynamic speed on the x-axis is set to 1. 
                                xDynamicSpeed = 1;
                            }
                            // If the camera is out of bounds on the left(-x) side of the map... 
                            else if (transform.position.x < xMinSoft)
                            {
                                //... calculate the dynamic speed on the x-axis.
                                CalculateDynamicSpeed(xMinSoft, xMinHard, ref xDynamicSpeed, 'x');
                            }
                            // If the camera is out of bounds on the right(+x) side of the map... 
                            else if (transform.position.x > xMaxSoft)
                            {
                                //... calculate the dynamic speed on the x-axis.
                                CalculateDynamicSpeed(xMaxSoft, xMaxHard, ref xDynamicSpeed, 'x');
                            }
                            #endregion

                            #region yDynamicSpeed
                            // Same as in xDynamicSpeed just on the y-axis.
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

                            // Saves the change in position in a Vector2
                            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                            // Moves the camera
                            transform.Translate(-touchDeltaPosition.x * moveSpeed * xDynamicSpeed, -touchDeltaPosition.y * moveSpeed * yDynamicSpeed, 0);

                            // Clamps the camera's position
                            cameraPosition.x = Mathf.Clamp(transform.position.x, xMinSoft - margin, xMaxSoft + margin);
                            cameraPosition.y = Mathf.Clamp(transform.position.y, yMinSoft - margin, yMaxSoft + margin);
                            cameraPosition.z = transform.position.z;

                            // Sets the position to the clamped values
                            transform.position = cameraPosition;
                        }

                        // If no touch input is received
                        if (Input.touchCount == 0)
                        {
                            #region X-Axis
                            // If the current x-position of the camera is larger than the x-position soft cap...
                            if (transform.position.x > xMaxSoft)
                            {
                                //... the camera's x-position is lerped to the value of the soft cap.
                                transform.position = new Vector3(Mathf.Lerp(transform.position.x, xMaxSoft, moveLerpTime), transform.position.y, transform.position.z);
                            }
                            // If the current x-position of the camera is smaller than the x-position soft cap...
                            else if (transform.position.x < xMinSoft)
                            {
                                //... the camera's x-position is lerped to the value of the soft cap.
                                transform.position = new Vector3(Mathf.Lerp(transform.position.x, xMinSoft, moveLerpTime), transform.position.y, transform.position.z);
                            }
                            #endregion

                            #region Y-Axis
                            // Same as the X-Axis but on the Y-Axis
                            if (transform.position.y > yMaxSoft)
                            {
                                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yMaxSoft, moveLerpTime), transform.position.z);
                            }
                            else if (transform.position.y < yMinSoft)
                            {
                                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yMinSoft, moveLerpTime), transform.position.z);
                            }
                            #endregion
                        }
                    }
                    
                }
                break;

            case CameraLockState.Follow:
                {
                    // Set the cameraPosition variable the x- and y-position of the cameraLockTarget.
                    cameraPosition.x = cameraLockTarget.transform.position.x;
                    cameraPosition.y = cameraLockTarget.transform.position.y;
                    cameraPosition.z = transform.position.z;

                    // Sets the position of the camera. Using a lerp to get a smooth movement.
                    transform.position = Vector3.Lerp(transform.position, cameraPosition, moveLerpTime);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Used to zoom in and out based on input form a touch device.
    /// </summary>
    private void HandleCameraZoom()
    {

        // If the number of touch inputs is 2
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Calculates the distance between the touch points from the last frame.
            float oldDistance = Vector2.Distance((touch0.position - touch0.deltaPosition), (touch1.position - touch1.deltaPosition));
            
            // Calculates the distance between the touch points in the current frame.
            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            // Calculates the diffence in distance between the two frames.
            float deltaDistance = oldDistance - currentDistance;

            // If the zoom level is between the soft caps...
            if (transform.position.z > zoomMinSoft && transform.position.z < zoomMaxSoft)
            {
                //... the dynamicZoomSpeed is set to 1.
                dynamicZoomSpeed = 1;
                currentZoomPercentage = 1 - (zoomMaxSoft - transform.position.z) / (zoomMaxSoft - zoomMinSoft);
            }
            // If the zoom level is smaller than the soft cap...
            else if (transform.position.z < zoomMinSoft)
            {
                //... the dynamicZoomSpeed is calculated.
                CalculateZoomSpeed(zoomMinSoft, zoomMinHard, ref dynamicZoomSpeed);
            }
            // If the zoom level is larger than the soft cap...
            else if (transform.position.z > zoomMaxSoft)
            {
                //... the dynamicZoomSpeed is calculated.
                CalculateZoomSpeed(zoomMaxSoft, zoomMaxHard, ref dynamicZoomSpeed);
            }

            if (deltaDistance < 0 && transform.position.z < zoomMaxHard || deltaDistance > 0 && transform.position.z > zoomMinHard)
            {
                // The camera is translated.
                transform.Translate(0, 0, -deltaDistance * zoomSpeed * dynamicZoomSpeed);

                // The-z component of the cameraPosition is clamped between the two hard caps.
                cameraPosition.z = Mathf.Clamp(transform.position.z, zoomMinHard, zoomMaxHard);

                // The z-position of the camera is set to the z-component of the cameraPosition vector.
                transform.position = new Vector3(transform.position.x, transform.position.y, cameraPosition.z);
            }
        }

        // If no touch input is received
        if (Input.touchCount == 0)
        {
            // If the zoom level is larger than the soft cap...
            if (transform.position.z > zoomMaxSoft)
            {
                //... the z-position of the camera is lerped to the value soft cap.
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, zoomMaxSoft, zoomLerpTime));
            }
            // If the zoom level is smaller than the soft cap...
            else if (transform.position.z < zoomMinSoft)
            {
                //... the z-position of the camera is lerped to the value soft cap.
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, zoomMinSoft, zoomLerpTime));
            }
        }
    }

    /// <summary>
    /// Used to calculate the dynamic movement speed of the camera.
    /// </summary>
    /// <param name="softCap">The soft cap used in the calculation.</param>
    /// <param name="hardCap">The hard cap used in the calculation.</param>
    /// <param name="dynamicSpeed">The speed variable that should be used in the calculation. !!NOTE!! This value is changed during the calculation.</param>
    /// <param name="axis">The axis the calculation should be made for. Only supports the x- and y-axis.</param>
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

    /// <summary>
    /// Used to calculate the dynamic zoom speed of the camera.
    /// </summary>
    /// <param name="softCap">The soft cap used in the calculation.</param>
    /// <param name="hardCap">The hard cap used in the calculation.</param>
    /// <param name="dynamicSpeed">The speed variable that should be used in the calculation. !!NOTE!! This value is changed during the calculation.</param>
    private void CalculateZoomSpeed(float softCap, float hardCap, ref float dynamicSpeed)
    {
        float percent = 1 - (softCap - transform.position.z) / (softCap - hardCap);
        dynamicSpeed = Mathf.Pow(percent, 2);
    }
}
