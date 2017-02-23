using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    [SerializeField]
    private float maxZoomSpeed = 0.5f;
    [SerializeField]
    private float hardMinCap = -10;
    [SerializeField]
    private float hardMaxCap = -15;

    private float minZoomSpeed = 0.05f;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            PinchZoom();
        }

    }

    private void PinchZoom()
    {

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        //Touch distancen i den forrige frame
        float previousTouchDistance = Vector2.Distance((touch0.position - touch0.deltaPosition), (touch1.position - touch1.deltaPosition));
        //Touch distancen i den nuværende frame
        float curTouchDistance = Vector2.Distance(touch0.position, touch1.position);

        //Difference mellem de to udregnede distancer
        float deltaDifference = previousTouchDistance - curTouchDistance;
        Debug.Log(deltaDifference);

        float zoomSpeed = (maxZoomSpeed / 100) * (touch1.position.y - touch0.position.y);
        zoomSpeed = Mathf.Clamp(zoomSpeed, minZoomSpeed, maxZoomSpeed);

        if (deltaDifference > 0 && this.transform.position.z < -10 || deltaDifference < 0 && this.transform.position.z > -15)
        {
            this.transform.position += new Vector3(0, 0, deltaDifference * zoomSpeed);
        }



    }

}
