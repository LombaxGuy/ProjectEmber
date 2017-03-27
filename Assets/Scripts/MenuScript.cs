using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{

    [SerializeField]
    private AudioMixer[] mixer;

    private GameObject mainMenuObject;
    
    private GameObject shopMenuObject;

    private string currentlyActive = "";

    private bool sfxOff = false;
    private bool musicOff = false;
    private bool mainMenuHidden = false;
    private bool moveRight = false;
    private bool moveLeft = false;

    private int currentItemShown = 0;

    #region Swipe
    private GameObject swipeSkinObject;
    private GameObject swipePowerupObject;

    private float margin = 50;

    private float xMaxSoft = 0;
    private float xMinSoft;

    private float xMaxHard = 0;
    private float xMinHard;

    private float dynamicSpeed = 1;

    private Vector3 oldMousePos;
    private Camera menuCamera;
    private float moveLerpTime = 0.1f;
    private float horizontalMoveSpeed = 0.5f;
    private float spacing = 220;

    private int elements = 5;

    private Coroutine centeringCoroutine;
    #endregion

    public string CurrentlyActive
    {
        get { return currentlyActive; }
        set { currentlyActive = value; }
    }

    private void Start()
    {
        xMinSoft = -1 * (elements - 1) * spacing;

        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;

        swipeSkinObject = GameObject.Find("SkinObject");

        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            mainMenuObject = GameObject.Find("MainMenuObject");
        }
    }

    private void Update()
    {
        if (currentlyActive == "SkinObject")
        {
            HandleSwipeHorizontal(swipeSkinObject);
        }
    }

    private void HandleSwipeHorizontal(GameObject swipeObject)
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
            if (swipeObject.transform.localPosition.x > xMinSoft && swipeObject.transform.localPosition.x < xMaxSoft)
            {
                //... the dynamic speed on the x-axis is set to 1. 
                dynamicSpeed = 1;
            }
            // If the camera is out of bounds on the left(-x) side of the map... 
            else if (swipeObject.transform.localPosition.x < xMinSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMinSoft, xMinHard, swipeObject.transform.localPosition.x, ref dynamicSpeed);
            }
            // If the camera is out of bounds on the right(+x) side of the map... 
            else if (swipeObject.transform.localPosition.x > xMaxSoft)
            {
                //xDynamicSpeed = 0;
                //... calculate the dynamic speed on the x-axis.
                CalculateDynamicSpeed(xMaxSoft, xMaxHard, swipeObject.transform.localPosition.x, ref dynamicSpeed);
            }

            Debug.Log("MOVED: " + Input.mousePosition + " . " + oldMousePos + " . " + dynamicSpeed);

            swipeObject.transform.Translate(deltaPosX * horizontalMoveSpeed * dynamicSpeed, 0, 0, Space.Self);

            Vector3 pos;

            pos.x = Mathf.Clamp(swipeObject.transform.localPosition.x, xMinHard, xMaxHard);
            pos.y = swipeObject.transform.localPosition.y;
            pos.z = swipeObject.transform.localPosition.z;

            swipeObject.transform.localPosition = pos;

            oldMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 fromPos = swipeObject.transform.localPosition;
            Vector3 toPos = new Vector3(Mathf.Round(swipeObject.transform.localPosition.x / spacing) * spacing, swipeObject.transform.localPosition.y, swipeObject.transform.localPosition.z);

            centeringCoroutine = StartCoroutine(CoroutineSnapToPosition(fromPos, toPos, swipeObject, 0.25f));

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

    public void PauseMenuExitButton()
    {
        //Loads the first scene in the build which should be our menu
        SceneManager.LoadScene(0);

    }
    /// <summary>
    /// Mutes or unmutes the sound depending on it's current state 
    /// </summary>
    public void SFXOnOff()
    {
        if (sfxOff == true)
        {
            //Turn sound on by increasing volume to 0
            mixer[0].SetFloat("sfxVol", 0);
            sfxOff = false;
            PlayerPrefs.SetInt("sound", 0);
        }
        else
        {
            //Turn sound off by lowering the volume to minimum value
            mixer[0].SetFloat("sfxVol", -144);
            sfxOff = true;
            PlayerPrefs.SetInt("sound", -144);
        }

    }

    /// <summary>
    /// Method for testing
    /// </summary>
    public void FlameOnOff()
    {
        if (sfxOff == true)
        {
            //Turn sound on by increasing volume to 0
            sfxOff = false;
            mixer[2].SetFloat("flameVol", 0);
        }
        else
        {
            //Turn sound off by lowering the volume to minimum value
            mixer[2].SetFloat("flameVol", -144);
            sfxOff = true;
        }

    }

    /// <summary>
    /// Mutes or unmutes the music depending on it's current state 
    /// </summary>
    public void MusicOnOff()
    {
        if (musicOff == true)
        {
            //Turn music on by increasing volume to 0
            mixer[1].SetFloat("musicVol", 0);
            musicOff = false;
            PlayerPrefs.SetInt("music", 0);
        }
        else
        {
            //Turn music off by lowering the volume to minimum value
            mixer[1].SetFloat("musicVol", -144);
            musicOff = true;
            PlayerPrefs.SetInt("music", -144);
        }
    }


    public void CloseButton()
    {
        switch (CurrentlyActive)
        {
            case "ShopObject":
                ToogleMainMenuWindow("ShopObject", true, true);
                CurrentlyActive = "none";
                break;
            case "PowerupObject":
                ToogleMainMenuWindow("PowerupObject", true, false);
                ToogleMainMenuWindow("ShopObject", false, false);
                CurrentlyActive = "ShopObject";
                break;
            case "SkinObject":
                ToogleMainMenuWindow("SkinObject", true, false);
                ToogleMainMenuWindow("ShopObject", false, false);
                CurrentlyActive = "ShopObject";
                break;
            case "SettingsObject":
                ToogleMainMenuWindow("SettingsObject", true, true);
                CurrentlyActive = "none";
                break;
            case "CreditsObject":
                ToogleMainMenuWindow("CreditsObject", true, true);
                CurrentlyActive = "none";
                break;
            case "MapSelectObject":
                ToogleMainMenuWindow("MapSelectObject", true, true);
                CurrentlyActive = "none";
                break;
            case "pauseMenu":
                this.gameObject.SetActive(false);
                CurrentlyActive = "none";
                break;
            case "none":
                Debug.Log("No Menu Open");
                break;
            default:
                CurrentlyActive = "none";
                break;
        }
    }

    //private GameObject levelSelectCanvas;


    //public void ContinueButton()
    //{
    //    levelSelectCanvas.SetActive(true);
    //}

    public void ShopButton()
    {
        ToogleMainMenuWindow("ShopObject", false, false);
        ToogleMainMenuWindow("SkinObject", false, false);
        shopMenuObject = GameObject.Find("SkinObject");
        CurrentlyActive = "ShopObject";
    }

    public void PowerUpButton()
    {
        if (CurrentlyActive == "SkinObject")
        {
            ToogleMainMenuWindow("SkinObject", true, false);
        }
        ToogleMainMenuWindow("PowerupObject", false, false);
        shopMenuObject = GameObject.Find("PowerupObject");
        CurrentlyActive = "PowerupObject";
    }

    public void SkinButton()
    {
        ToogleMainMenuWindow("SkinObject", false, false);
        if (CurrentlyActive == "PowerupObject")
        {
            ToogleMainMenuWindow("PowerupObject", true, false);
        }
        shopMenuObject = GameObject.Find("SkinObject");
        CurrentlyActive = "SkinObject";
    }

    public void SettingsButton()
    {
        ToogleMainMenuWindow("SettingsObject", false, false);
        CurrentlyActive = "SettingsObject";
    }

    public void CreditsButton()
    {
        ToogleMainMenuWindow("CreditsObject", false, false);
        CurrentlyActive = "CreditsObject";
    }

    public void MapSelectButton()
    {
        ToogleMainMenuWindow("MapSelectObject", false, false);
        CurrentlyActive = "MapSelectObject";
    }

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }

    private void CurrentlySelected()
    {
        //This method should set the item in the middle as the currently displayed item
        //The method should also update the button and text below the item
    }

    private void ToogleMainMenuWindow(string name, bool closeWindow, bool showMainMenu)
    {
        Debug.Log("Opening/Closing " + name);

        GameObject tempObject = GameObject.Find(name);

        for (int i = 0; i < tempObject.transform.childCount; i++)
        {
            if (closeWindow == false)
            {
                Debug.Log(tempObject.transform.childCount);
                if (tempObject.transform.GetChild(i).GetComponent<Image>() == true)
                {
                    tempObject.transform.GetChild(i).GetComponent<Image>().enabled = true;
                }
                if (tempObject.transform.GetChild(i).GetComponent<Text>() == true)
                {
                    tempObject.transform.GetChild(i).GetComponent<Text>().enabled = true;
                }
                if (tempObject.transform.GetChild(i).transform.childCount > 0)
                {
                    tempObject.transform.GetChild(i).GetComponentInChildren<Text>().enabled = true;
                }
            }
            else
            {
                if (tempObject.transform.GetChild(i).GetComponent<Image>() == true)
                {
                    tempObject.transform.GetChild(i).GetComponent<Image>().enabled = false;
                }
                if (tempObject.transform.GetChild(i).GetComponent<Text>() == true)
                {
                    tempObject.transform.GetChild(i).GetComponent<Text>().enabled = false;
                }
                if (tempObject.transform.GetChild(i).transform.childCount > 0)
                {
                    tempObject.transform.GetChild(i).GetComponentInChildren<Text>().enabled = false;
                }
            }
        }

        ToogleMainMenu(showMainMenu);


    }

    private void ToogleMainMenu(bool state)
    {
        if (state == true)
        {
            for (int i = 0; i < mainMenuObject.transform.childCount; i++)
            {
                mainMenuObject.transform.GetChild(i).GetComponent<Image>().enabled = true;
                if (mainMenuObject.transform.GetChild(i).transform.childCount > 0)
                {
                    mainMenuObject.transform.GetChild(i).GetComponentInChildren<Text>().enabled = true;
                }
            }
            mainMenuHidden = false;
        }
        else
        {
            if (mainMenuHidden == false)
            {
                for (int i = 0; i < mainMenuObject.transform.childCount; i++)
                {
                    mainMenuObject.transform.GetChild(i).GetComponent<Image>().enabled = false;
                    if (mainMenuObject.transform.GetChild(i).transform.childCount > 0)
                    {
                        mainMenuObject.transform.GetChild(i).GetComponentInChildren<Text>().enabled = false;
                    }
                }
                mainMenuHidden = true;
            }
        }
    }

}
