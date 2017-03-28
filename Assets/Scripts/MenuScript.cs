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

    private ShopItem[] skinItemArray;
    private ShopItem[] powerupItemArray;
    private Text priceText;
    private Button buyButton;
    private Text buyButtonText;
    private int rngNumber;



    private GameObject mainMenuObject;
    private GameObject shopMenuObject;

    private string currentlyActive = "";

    private int snappedPosInt = 0;
    private int currentItemShown = 0;
    private int currentSkinEquipped = 0;

    private bool sfxOff = false;
    private bool musicOff = false;
    private bool mainMenuHidden = false;
    private bool moveRight = false;
    private bool moveLeft = false;
    private bool updateObjectPosition = false;
    private bool snapToPosRunning = false;


    private Vector3 oldPos = new Vector3(0, 0, 0);

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

    private int skinElements;
    private int powerupElements;

    private Coroutine centeringCoroutine;
    #endregion


    public string CurrentlyActive
    {
        get { return currentlyActive; }
        set { currentlyActive = value; }
    }

    private void Start()
    {
        //Stuff that only needs to be run in the main menu
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            priceText = GameObject.Find("Pricetag").GetComponent<Text>();
            buyButton = GameObject.Find("BuyEquipButton").GetComponent<Button>();
            buyButtonText = GameObject.Find("BuyEquipText").GetComponent<Text>();
            swipeSkinObject = GameObject.Find("SkinObject");
            swipePowerupObject = GameObject.Find("PowerupObject");

            skinElements = swipeSkinObject.transform.childCount;
            powerupElements = swipePowerupObject.transform.childCount;

            skinItemArray = new ShopItem[skinElements];
            powerupItemArray = new ShopItem[powerupElements];

            CalculateCaps(skinElements);

            //Sets up an array containing all of the shopitems. Here their position is set which is used later for determining which skin is in the center
            //Here the price for the skins is also set tempoarily. System will be updated later on
            //The normal skin will currently be equipped here, will be changed to the game equipping whatever is saved
            for (int i = 0; i < skinElements; i++)
            {
                skinItemArray[i] = new ShopItem();
                if (i == 0)
                {
                    skinItemArray[i].ItemPosition = 395;
                    skinItemArray[i].Unlocked = true;
                    skinItemArray[i].Equipped = true;
                }
                else
                {
                    skinItemArray[i].ItemPosition = skinItemArray[i - 1].ItemPosition - 220;
                }
                Debug.Log(i + " " + skinItemArray[i].ItemPosition);
                //Currently rnging the price for each object
                rngNumber = Random.Range(1, 5);
                switch (rngNumber)
                {
                    case 1:
                        skinItemArray[i].Price = "1.50€";
                        break;
                    case 2:
                        skinItemArray[i].Price = "5€";
                        break;
                    case 3:
                        skinItemArray[i].Price = "7.50€";
                        break;
                    case 4:
                        skinItemArray[i].Price = "9.99€";
                        break;
                }

            }
            //Setting up powerups much like the skins is set up
            for (int i = 0; i < powerupElements; i++)
            {
                powerupItemArray[i] = new ShopItem();
                if (i == 0)
                {
                    powerupItemArray[i].ItemPosition = 395;
                }
                else
                {
                    powerupItemArray[i].ItemPosition = powerupItemArray[i - 1].ItemPosition - 220;
                }
                Debug.Log(i + " " + powerupItemArray[i].ItemPosition);
                rngNumber = Random.Range(1, 5);
                switch (rngNumber)
                {
                    case 1:
                        powerupItemArray[i].Price = "1.50€";
                        break;
                    case 2:
                        powerupItemArray[i].Price = "5€";
                        break;
                    case 3:
                        powerupItemArray[i].Price = "7.50€";
                        break;
                    case 4:
                        powerupItemArray[i].Price = "9.99€";
                        break;
                }

            }

            mainMenuObject = GameObject.Find("MainMenuObject");
        }
    }

    private void Update()
    {
        if (currentlyActive == "SkinObject")
        {
            HandleSwipeHorizontal(swipeSkinObject);
            CalculateCaps(skinElements);
        }
        else if (currentlyActive == "PowerupObject")
        {
            HandleSwipeHorizontal(swipePowerupObject);
            CalculateCaps(powerupElements);
        }
        if (updateObjectPosition == true)
        {
            if (snapToPosRunning == false)
            {
                UpdateCurrentlySelected();
                updateObjectPosition = false;
            }
        }
    }

    /// <summary>
    /// Handles all swiping
    /// </summary>
    /// <param name="swipeObject"></param>
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

            updateObjectPosition = true;
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

    /// <summary>
    /// Coroutine which centers the object closest to the middle of the screen
    /// </summary>
    /// <param name="fromPos"></param>
    /// <param name="toPos"></param>
    /// <param name="objectToMove"></param>
    /// <param name="snapTime"></param>
    /// <returns></returns>
    private IEnumerator CoroutineSnapToPosition(Vector3 fromPos, Vector3 toPos, GameObject objectToMove, float snapTime)
    {
        snapToPosRunning = true;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / snapTime;

            objectToMove.transform.localPosition = Vector3.Lerp(fromPos, toPos, t);

            yield return null;
        }

        snapToPosRunning = false;
    }

    /// <summary>
    /// Calculates the soft and hard caps 
    /// </summary>
    /// <param name="elements">number of items in the object being swiped</param>
    private void CalculateCaps(int elements)
    {
        xMinSoft = -1 * (elements - 1) * spacing;

        xMinHard = xMinSoft - margin;
        xMaxHard = xMaxSoft + margin;
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

    /// <summary>
    /// A method for all close buttons in our menus
    /// </summary>
    public void CloseButton()
    {
        switch (CurrentlyActive)
        {
            case "PowerupObject":
                ToogleMainMenuWindow("PowerupObject", true, false);
                ToogleMainMenuWindow("ShopObject", true, true);
                CurrentlyActive = "none";
                break;
            case "SkinObject":
                ToogleMainMenuWindow("SkinObject", true, false);
                ToogleMainMenuWindow("ShopObject", true, true);
                CurrentlyActive = "none";
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

    /// <summary>
    /// Opens the shop
    /// </summary>
    public void ShopButton()
    {
        ToogleMainMenuWindow("ShopObject", false, false);
        ToogleMainMenuWindow("SkinObject", false, false);
        shopMenuObject = GameObject.Find("SkinObject");
        CurrentlyActive = "SkinObject";
        UpdateCurrentlySelected();
    }
    /// <summary>
    /// Switches shop view to powerups
    /// </summary>
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
    /// <summary>
    /// Switches shop view to skins
    /// </summary>
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
    /// <summary>
    /// Opens settingsmenu
    /// </summary>
    public void SettingsButton()
    {
        ToogleMainMenuWindow("SettingsObject", false, false);
        CurrentlyActive = "SettingsObject";
    }
    /// <summary>
    /// Opens creditsmenu
    /// </summary>
    public void CreditsButton()
    {
        ToogleMainMenuWindow("CreditsObject", false, false);
        CurrentlyActive = "CreditsObject";
    }
    /// <summary>
    /// Opens mapselect
    /// </summary>
    public void MapSelectButton()
    {
        ToogleMainMenuWindow("MapSelectObject", false, false);
        CurrentlyActive = "MapSelectObject";
    }
    /// <summary>
    /// The OnClick method for buying/equipping. Handles both skins and powerups
    /// </summary>
    public void BuyEquipButton()
    {
        if (currentlyActive == "SkinObject")
        {
            if (skinItemArray[currentItemShown].Unlocked == true)
            {
                //Unequips the currently equipped item and equips the item currently in center
                skinItemArray[currentSkinEquipped].Equipped = false;
                skinItemArray[currentItemShown].Equipped = true;
                currentSkinEquipped = currentItemShown;
            }
            else
            {
                //Unlocks an item
                skinItemArray[currentItemShown].Unlocked = true;
                priceText.text = "";
            }
        }
        else if (currentlyActive == "PowerupObject")
        {
            
        }
        UpdateButton();
    }

    /// <summary>
    /// Temp method for starting a lvl
    /// </summary>
    public void Level1()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Method determining which image is selected by looking at which item is currently in the center of the screen. Also updates the price text. Is run whenever the user swipes
    /// </summary>
    private void UpdateCurrentlySelected()
    {
        //Converts the currently selected swipeobjects position to an int and then determines which of the current objects is in center
        if (currentlyActive == "SkinObject")
        {
            snappedPosInt = Mathf.RoundToInt(swipeSkinObject.transform.position.x);

            for (int i = 0; i < skinItemArray.Length; i++)
            {
                if (snappedPosInt > skinItemArray[i].ItemPosition && snappedPosInt < skinItemArray[i].ItemPosition + 5 || snappedPosInt < skinItemArray[i].ItemPosition && snappedPosInt > skinItemArray[i].ItemPosition - 5)
                {
                    currentItemShown = i;
                    if (!skinItemArray[currentItemShown].Unlocked)
                    {
                        priceText.text = skinItemArray[i].Price;
                    }
                    else
                    {
                        priceText.text = "";
                    }
                }
            }
        }
        else if (currentlyActive == "PowerupObject")
        {
            snappedPosInt = Mathf.RoundToInt(swipePowerupObject.transform.position.x);

            for (int i = 0; i < powerupItemArray.Length; i++)
            {
                Debug.Log(powerupItemArray[i].ItemPosition);
                if (snappedPosInt > powerupItemArray[i].ItemPosition && snappedPosInt < powerupItemArray[i].ItemPosition + 5 || snappedPosInt < powerupItemArray[i].ItemPosition && snappedPosInt > powerupItemArray[i].ItemPosition - 5)
                {
                    currentItemShown = i;
                    if (!powerupItemArray[currentItemShown].Unlocked)
                    {
                        priceText.text = powerupItemArray[i].Price;
                    }
                    else
                    {
                        priceText.text = "";
                    }
                }
            }
        }

        UpdateButton();

    }
    /// <summary>
    /// Updates the Buy/Equipbutton based on what is in the middle of the screen. Changes text to Equip when items that are already bought are selected.
    /// </summary>
    public void UpdateButton()
    {
        if (currentlyActive == "SkinObject")
        {
            if (skinItemArray[currentItemShown].Unlocked == true)
            {
                buyButtonText.text = "Equip";
                if (skinItemArray[currentItemShown].Equipped == true)
                {
                    buyButton.interactable = false;
                }
                else
                {
                    buyButton.interactable = true;
                }
            }
            else
            {
                buyButtonText.text = "Buy";
                if (buyButton.interactable == false)
                {
                    buyButton.interactable = true;
                }
            }
        }
        else if (currentlyActive == "PowerupObject")
        {
            buyButtonText.text = "Buy";
            if (buyButton.interactable == false)
            {
                buyButton.interactable = true;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">name of the object in the editor that should be toogled</param>
    /// <param name="closeWindow">bool that should be true if you wish to close the currently active window</param>
    /// <param name="showMainMenu">bool that should be true if the main menu should be shown</param>
    private void ToogleMainMenuWindow(string name, bool closeWindow, bool showMainMenu)
    {
        Debug.Log("Opening/Closing " + name);

        GameObject tempObject = GameObject.Find(name);

        for (int i = 0; i < tempObject.transform.childCount; i++)
        {
            if (closeWindow == false)
            {
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
    /// <summary>
    /// Hides or shows the main menu
    /// </summary>
    /// <param name="state"></param>
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
