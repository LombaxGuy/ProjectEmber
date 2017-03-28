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

    //private Vector2 touchStart;
    //private Vector2 touchStartUpdate;
    //private Vector2 touchEnd;
    //private Vector2 secTouch;
    //private Vector3 touchTemp;
    private bool inWell = true;
    private Vector3 swipeWellStartLocation;
    private Vector3 swipeLevelStartLocation;


    [Tooltip("Number of wells in the menu.")]
    [SerializeField]
    private int numberOfWells = 3;

    #region Added in QA
    private List<GameObject[]> levelMenusList = new List<GameObject[]>();

    private UIVisibilityControl wellMenuUIControl;
    private UIVisibilityControl levelMenuUIControl;
    #endregion

    #region Swipe
    private GameObject swipeWell;

    private GameObject swipeLevel;

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

    private float moveLerpTime = 0.1f;

    private float horizontalMoveSpeed = 0.5f;

    private float wellSpacing = 800;
    private float levelSpacing = 200;

    Coroutine centeringCoroutine;
    #endregion

    // Use this for initialization
    void Start()
    {
        #region Swipe
        swipeWell = GameObject.Find("WellSwipeObject");
        swipeLevel = GameObject.Find("LevelSwipeObject");

        xMinSoft = -1 * (numberOfWells - 1) * wellSpacing;

        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;
        #endregion

        wellMenuUIControl = GameObject.Find("WellMenuObject").GetComponent<UIVisibilityControl>();
        levelMenuUIControl = GameObject.Find("LevelMenuObject").GetComponent<UIVisibilityControl>();

        for (int i = 0; i < numberOfWells; i++)
        {
            levelMenusList.Add(new GameObject[0]);
        }

        swipeWellStartLocation = swipeWell.transform.position;
        swipeLevelStartLocation = swipeLevel.transform.position;
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
            HandleSwipeVertical();
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

            centeringCoroutine = StartCoroutine(CoroutineSnapToPosition(fromPos, toPos, swipeWell, 0.25f));

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
        }
        else if (Input.GetMouseButton(0) && oldMousePos != Input.mousePosition)
        {
            deltaPosY = Input.mousePosition.y - oldMousePos.y;

            // If the camera is located within the x-bounds of the map...
            if (swipeLevel.transform.localPosition.y > yMinSoft && swipeLevel.transform.localPosition.y < yMaxSoft)
            {
                //... the dynamic speed on the x-axis is set to 1. 
                dynamicSpeed = 1;
            }
            // If the camera is out of bounds on the left(-x) side of the map... 
            else if (swipeLevel.transform.localPosition.y < yMinSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(yMinSoft, yMinHard, swipeLevel.transform.localPosition.y, ref dynamicSpeed);
            }
            // If the camera is out of bounds on the right(+x) side of the map... 
            else if (swipeLevel.transform.localPosition.y > yMaxSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(yMaxSoft, yMaxHard, swipeLevel.transform.localPosition.y, ref dynamicSpeed);
            }

            swipeLevel.transform.Translate(0, deltaPosY * horizontalMoveSpeed * dynamicSpeed, 0, Space.Self);

            Vector3 pos;

            pos.x = swipeLevel.transform.localPosition.x;
            pos.y = Mathf.Clamp(swipeLevel.transform.localPosition.y, yMinHard, yMaxHard);
            pos.z = swipeLevel.transform.localPosition.z;

            swipeLevel.transform.localPosition = pos;

            oldMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 fromPos = swipeLevel.transform.localPosition;
            Vector3 toPos;

            if (swipeLevel.transform.localPosition.y > yMaxSoft)
            {
                toPos = new Vector3(swipeLevel.transform.localPosition.x, yMaxSoft, swipeLevel.transform.localPosition.z);
                centeringCoroutine = StartCoroutine(CoroutineSnapToPosition(fromPos, toPos, swipeLevel, 0.1f));
            }
            // If the current x-position of the camera is smaller than the x-position soft cap...
            else if (swipeLevel.transform.localPosition.y < yMinSoft)
            {
                toPos = new Vector3(swipeLevel.transform.localPosition.x, yMinSoft, swipeLevel.transform.localPosition.z);
                centeringCoroutine = StartCoroutine(CoroutineSnapToPosition(fromPos, toPos, swipeLevel, 0.1f));
            }

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

    private IEnumerator CoroutineSnapToPosition(Vector3 fromPos, Vector3 toPos, GameObject objectToMove, float snapTime)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / snapTime;

            objectToMove.transform.localPosition = Vector3.Lerp(fromPos, toPos, t);

            yield return null;
        }
    }

    //When there is a well button clicked, this method wil run. It will remove and then populate canvas with new buttons.
    public void OnWellSelected(int numberOfLevels)
    {
        #region Vertical Swipe
        yMinSoft = -1 * (numberOfLevels - 1) * levelSpacing;

        yMinHard = yMinSoft - margin;
        yMaxHard = yMaxSoft + margin;
        #endregion

        wellName = EventSystem.current.currentSelectedGameObject.name;

        switch (wellName)
        {
            case "1stWell":

                levelMenusList[0] = CreateLevels(levelMenusList[0], numberOfLevels);

                Debug.Log(levelMenusList[0].Length);
                break;

            case "2ndWell":

                levelMenusList[1] = CreateLevels(levelMenusList[1], numberOfLevels);


                Debug.Log(levelMenusList[1].Length);
                break;

            case "3rdWell":

                levelMenusList[2] = CreateLevels(levelMenusList[2], numberOfLevels);


                Debug.Log(levelMenusList[2].Length);
                break;

            default:
                break;
        }

        // Hides the well buttons.
        wellMenuUIControl.HideUI();

        swipeWell.transform.position = swipeWellStartLocation;
    }

    private GameObject[] CreateLevels(GameObject[] levelMenuElements, int numberOfLevels)
    {
        if (levelMenuElements.Length == 0)
        {
            levelMenuElements = new GameObject[numberOfLevels];

            for (int i = 1; i < numberOfLevels + 1; i++)
            {
                GameObject temp = Instantiate(buttonGameObject);
                temp.transform.SetParent(swipeLevel.transform);
                temp.name = "Button_" + i.ToString();
                temp.transform.localScale = new Vector3(1, 1, 1);

                if (i % 2 == 0)
                {
                    temp.transform.localPosition = new Vector3(levelSpacing, i * levelSpacing, 0);
                }
                else
                {
                    temp.transform.localPosition = new Vector3(-levelSpacing, i * levelSpacing, 0);
                }

                temp.GetComponentInChildren<Text>().text = i.ToString();

                levelMenuElements[i - 1] = temp;

                levelMenuUIControl.Reinitialize();
            }
        }

        inWell = false;

        return levelMenuElements;
    }

    //This changes to a level scene
    public void OnLevelSelected()
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
            levelMenuUIControl.HideUI();

            wellMenuUIControl.ShowUI();

            swipeLevel.transform.position = swipeLevelStartLocation;

            inWell = true;
        }
    }
}
