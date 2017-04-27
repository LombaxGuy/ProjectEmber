using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    #region UIVariables
    private Text priceText;
    private Button buyButton;
    private Text coinText;
    private Text buyButtonText;
    private Text currentlyOwnedText;
    private GameObject mainMenuObject;
    private GameObject shopMenuObject;
    private bool mainMenuHidden = false;
    #endregion

    #region ShopVariables
    private bool updateObjectPosition = false;

    private string currentlyActive = "";

    private ShopItem[] skinItemArray;
    private ShopItem[] powerupItemArray;
    private ShopItem[] coinItemArray;

    //Used until we determine prices for stuff, then becomes obsolete and needs deletion
    private int rngNumber;

    private int currentSkinEquipped = 0;
    private int currentItemShown = 0;
    private int snappedPosInt = 0;
    #endregion

    //Items in this region becomes obsolete when a proper inventory system is made
    #region BogusInventoryVariables
    private int coins = 0;

    private TransactionScript transactionScript = new TransactionScript();

    private int powerup0Count = 0;
    private int powerup1Count = 0;
    private int powerup2Count = 0;
    private int powerup3Count = 0;
    private int powerup4Count = 0;
    private int powerup5Count = 0;
    private int powerup6Count = 0;
    private int powerup7Count = 0;
    private int powerup8Count = 0;

    private List<string> itemIDList = new List<string>();
    #endregion

    #region Swipe
    private GameObject swipeSkinObject;
    private GameObject swipePowerupObject;
    private GameObject swipeCoinObject;

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
    private int coinElements;

    private bool snapToPosRunning = false;

    private Coroutine centeringCoroutine;
    #endregion

    #region Properties
    public string CurrentlyActive
    {
        get { return currentlyActive; }
        set { currentlyActive = value; }
    }

    public int Coins
    {
        get
        {
            return coins;
        }

        set
        {
            coins = value;
        }
    }
    #endregion

    private InGameUI ingameUI;

    private void Start()
    {
        //Stuff that only needs to be run in the main menu
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            priceText = GameObject.Find("Pricetag").GetComponent<Text>();
            coinText = GameObject.Find("CoinText").GetComponent<Text>();
            buyButton = GameObject.Find("BuyEquipButton").GetComponent<Button>();
            buyButtonText = GameObject.Find("BuyEquipText").GetComponent<Text>();
            swipeSkinObject = GameObject.Find("SkinObject");
            swipePowerupObject = GameObject.Find("PowerupObject");
            swipeCoinObject = GameObject.Find("CoinObject");
            currentlyOwnedText = GameObject.Find("CurrentlyOwned").GetComponent<Text>();

            skinElements = swipeSkinObject.transform.childCount;
            powerupElements = swipePowerupObject.transform.childCount;
            coinElements = swipeCoinObject.transform.childCount;

            skinItemArray = new ShopItem[skinElements];
            powerupItemArray = new ShopItem[powerupElements];
            coinItemArray = new ShopItem[coinElements];

            CalculateCaps(skinElements);

            SetupShopItems();

            mainMenuObject = GameObject.Find("MainMenuObject");
            transactionScript.OnStartUp(skinElements, powerupElements, coinElements);
        }
    }

    private void Update()
    {
        Debug.Log(CurrentlyActive);
        if (currentlyActive == "SkinObject")
        {
            HandleSwipeHorizontal(swipeSkinObject);
            CalculateCaps(skinElements);
        }
        else if (currentlyActive == "PowerupObject")
        {
            HandleSwipeHorizontal(swipePowerupObject);
            CalculateCaps(powerupElements);
            switch (currentItemShown)
            {
                case 0:
                    currentlyOwnedText.text = powerup0Count.ToString();
                    break;
                case 1:
                    currentlyOwnedText.text = powerup1Count.ToString();
                    break;
                case 2:
                    currentlyOwnedText.text = powerup2Count.ToString();
                    break;
                case 3:
                    currentlyOwnedText.text = powerup3Count.ToString();
                    break;
                case 4:
                    currentlyOwnedText.text = powerup4Count.ToString();
                    break;
                case 5:
                    currentlyOwnedText.text = powerup5Count.ToString();
                    break;
                case 6:
                    currentlyOwnedText.text = powerup6Count.ToString();
                    break;
                case 7:
                    currentlyOwnedText.text = powerup7Count.ToString();
                    break;
                case 8:
                    currentlyOwnedText.text = powerup8Count.ToString();
                    break;

            }
        }
        else if (CurrentlyActive == "CoinObject")
        {
            HandleSwipeHorizontal(swipeCoinObject);
            CalculateCaps(coinElements);
        }
        if (updateObjectPosition == true)
        {
            if (snapToPosRunning == false)
            {
                UpdateCurrentlySelected();
                updateObjectPosition = false;
            }
        }
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            coinText.text = Coins.ToString();
        }
    }

    #region Shop
    private void SetupShopItems()
    {
        //Sets up an array containing all of the shopitems. Here their position is set which is used later for determining which skin is in the center
        //Here the price for the skins is also set tempoarily. System will be updated later on
        //The normal skin will currently be equipped here, will be changed to the game equipping whatever is saved
        for (int i = 0; i < skinElements; i++)
        {
            skinItemArray[i] = new ShopItem();
            if (i == 0)
            {
                skinItemArray[i].ItemPosition = 138;
                skinItemArray[i].Unlocked = true;
                skinItemArray[i].Equipped = true;
            }
            else
            {
                skinItemArray[i].ItemPosition = skinItemArray[i - 1].ItemPosition - 220;
            }
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
                powerupItemArray[i].ItemPosition = 138;
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

        for (int i = 0; i < coinElements; i++)
        {
            coinItemArray[i] = new ShopItem();

            if (i == 0)
            {
                coinItemArray[i].ItemPosition = 138;
            }
            else
            {
                coinItemArray[i].ItemPosition = coinItemArray[i - 1].ItemPosition - 220;
            }

            rngNumber = Random.Range(1, 5);
            switch (rngNumber)
            {
                case 1:
                    coinItemArray[i].Price = "1.50€";
                    break;
                case 2:
                    coinItemArray[i].Price = "5€";
                    break;
                case 3:
                    coinItemArray[i].Price = "7.50€";
                    break;
                case 4:
                    coinItemArray[i].Price = "9.99€";
                    break;

            }
        }
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
                transactionScript.BuyShopItem(currentItemShown, currentlyActive, skinItemArray[currentItemShown]);
                priceText.text = "";
            }
        }
        else if (currentlyActive == "PowerupObject")
        {
            transactionScript.BuyShopItem(currentItemShown, currentlyActive, powerupItemArray[currentItemShown]);
        }
        else if (currentlyActive == "CoinObject")
        {
            transactionScript.BuyShopItem(currentItemShown, currentlyActive, coinItemArray[currentItemShown]);
        }
        UpdateButton();
    }

    public void AddToList(string itemID)
    {
        itemIDList.Add(itemID);
        Debug.Log(itemIDList.Count);
        UpdatePowerupCounts();
    }
    #endregion

    #region UI
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
            case "CoinObject":
                ToogleMainMenuWindow("CoinObject", true, false);
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
        else if (CurrentlyActive == "CoinObject")
        {
            ToogleMainMenuWindow("CoinObject", true, false);
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
        else if (CurrentlyActive == "CoinObject")
        {
            ToogleMainMenuWindow("CoinObject", true, false);
        }
        shopMenuObject = GameObject.Find("SkinObject");
        CurrentlyActive = "SkinObject";
        currentlyOwnedText.text = "";
    }

    public void CoinButton()
    {
        ToogleMainMenuWindow("CoinObject", false, false);
        if (CurrentlyActive == "SkinObject")
        {
            ToogleMainMenuWindow("SkinObject", true, false);
        }
        else if (CurrentlyActive == "PowerupObject")
        {
            ToogleMainMenuWindow("PowerupObject", true, false);
        }
        shopMenuObject = GameObject.Find("CoinObject");
        CurrentlyActive = "CoinObject";
        currentlyOwnedText.text = "";
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
        Debug.Log("Blargh1");
        //Converts the currently selected swipeobjects position to an int and then determines which of the current objects is in center
        if (currentlyActive == "SkinObject")
        {
            Debug.Log("Blargh2");
            snappedPosInt = Mathf.RoundToInt(swipeSkinObject.transform.position.x);
            Debug.Log("Hulubulu " + snappedPosInt);
            for (int i = 0; i < skinItemArray.Length; i++)
            {
                Debug.Log("Blargh3");
                Debug.Log("ITEM POSITION" + skinItemArray[i].ItemPosition);
                if (snappedPosInt >= skinItemArray[i].ItemPosition && snappedPosInt <= skinItemArray[i].ItemPosition + 5 || snappedPosInt <= skinItemArray[i].ItemPosition && snappedPosInt >= skinItemArray[i].ItemPosition - 5)
                {
                    Debug.Log("Blargh4");
                    currentItemShown = i;
                    if (!skinItemArray[currentItemShown].Unlocked)
                    {
                        Debug.Log("Blargh5");
                        priceText.text = skinItemArray[i].Price;
                    }
                    else
                    {
                        Debug.Log("Blargh6");
                        priceText.text = "";
                    }
                }
            }
        }
        else if (currentlyActive == "PowerupObject")
        {
            Debug.Log("Blargh7");
            snappedPosInt = Mathf.RoundToInt(swipePowerupObject.transform.position.x);
            for (int i = 0; i < powerupItemArray.Length; i++)
            {
                Debug.Log("Blargh8");
                if (snappedPosInt >= powerupItemArray[i].ItemPosition && snappedPosInt <= powerupItemArray[i].ItemPosition + 5 || snappedPosInt <= powerupItemArray[i].ItemPosition && snappedPosInt >= powerupItemArray[i].ItemPosition - 5)
                {
                    Debug.Log("Blargh9");
                    currentItemShown = i;
                    priceText.text = powerupItemArray[i].Price;
                }
            }
        }
        else if (currentlyActive == "CoinObject")
        {
            Debug.Log("Blargh10");
            snappedPosInt = Mathf.RoundToInt(swipeCoinObject.transform.position.x);
            for (int i = 0; i < coinItemArray.Length; i++)
            {
                Debug.Log("Blargh11");
                if (snappedPosInt >= coinItemArray[i].ItemPosition && snappedPosInt <= coinItemArray[i].ItemPosition + 5 || snappedPosInt <= coinItemArray[i].ItemPosition && snappedPosInt >= coinItemArray[i].ItemPosition - 5)
                {
                    Debug.Log("Blargh12");
                    currentItemShown = i;
                    priceText.text = coinItemArray[i].Price;
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
        else if (currentlyActive == "CoinObject")
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

        UIVisibilityControl tempControl = GameObject.Find(name).GetComponent<UIVisibilityControl>();

        if (closeWindow == false)
        {
            tempControl.ShowUI();
        }
        else
        {
            tempControl.HideUI();
        }

        ToogleMainMenu(showMainMenu);


    }
    /// <summary>
    /// Hides or shows the main menu
    /// </summary>
    /// <param name="state"></param>
    private void ToogleMainMenu(bool state)
    {
        UIVisibilityControl tempControl = GameObject.Find("MainMenuObject").GetComponent<UIVisibilityControl>();
        if (state == true)
        {
            tempControl.ShowUI();
            mainMenuHidden = false;
        }
        else
        {
            if (mainMenuHidden == false)
            {
                tempControl.HideUI();
                mainMenuHidden = true;
            }
        }
    }

    private void UpdatePowerupCounts()
    {
        switch (itemIDList[itemIDList.Count - 1])
        {
            case "powerup00":
                powerup0Count++;
                break;
            case "powerup01":
                powerup1Count++;
                break;
            case "powerup02":
                powerup2Count++;
                break;
            case "powerup03":
                powerup3Count++;
                break;
            case "powerup04":
                powerup4Count++;
                break;
            case "powerup05":
                powerup5Count++;
                break;
            case "powerup06":
                powerup6Count++;
                break;
            case "powerup07":
                powerup7Count++;
                break;
            case "powerup08":
                powerup8Count++;
                break;

        }
    }
    #endregion
}
