using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectionManager : MonoBehaviour
{
    private string wellName;
    private bool inWell = true;
    private int numberOfWells;

    [Tooltip("Button prefab for levels inside well")]
    [SerializeField]
    private GameObject buttonGameObject;

    private Vector3 swipeWellStartPos;
    private Vector3 swipeLevelStartPos;

    #region Added in QA
    private GameObject[] levelMenuArray;

    private UIVisibilityControl wellMenuUIControl;
    private UIVisibilityControl levelMenuUIControl;

    private Button[] wellButtons;
    private Button[][] levelButtons;

    private Button backButton;
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
        // Finds GameObjects by name.
        swipeWell = GameObject.Find("WellSwipeObject");
        swipeLevel = GameObject.Find("LevelSwipeObject");
        wellMenuUIControl = GameObject.Find("WellMenuObject").GetComponent<UIVisibilityControl>();
        levelMenuUIControl = GameObject.Find("LevelMenuObject").GetComponent<UIVisibilityControl>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        // Adds a listener to the back buttons OnClick event.
        backButton.onClick.AddListener(() => OnBack());

        // Sets the number of wells based on the child count of swipeWell.
        numberOfWells = swipeWell.transform.childCount;

        // Sets the horizontal borders for the swipe.
        xMinSoft = -1 * (numberOfWells - 1) * wellSpacing;

        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;

        // Initializes the arrays.
        levelMenuArray = new GameObject[numberOfWells];
        wellButtons = new Button[numberOfWells];
        levelButtons = new Button[numberOfWells][];

        // Populates the arrays.
        for (int i = 0; i < numberOfWells; i++)
        {
            // Adds the well buttons to the array.
            wellButtons[i] = swipeWell.transform.GetChild(i).gameObject.GetComponent<Button>();
            // Adds a listener to the buttons OnClick event.
            wellButtons[i].onClick.AddListener(() => OnWellSelected());

            // Adds the level menu objects to the levelMenuArray.
            levelMenuArray[i] = swipeLevel.transform.GetChild(i).gameObject;

            // Turns off the level menu UI.
            levelMenuArray[i].GetComponent<UIVisibilityControl>().HideUI();
        }

        // Populates the levelButtons array.
        for (int i = 0; i < levelMenuArray.Length; i++)
        {
            // Creates a temporary button array.
            Button[] temp = new Button[levelMenuArray[i].transform.childCount];
            levelButtons[i] = temp;

            // Adds the level buttons to the array.
            for (int j = 0; j < levelMenuArray[i].transform.childCount; j++)
            {
                levelButtons[i][j] = levelMenuArray[i].transform.GetChild(j).GetComponent<Button>();
                levelButtons[i][j].onClick.AddListener(() => OnLevelSelected());
            }
        }

        // Set the starting positions of the two moving objects
        swipeWellStartPos = swipeWell.transform.position;
        swipeLevelStartPos = swipeLevel.transform.position;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // If we are in the well menu.
        if (inWell)
        {
            HandleSwipeHorizontal();
        }
        // If we are in the level menu.
        else
        {
            HandleSwipeVertical();
        }
    }

    /// <summary>
    /// Handles horizontal swipe actions.
    /// </summary>
    private void HandleSwipeHorizontal()
    {
#if(DEBUG)
        float deltaPosX = 0;

        // If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            // If the centering coroutine exists it is stopped.
            if (centeringCoroutine != null)
            {
                StopCoroutine(centeringCoroutine);
            }

            // The position of the mouse is saved.
            oldMousePos = Input.mousePosition;
        }
        // If the left mouse button is held down and the old mouse position is not the the current mouseposition.
        else if (Input.GetMouseButton(0) && oldMousePos != Input.mousePosition)
        {
            // Calculate the difference in x-position.
            deltaPosX = Input.mousePosition.x - oldMousePos.x;

            // If the swipeWell GameObject is located within the x-bounds of the map...
            if (swipeWell.transform.localPosition.x > xMinSoft && swipeWell.transform.localPosition.x < xMaxSoft)
            {
                //... the dynamic speed on the x-axis is set to 1. 
                dynamicSpeed = 1;
            }
            // If the swipeWell GameObject is out of bounds on the left(-x) side of the map... 
            else if (swipeWell.transform.localPosition.x < xMinSoft)
            {
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMinSoft, xMinHard, swipeWell.transform.localPosition.x, ref dynamicSpeed);
            }
            // If the swipeWell GameObject is out of bounds on the right(+x) side of the map... 
            else if (swipeWell.transform.localPosition.x > xMaxSoft)
            {
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMaxSoft, xMaxHard, swipeWell.transform.localPosition.x, ref dynamicSpeed);
            }

            // Move the transform in local space
            swipeWell.transform.Translate(deltaPosX * horizontalMoveSpeed * dynamicSpeed, 0, 0, Space.Self);

            Vector3 pos;

            // Clamps the position to the hard cap values.
            pos.x = Mathf.Clamp(swipeWell.transform.localPosition.x, xMinHard, xMaxHard);
            pos.y = swipeWell.transform.localPosition.y;
            pos.z = swipeWell.transform.localPosition.z;

            swipeWell.transform.localPosition = pos;

            // Saves the current position of the mouse.
            oldMousePos = Input.mousePosition;
        }
        // If the left mouse button is released.
        else if (Input.GetMouseButtonUp(0))
        {
            // The current local position of the swipeWell GameObject is saved.
            Vector3 fromPos = swipeWell.transform.localPosition;
            // The nearest snap point is calculated based on the well spacing.
            Vector3 toPos = new Vector3(Mathf.Round(swipeWell.transform.localPosition.x / wellSpacing) * wellSpacing, swipeWell.transform.localPosition.y, swipeWell.transform.localPosition.z);

            // Starts the centering coroutine.
            centeringCoroutine = StartCoroutine(CoroutineSnapToPosition(fromPos, toPos, swipeWell, 0.25f));

            // Resets the dynamic speed to 1;
            dynamicSpeed = 1;
        }
#endif
    }

    /// <summary>
    /// Handles vertical swipe actions.
    /// </summary>
    private void HandleSwipeVertical()
    {
#if(DEBUG)
        // Almost identical to HandleSwipeHorizontal. See HandleSwipeHorizontal for comments.
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

            if (swipeLevel.transform.localPosition.y > yMinSoft && swipeLevel.transform.localPosition.y < yMaxSoft)
            {
                dynamicSpeed = 1;
            }
            else if (swipeLevel.transform.localPosition.y < yMinSoft)
            {
                CalculateDynamicSpeed(yMinSoft, yMinHard, swipeLevel.transform.localPosition.y, ref dynamicSpeed);
            }
            else if (swipeLevel.transform.localPosition.y > yMaxSoft)
            {
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
    /// <param name="currentAxisPos">The current position on one axis.</param>
    /// <param name="dynamicSpeed">The speed variable that should be used in the calculation. !!NOTE!! This value is changed during the calculation.</param>
    private void CalculateDynamicSpeed(float softCap, float hardCap, float currentAxisPos, ref float dynamicSpeed)
    {
        float percentage = 0;

        // Calculates the percentage of movement.
        percentage = 1 - (softCap - currentAxisPos) / (softCap - hardCap);
        dynamicSpeed = Mathf.Pow(percentage, 2);
    }

    /// <summary>
    /// Coroutine used to snap the objects back to a point.
    /// </summary>
    /// <param name="fromPos">The position the object is moving from.</param>
    /// <param name="toPos">The position the object should snap to.</param>
    /// <param name="objectToMove">The object that should be moved.</param>
    /// <param name="snapTime">The time it takes in seconds to complete the snap.</param>
    /// <returns></returns>
    private IEnumerator CoroutineSnapToPosition(Vector3 fromPos, Vector3 toPos, GameObject objectToMove, float snapTime)
    {
        // t value used in a lerp function.
        float t = 0;

        // While the t value is not yet 1.
        while (t < 1)
        {
            // Increase t with the time since last frame divided by the total time.
            t += Time.deltaTime / snapTime;

            // Updates the local position of the object using a lerp function.
            objectToMove.transform.localPosition = Vector3.Lerp(fromPos, toPos, t);

            yield return null;
        }
    }

    /// <summary>
    /// Method that runs when a well button is clicked.
    /// </summary>
    public void OnWellSelected()
    {
        int numberOfLevels = 0;

        // The name of the clicked well.
        wellName = EventSystem.current.currentSelectedGameObject.name;

        for (int i = 0; i < numberOfWells; i++)
        {
            // Finds the correct UIVisibilityControl and shows it.
            if (wellName == "Well_" + (i + 1).ToString())
            {
                numberOfLevels = levelMenuArray[i].transform.childCount;

                levelMenuArray[i].GetComponent<UIVisibilityControl>().ShowUI();
            }
        }

        // Sets the swipe border values.
        #region Vertical Swipe
        yMinSoft = -1 * (numberOfLevels - 1) * levelSpacing;

        yMinHard = yMinSoft - margin;
        yMaxHard = yMaxSoft + margin;
        #endregion

        // Hides the well buttons.
        wellMenuUIControl.HideUI();

        inWell = false;
    }

    /// <summary>
    /// Method that runs when a level button is clicked.
    /// </summary>
    public void OnLevelSelected()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        
        // Loads a level with the name.
        SceneManager.LoadScene(wellName + "_" + EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
    }

    /// <summary>
    /// Method that runs when the back button is clicked.
    /// </summary>
    public void OnBack()
    {
        // If the game is in the well menu the game should return to the main menu.
        if (inWell == true)
        {
            Debug.Log("Nothing to go back to at the moment.");
        }
        // If the game is in the level select the game should return to the well selection screen.
        else
        {
            levelMenuUIControl.HideUI();
            wellMenuUIControl.ShowUI();

            swipeLevel.transform.position = swipeLevelStartPos;

            inWell = true;
        }
    }
}