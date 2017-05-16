using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InGameUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private WorldManager worldManager;

    #region Powerup Menu
    private GameObject powerupDropDown;
    private GameObject powerupUI;
    private Image powerupDropDownImage;

    private float powerupStartYPosition;

    private float powerupOpenPosition = 100;

    private bool powerupMenuClosed = true;

    private UIVisibilityControl powerupVisibilityCtrl;
    #endregion

    #region Pause Menu
    private Button continueButton;
    private Button pauseButton;

    private UIVisibilityControl pauseVisibilityCtrl;
    #endregion

    #region Ingame Indicators
    private Text roundsCountdownText;
    private Text scoreText;
    private Image shootMarker;
    #endregion

    #region End of game screens
    private UIVisibilityControl endVisibilityCtrl;
    #endregion

    [SerializeField]
    private bool disableEndScreens = false;

    private void OnEnable()
    {
        EventManager.OnShootingStarted += OnShootingStarted;
        EventManager.OnLevelCompleted += OnLevelCompleted;
        EventManager.OnLevelLost += OnLevelLost;
        EventManager.OnGameWorldReset += OnGameWorldReset;
        EventManager.OnEndOfTurn += OnEndOfTurn;
    }

    private void OnDisable()
    {
        EventManager.OnShootingStarted -= OnShootingStarted;
        EventManager.OnLevelCompleted -= OnLevelCompleted;
        EventManager.OnLevelLost -= OnLevelLost;
        EventManager.OnGameWorldReset -= OnGameWorldReset;
        EventManager.OnEndOfTurn -= OnEndOfTurn;
    }

    private void OnShootingStarted()
    {
        shootMarker.enabled = true;

        if (Input.touchCount > 0)
        {
            shootMarker.transform.position = Input.GetTouch(0).position;
        }
        else
        {
            shootMarker.transform.position = Input.mousePosition;
        }

    }

    /// <summary>
    /// Makes the gamewon endscreen pop up if the disableEndScreens variable is false
    /// </summary>
    private void OnLevelCompleted()
    {
        if (!disableEndScreens)
        {
            endVisibilityCtrl.ShowUI();
            HideInGameUI();
        }
    }

    /// <summary>
    /// Makes the gamelost endscreen pop up if the disableEndScreens variable is false
    /// </summary>
    private void OnLevelLost()
    {
        if (!disableEndScreens)
        {
            endVisibilityCtrl.ShowUI();
            continueButton.GetComponent<Image>().enabled = false;
            continueButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
            HideInGameUI();
        }
    }

    /// <summary>
    /// Closes the endscreen
    /// </summary>
    private void OnGameWorldReset()
    {
        ShowInGameUI();
        endVisibilityCtrl.HideUI();
        roundsCountdownText.text = worldManager.RoundsBeforeWaterRising.ToString();
    }

    private void OnEndOfTurn()
    {
        UpdateWaterText();
        UpdateScoreText();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.hovered.Contains(shootMarker.gameObject))
        {
            if (shootMarker.enabled == true)
            {
                shootMarker.color = Color.red;
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (eventData.hovered.Contains(shootMarker.gameObject))
        {
            if (shootMarker.enabled == true)
            {
                shootMarker.color = Color.green;
            }
        }
    }

    /// <summary>
    /// Shows the pausemenu UI while hiding the powerupUI and the pausebutton
    /// </summary>
    public void OnPauseButtonClick()
    {
        PauseGame();
    }

    /// <summary>
    /// Changes the scene to main menu
    /// </summary>
    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Resumes the game and shows the pause button
    /// </summary>
    public void OnResumeGameButtonClick()
    {
        ResumeGame();
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void OnPlayAgainButtonClick()
    {
        EventManager.InvokeOnGameWorldReset();

        ResumeGame();
    }

    /// <summary>
    /// Continues to the next scene in the build index
    /// </summary>
    public void OnContinueButtonClick()
    {
        if (SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1).IsValid())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    /// <summary>
    /// Toggles the powerup UI
    /// </summary>
    public void OnTogglePowerupUIButtonClick()
    {
        if (powerupMenuClosed == true && pauseVisibilityCtrl.CurrentlyVisible == false)
        {
            ShowPowerupUI();
        }
        else if (powerupMenuClosed == false || pauseVisibilityCtrl.CurrentlyVisible == true)
        {
            HidePowerupUI();
        }
    }

    /// <summary>
    /// Toogles the PowerUpDropdown UI
    /// </summary>
    public void OnTogglePowerupDropdownButtonClick()
    {
        if (powerupDropDownImage.enabled == true || powerupMenuClosed == true)
        {
            ShowPowerUpDropdown();
        }
        else
        {
            HidePowerUpDropdown();
        }
    }

    public void OnUsePowerUpButtonClick()
    {
        //Start UsePowerUp Event
    }

    private void Start()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();

        powerupDropDown = GameObject.Find("PowerUpDropdown"); ;
        powerupUI = GameObject.Find("PowerUpUI");
        powerupDropDownImage= powerupDropDown.GetComponent<Image>();

        endVisibilityCtrl = GameObject.Find("EndScreenObject").GetComponent<UIVisibilityControl>();
        pauseVisibilityCtrl = GameObject.Find("PauseMenuObject").GetComponent<UIVisibilityControl>();
        powerupVisibilityCtrl = GameObject.Find("PowerUpUI").GetComponent<UIVisibilityControl>();

        powerupStartYPosition = powerupUI.transform.position.y;

        shootMarker = GameObject.Find("StartShootMarker").GetComponent<Image>();

        roundsCountdownText = GameObject.Find("WaterText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        pauseButton = GameObject.Find("PauseMenuButton").GetComponent<Button>();

        roundsCountdownText.text = worldManager.RoundsBeforeWaterRising.ToString();
    }

    /// <summary>
    /// Tempcode to call OnLevelCompleted and OnLevelLost events for testing
    /// </summary>
    private void Update()
    {
        #if(DEBUG)
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.InvokeOnLevelCompleted();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EventManager.InvokeOnLevelLost();
        }

        //if (Input.GetMouseButtonUp(0) && shootMarkerImageComponent.enabled)
        //{
        //    shootMarkerImageComponent.enabled = false;
        //}
        #endif

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && shootMarker.enabled)
        {
            shootMarker.enabled = false;
        }
    }

    /// <summary>
    /// Hides the UI shown while game is not paused
    /// </summary>
    private void HideInGameUI()
    {
        HidePowerupUI();
        powerupVisibilityCtrl.HideUI();

        pauseButton.GetComponent<Image>().enabled = false;
        pauseButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
    }

    /// <summary>
    /// Shows the UI shown while game is not paused
    /// </summary>
    private void ShowInGameUI()
    {
        powerupVisibilityCtrl.ShowUI();
        pauseButton.GetComponent<Image>().enabled = true;
        pauseButton.transform.GetChild(0).GetComponent<Text>().enabled = true;
    }

    /// <summary>
    /// Hides the powerup UI
    /// </summary>
    private void HidePowerupUI()
    {
        powerupMenuClosed = true;
        powerupUI.transform.position = new Vector3(powerupUI.transform.position.x, powerupStartYPosition, powerupUI.transform.position.z);
        HidePowerUpDropdown();
    }

    private void ShowPowerupUI()
    {
        powerupMenuClosed = false;
        powerupUI.transform.position = Vector3.MoveTowards(powerupUI.transform.position, new Vector3(powerupUI.transform.position.x, powerupOpenPosition, powerupUI.transform.position.z), 500);
    }

    private void ShowPowerUpDropdown()
    {
        powerupDropDownImage.enabled = false;
        powerupDropDown.transform.Find("Arrow").GetComponent<Image>().enabled = false;
        powerupDropDown.GetComponentInChildren<Text>().enabled = false;
    }

    private void HidePowerUpDropdown()
    {
        powerupDropDownImage.enabled = true;
        powerupDropDown.transform.Find("Arrow").GetComponent<Image>().enabled = true;
        powerupDropDown.GetComponentInChildren<Text>().enabled = true;
    }

    private void UpdateWaterText()
    {
        if (worldManager.RoundsPassed < worldManager.RoundsBeforeWaterRising)
        {
            worldManager.RoundsPassed++;
            roundsCountdownText.text = (worldManager.RoundsBeforeWaterRising - worldManager.RoundsPassed).ToString();
            if (worldManager.RoundsPassed == worldManager.RoundsBeforeWaterRising)
            {
                roundsCountdownText.text = worldManager.RoundsAfterWaterRising.ToString();
            }
        }
        else
        {
            if (worldManager.RoundsPassed < (worldManager.RoundsBeforeWaterRising + worldManager.RoundsAfterWaterRising))
            {
                worldManager.RoundsPassed++;
                roundsCountdownText.text = worldManager.RoundsAfterWaterRising.ToString();
            }
        }
    }

    private void UpdateScoreText()
    {

    }

    private void ResumeGame()
    {
        pauseVisibilityCtrl.HideUI();

        ShowInGameUI();

        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        pauseVisibilityCtrl.ShowUI();

        HideInGameUI();

        Time.timeScale = 0;
    }
}
