using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private GameObject dropDownPowerUp;

    private GameObject powerupUI;

    private float uiStartPosition;

    private float uiOpenPosition = 100;

    private bool isHidden = true;

    private Text scoreText;
    private Text ratingText;

    private Button continueButton;
    private GameObject endScreen;
    private GameObject pauseScreen;

    private UIVisibilityControl endVisibilityCtrl;
    private UIVisibilityControl pauseVisibilityCtrl;

    public bool fuckYourShittyEndScreens = false;

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
        powerupUI = GameObject.Find("PowerUpUI");

        endVisibilityCtrl = GameObject.Find("EndScreenObject").GetComponent<UIVisibilityControl>();
        pauseVisibilityCtrl = GameObject.Find("PauseMenuObject").GetComponent<UIVisibilityControl>();

        uiStartPosition = powerupUI.transform.position.y;
        Debug.Log(uiStartPosition);
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        ratingText = GameObject.Find("RatingText").GetComponent<Text>();

        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        endScreen = GameObject.Find("EndScreenObject");
        pauseScreen = GameObject.Find("PauseMenuObject");
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
        pauseVisibilityCtrl.ToggleUI();
        if (isHidden == false)
        {
            OnShowPowerUpUiButtonClick();
        }

        if(pauseVisibilityCtrl.CurrentlyVisible == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void OnChangePowerUpButtonClick()
    {
        Image image = dropDownPowerUp.GetComponent<Image>();
        if (image.enabled == true || isHidden == true)
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

    public void OnShowPowerUpUiButtonClick()
    {
        if (isHidden == true && pauseVisibilityCtrl.CurrentlyVisible == false)
        {
            isHidden = false;
            powerupUI.transform.position = Vector3.MoveTowards(powerupUI.transform.position, new Vector3(powerupUI.transform.position.x, uiOpenPosition, powerupUI.transform.position.z), 500);
        }
        else if (isHidden == false || pauseVisibilityCtrl.CurrentlyVisible == true)
        {
            isHidden = true;
            powerupUI.transform.position = Vector3.MoveTowards(powerupUI.transform.position, new Vector3(powerupUI.transform.position.x, uiStartPosition, powerupUI.transform.position.z), 500);
            OnChangePowerUpButtonClick();
        }

    }

    private void GameLostUI()
    {
        if (!fuckYourShittyEndScreens)
        {
            endVisibilityCtrl.ShowUI();
            continueButton.GetComponent<Image>().enabled = false;
            continueButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
        }
    }

    private void GameWonUI()
    {
        if (!fuckYourShittyEndScreens)
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
