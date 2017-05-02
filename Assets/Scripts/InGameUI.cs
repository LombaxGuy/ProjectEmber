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

    private UIVisibilityControl endVisibilityCtrl;
    private UIVisibilityControl pauseVisibilityCtrl;

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

        uiStartPosition = powerUpUI.transform.position.y;
        Debug.Log(uiStartPosition);
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        ratingText = GameObject.Find("RatingText").GetComponent<Text>();

        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
    }

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

    public void OnPauseButtonClick()
    {
        //pauseVisibilityCtrl.ShowUI();

        //HidePowerUpUI();

        //Time.timeScale = 0;


        pauseVisibilityCtrl.ToggleUI();

        if (powerUpIsHidden == false)
        {
            OnShowPowerUpUiButtonClick();
        }

        if (pauseVisibilityCtrl.CurrentlyVisible == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void OnResumeGame()
    {
        pauseVisibilityCtrl.HideUI();

        Time.timeScale = 1;
    }

    public void OnChangePowerUpButtonClick()
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

    public void HidePowerUpUI()
    {
        powerUpIsHidden = true;
        powerUpUI.transform.position = new Vector3(powerUpUI.transform.position.x, uiStartPosition, powerUpUI.transform.position.z);

    }

    public void OnShowPowerUpUiButtonClick()
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
            OnChangePowerUpButtonClick();
        }

    }

    private void GameLostUI()
    {
        if (!disableEndScreens)
        {
            endVisibilityCtrl.ShowUI();
            continueButton.GetComponent<Image>().enabled = false;
            continueButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
        }
    }

    private void GameWonUI()
    {
        if (!disableEndScreens)
        {
            endVisibilityCtrl.ShowUI();
        }
    }

    private void CloseUI()
    {
        endVisibilityCtrl.HideUI();
    }

    public void OnContinueButton()
    {
        if (SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1) != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void OnPlayAgainButton()
    {
        EventManager.InvokeOnGameWorldReset();

    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
