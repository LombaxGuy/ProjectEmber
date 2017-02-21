using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {

    private float zoomSpeed = 0.2f;

    // Use this for initialization
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Debug.Log("Blargh");
            PinchZoom();
        }

    }

    private void PinchZoom()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);
        

        Debug.Log("" + touch0.position + "" + touch1.position);

        float previousTouchDistance = Vector2.Distance((touch0.position - touch0.deltaPosition), (touch1.position - touch1.deltaPosition));
        float curTouchDistance = Vector2.Distance(touch0.position, touch1.position);

        float deltaDifference = previousTouchDistance - curTouchDistance;
        Debug.Log(deltaDifference);
        this.GetComponent<Camera>().fieldOfView += deltaDifference * zoomSpeed;

        this.GetComponent<Camera>().fieldOfView = Mathf.Clamp(this.GetComponent<Camera>().fieldOfView, 40f, 90f);

    }
}
