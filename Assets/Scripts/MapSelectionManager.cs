using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectionManager : MonoBehaviour
{

    //TODO
    //Get the right position to the level button so it fits with the screen size
    //Fix swipe. Distance? something different?
    //Swipe for wells need to center one of the wells when touch is ended

    private static string wellName;
    [Tooltip("Button prefab for levels inside well")]
    [SerializeField]
    private GameObject buttonGameObject;
    [Tooltip("Well GameObject in canvas")]
    [SerializeField]
    private GameObject wellEmpty;
    [Tooltip("Levels GameObject in canvas")]
    [SerializeField]
    private GameObject levelsEmpty;

    private Vector2 touchStart;
    private Vector2 touchStartUpdate;
    private Vector2 touchEnd;
    private Vector2 secTouch;
    private Vector3 touchTemp;
    private bool inWell;
    private Vector3 wellEmptyStartLocation;
    private Vector3 levelsEmptyStartLocation;
    private int wellSelected;
    [Tooltip("Number of wells in the scene")]
    [SerializeField]
    private int maxWells = 3;

    

    #region Swipe
    [SerializeField]
    private GameObject swipeWell;

    private float margin = 200;

    private float xMaxSoft = 0;
    private float xMinSoft;
    private float yMaxSoft = 0;
    private float yMinSoft;

    private float xMinHard;
    private float xMaxHard;
    private float yMinHard;
    private float yMaxHard;

    private float dynamicSpeed = 1;

    private Vector3 oldMousePos;

    private Camera menuCamera;

    private float moveLerpTime = 0.1f;

    private float horizontalMoveSpeed = 0.5f;

    private float wellSpacing = 800;

    Coroutine centeringCoroutine;
    #endregion

    // Use this for initialization
    void Start()
    {
        #region Swipe
        xMinSoft = -1 * (maxWells - 1) * wellSpacing;

        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;

        menuCamera = Camera.main;
        #endregion

        wellEmptyStartLocation = wellEmpty.transform.position;
        levelsEmptyStartLocation = levelsEmpty.transform.position;
        inWell = true;
        wellSelected = 0;
        maxWells = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (inWell)
        {
            HandleSwipeHorizontal();
        }
        else
        {
            SwipeUpOrDown();
        }
    }

    private void HandleSwipeHorizontal()
    {
#if(DEBUG)
        float deltaPosX = 0;

        if (Input.GetMouseButtonDown(0))
        {
            if (centeringCoroutine != null)
            {
                StopCoroutine(centeringCoroutine);
            }
   
            oldMousePos = Input.mousePosition;
            Debug.Log("DOWN: " + oldMousePos);
        }
        else if (Input.GetMouseButton(0) && oldMousePos != Input.mousePosition)
        {
            deltaPosX = Input.mousePosition.x - oldMousePos.x;

            // If the camera is located within the x-bounds of the map...
            if (swipeWell.transform.localPosition.x > xMinSoft && swipeWell.transform.localPosition.x < xMaxSoft)
            {
                //... the dynamic speed on the x-axis is set to 1. 
                dynamicSpeed = 1;
            }
            // If the camera is out of bounds on the left(-x) side of the map... 
            else if (swipeWell.transform.localPosition.x < xMinSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMinSoft, xMinHard, swipeWell.transform.localPosition.x, ref dynamicSpeed);
            }
            // If the camera is out of bounds on the right(+x) side of the map... 
            else if (swipeWell.transform.localPosition.x > xMaxSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMaxSoft, xMaxHard, swipeWell.transform.localPosition.x, ref dynamicSpeed);
            }

            Debug.Log("MOVED: " + Input.mousePosition + " . " + oldMousePos + " . " + dynamicSpeed);

            swipeWell.transform.Translate(deltaPosX * horizontalMoveSpeed * dynamicSpeed, 0, 0, Space.Self);

            Vector3 pos;

            pos.x = Mathf.Clamp(swipeWell.transform.localPosition.x, xMinHard, xMaxHard);
            pos.y = swipeWell.transform.localPosition.y;
            pos.z = swipeWell.transform.localPosition.z;

            swipeWell.transform.localPosition = pos;

            oldMousePos = Input.mousePosition; 
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 fromPos = swipeWell.transform.localPosition;
            Vector3 toPos = new Vector3(Mathf.Round(swipeWell.transform.localPosition.x / wellSpacing) * wellSpacing, swipeWell.transform.localPosition.y, swipeWell.transform.localPosition.z);

            centeringCoroutine = StartCoroutine(CoroutineSnapToPosition(fromPos, toPos, 0.25f));

            dynamicSpeed = 1; 
        }
#endif
    }

    private void HandleSwipeVertical()
    {
#if(DEBUG)
        float deltaPosY = 0;

        if (Input.GetMouseButtonDown(0))
        {
            if (centeringCoroutine != null)
            {
                StopCoroutine(centeringCoroutine);
            }

            oldMousePos = Input.mousePosition;
            Debug.Log("DOWN: " + oldMousePos);
        }
        else if (Input.GetMouseButton(0) && oldMousePos != Input.mousePosition)
        {
            deltaPosY = Input.mousePosition.y - oldMousePos.y;

            // If the camera is located within the x-bounds of the map...
            if (swipeWell.transform.localPosition.y > xMinSoft && swipeWell.transform.localPosition.y < xMaxSoft)
            {
                //... the dynamic speed on the x-axis is set to 1. 
                dynamicSpeed = 1;
            }
            // If the camera is out of bounds on the left(-x) side of the map... 
            else if (swipeWell.transform.localPosition.y < xMinSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMinSoft, xMinHard, swipeWell.transform.localPosition.y, ref dynamicSpeed);
            }
            // If the camera is out of bounds on the right(+x) side of the map... 
            else if (swipeWell.transform.localPosition.y > xMaxSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMaxSoft, xMaxHard, swipeWell.transform.localPosition.y, ref dynamicSpeed);
            }

            Debug.Log("MOVED: " + Input.mousePosition + " . " + oldMousePos + " . " + dynamicSpeed);

            swipeWell.transform.Translate(0, deltaPosY * horizontalMoveSpeed * dynamicSpeed, 0, Space.Self);

            Vector3 pos;

            pos.x = swipeWell.transform.localPosition.x;
            pos.y = Mathf.Clamp(swipeWell.transform.localPosition.x, xMinHard, xMaxHard);
            pos.z = swipeWell.transform.localPosition.z;

            swipeWell.transform.localPosition = pos;

            oldMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 fromPos = swipeWell.transform.localPosition;
            Vector3 toPos = new Vector3(swipeWell.transform.localPosition.x, Mathf.Round(swipeWell.transform.localPosition.y / wellSpacing) * wellSpacing, swipeWell.transform.localPosition.z);

            centeringCoroutine = StartCoroutine(CoroutineSnapToPosition(fromPos, toPos, 0.25f));

            dynamicSpeed = 1;
        }
#endif
    }

    /// <summary>
    /// Used to calculate the dynamic movement speed of the UI elements.
    /// </summary>
    /// <param name="softCap">The soft cap used in the calculation.</param>
    /// <param name="hardCap">The hard cap used in the calculation.</param>
    /// <param name="dynamicSpeed">The speed variable that should be used in the calculation. !!NOTE!! This value is changed during the calculation.</param>
    private void CalculateDynamicSpeed(float softCap, float hardCap, float currentAxisPos, ref float dynamicSpeed)
    {
        float percent = 0;

        percent = 1 - (softCap - currentAxisPos) / (softCap - hardCap);
        dynamicSpeed = Mathf.Pow(percent, 2);
    }

    private IEnumerator CoroutineSnapToPosition(Vector3 fromPos, Vector3 toPos, float snapTime)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / snapTime;

            swipeWell.transform.localPosition = Vector3.Lerp(fromPos, toPos, t);

            yield return null;
        }
    }


    //Swipe left or right for picking wells
    private void SwipeLeftOrRight()
    {
        if (Input.touchCount == 1)
        {

            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    touchStart = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Moved:
                    secTouch = Input.GetTouch(0).position;
                    touchTemp = touchStart - secTouch;
                    touchTemp.Normalize();
                    wellEmpty.transform.localPosition = new Vector3(wellEmpty.transform.localPosition.x - (touchTemp.x * 50), wellEmpty.transform.localPosition.y, wellEmpty.transform.localPosition.z);
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    if (touchTemp.x >= 0)
                    {
                        if (wellSelected < maxWells - 1)
                        {
                            wellSelected++;
                        }
                    }
                    else
                    {
                        if (wellSelected > 0)
                        {
                            wellSelected--;
                        }
                    }
                    wellEmpty.transform.localPosition = new Vector3(wellEmptyStartLocation.x - (((Screen.width / 2) + (Screen.width / 4)) * wellSelected), wellEmpty.transform.localPosition.y, wellEmpty.transform.localPosition.z);

                    Debug.Log("WellEmpty position : " + wellEmpty.transform.position);
                    Debug.Log("wellSelected : " + wellSelected);

                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }

    //Swipe up or down for picking levels inside wells
    private void SwipeUpOrDown()
    {
        if (Input.touchCount == 1)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    touchStart = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Moved:
                    secTouch = Input.GetTouch(0).position;
                    touchTemp = touchStart - secTouch;
                    float distance = Vector2.Distance(touchStart, secTouch);
                    if (distance >= 3)
                    {
                        distance = 3;
                    }
                    touchTemp.Normalize();
                    levelsEmpty.transform.Translate(0, -touchTemp.y * distance, 0);
                    touchStart = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }

    //When there is a well button clicked, this method wil run. It will remove and then populate canvas with new buttons.
    public void PickWell(int numberOfLevels)
    {
        wellName = EventSystem.current.currentSelectedGameObject.name;
        wellEmpty.SetActive(false);
        levelsEmpty.SetActive(true);
        inWell = false;
        for (int i = 1; i <= numberOfLevels; i++)
        {
            GameObject temp = Instantiate(buttonGameObject);
            temp.transform.SetParent(levelsEmpty.transform);
            temp.name = "Button_" + i.ToString();
            temp.transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("Screen res : " + Screen.currentResolution);

            if (i % 2 == 0)
            {
                temp.transform.localPosition = new Vector3(Screen.width / 6, -Screen.height / 2 + (i * (Screen.height / 8)), 0);
            }
            else
            {
                temp.transform.localPosition = new Vector3(-(Screen.width / 6), -Screen.height / 2 + (i * (Screen.height / 8)), 0);
            }
            Debug.Log("Screen res : " + Screen.width + "x" + Screen.height);
            temp.GetComponentInChildren<Text>().text = i.ToString();
        }

        wellEmpty.transform.position = wellEmptyStartLocation;
    }

    //This changes to a level scene
    public void PickScene()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        SceneManager.LoadScene(wellName + "_" + EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
    }

    //Simple back button method. 
    public void BackFromSceneSelection()
    {
        if (inWell == true)
        {

        }
        else
        {
            foreach (Transform child in levelsEmpty.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            levelsEmpty.SetActive(false);
            wellEmpty.SetActive(true);
            levelsEmpty.transform.position = levelsEmptyStartLocation;
            inWell = true;
        }

    }
}
