using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private GameObject dropDownPowerUp;

    private GameObject powerUpUI;

    private float uiStartPosition;

    private float uiOpenPosition = 100;

    private bool powerUpIsHidden = true;

    private Text scoreText;
    private Text ratingText;

    private Button continueButton;
    private Button pauseButton;

    private UIVisibilityControl endVisibilityCtrl;
    private UIVisibilityControl pauseVisibilityCtrl;
    private UIVisibilityControl powerupVisibilityCtrl;

    public bool disableEndScreens = false;

    private void OnEnable()
    {
        EventManager.OnLevelCompleted += GameWonUI;
        EventManager.OnLevelLost += GameLostUI;
        EventManager.OnGameWorldReset += CloseUI;
    }

    private void OnDisable()
    {
        EventManager.OnLevelCompleted -= GameWonUI;
        EventManager.OnLevelLost -= GameLostUI;
        EventManager.OnGameWorldReset -= CloseUI;
    }

    private void Start()
    {
        dropDownPowerUp = GameObject.Find("PowerUpDropdown"); ;
        powerUpUI = GameObject.Find("PowerUpUI");

        endVisibilityCtrl = GameObject.Find("EndScreenObject").GetComponent<UIVisibilityControl>();
        pauseVisibilityCtrl = GameObject.Find("PauseMenuObject").GetComponent<UIVisibilityControl>();
        powerupVisibilityCtrl = GameObject.Find("PowerUpUI").GetComponent<UIVisibilityControl>();

        uiStartPosition = powerUpUI.transform.position.y;
        Debug.Log(uiStartPosition);
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        ratingText = GameObject.Find("RatingText").GetComponent<Text>();

        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        pauseButton = GameObject.Find("PauseMenuButton").GetComponent<Button>();
    }

    /// <summary>
    /// Tempcode to call OnLevelCompleted and OnLevelLost events for testing
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.InvokeOnLevelCompleted();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EventManager.InvokeOnLevelLost();
        }
    }

    /// <summary>
    /// Shows the pausemenu UI while hiding the powerupUI and the pausebutton
    /// </summary>
    public void OnPauseButtonClick()
    {
        pauseVisibilityCtrl.ShowUI();

        HideGameRunningUI();

        Time.timeScale = 0;

    }

    /// <summary>
    /// Hides the pause button
    /// </summary>
    private void HideGameRunningUI()
    {
        HidePowerUpUI();
        powerupVisibilityCtrl.HideUI();

        pauseButton.GetComponent<Image>().enabled = false;
        pauseButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
    }
    
    /// <summary>
    /// Shows the pausebutton
    /// </summary>
    private void ShowGameRunningUI()
    {
        powerupVisibilityCtrl.ShowUI();
        pauseButton.GetComponent<Image>().enabled = true;
        pauseButton.transform.GetChild(0).GetComponent<Text>().enabled = true;
    }

    /// <summary>
    /// Resumes the game and shows the pause button
    /// </summary>
    public void OnResumeGame()
    {
        pauseVisibilityCtrl.HideUI();

        ShowGameRunningUI();

        Time.timeScale = 1;
    }

    /// <summary>
    /// Toogles the PowerUpDropdown UI
    /// </summary>
    public void TooglePowerUpDropdownUI()
    {
        Image image = dropDownPowerUp.GetComponent<Image>();
        if (image.enabled == true || powerUpIsHidden == true)
        {
            image.enabled = false;
            dropDownPowerUp.transform.Find("Arrow").GetComponent<Image>().enabled = false;
            dropDownPowerUp.GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            image.enabled = true;
            dropDownPowerUp.transform.Find("Arrow").GetComponent<Image>().enabled = true;
            dropDownPowerUp.GetComponentInChildren<Text>().enabled = true;
        }
    }

    public void OnUsePowerUpButtonClick()
    {
        //Start UsePowerUp Event
    }

    /// <summary>
    /// Hides the powerup UI
    /// </summary>
    public void HidePowerUpUI()
    {
        powerUpIsHidden = true;
        powerUpUI.transform.position = new Vector3(powerUpUI.transform.position.x, uiStartPosition, powerUpUI.transform.position.z);

    }

    /// <summary>
    /// Toggles the powerup UI
    /// </summary>
    public void TooglePowerUpUI()
    {
        if (powerUpIsHidden == true && pauseVisibilityCtrl.CurrentlyVisible == false)
        {
            powerUpIsHidden = false;
            powerUpUI.transform.position = Vector3.MoveTowards(powerUpUI.transform.position, new Vector3(powerUpUI.transform.position.x, uiOpenPosition, powerUpUI.transform.position.z), 500);
        }
        else if (powerUpIsHidden == false || pauseVisibilityCtrl.CurrentlyVisible == true)
        {
            powerUpIsHidden = true;
            powerUpUI.transform.position = Vector3.MoveTowards(powerUpUI.transform.position, new Vector3(powerUpUI.transform.position.x, uiStartPosition, powerUpUI.transform.position.z), 500);
            TooglePowerUpDropdownUI();
        }

    }

    /// <summary>
    /// Makes the gamelost endscreen pop up if the disableEndScreens variable is false
    /// </summary>
    private void GameLostUI()
    {
        if (!disableEndScreens)
        {
            endVisibilityCtrl.ShowUI();
            continueButton.GetComponent<Image>().enabled = false;
            continueButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
            HideGameRunningUI();
        }
    }

    /// <summary>
    /// Makes the gamewon endscreen pop up if the disableEndScreens variable is false
    /// </summary>
    private void GameWonUI()
    {
        if (!disableEndScreens)
        {
            endVisibilityCtrl.ShowUI();
            HideGameRunningUI();
        }
    }
    
    /// <summary>
    /// Closes the endscreen
    /// </summary>
    private void CloseUI()
    {
        ShowGameRunningUI();
        endVisibilityCtrl.HideUI();

    }

    /// <summary>
    /// Continues to the next scene in the build index
    /// </summary>
    public void OnContinueButton()
    {
        if (SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1) != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void OnPlayAgainButton()
    {
        EventManager.InvokeOnGameWorldReset();
        OnResumeGame();
        CloseUI();
    }

    /// <summary>
    /// Changes the scene to main menu
    /// </summary>
    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
