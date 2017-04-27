using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfScreenTempScript : MonoBehaviour
{

    private Text scoreText;
    private Text ratingText;

    private Button continueButton;
    private GameObject endScreen;

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

    // Use this for initialization
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        ratingText = GameObject.Find("RatingText").GetComponent<Text>();

        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        endScreen = GameObject.Find("EndScreenObject");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.InvokeOnLevelCompleted();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            EventManager.InvokeOnLevelLost();
        }
    }

    private void GameLostUI()
    {
        if (!fuckYourShittyEndScreens)
        {
            for (int i = 0; i < endScreen.transform.childCount; i++)
            {
                if (endScreen.transform.GetChild(i).name != "ContinueButton")
                {
                    if (endScreen.transform.GetChild(i).GetComponent<Image>() == true)
                    {
                        endScreen.transform.GetChild(i).GetComponent<Image>().enabled = true;
                    }
                    if (endScreen.transform.GetChild(i).GetComponent<Text>() == true)
                    {
                        endScreen.transform.GetChild(i).GetComponent<Text>().enabled = true;
                    }
                    if (endScreen.transform.GetChild(i).transform.childCount > 0)
                    {
                        endScreen.transform.GetChild(i).GetComponentInChildren<Text>().enabled = true;
                    }
                }
            }
        }
    }

    private void GameWonUI()
    {
        if (!fuckYourShittyEndScreens)
        {
            for (int i = 0; i < endScreen.transform.childCount; i++)
            {
                if (endScreen.transform.GetChild(i).GetComponent<Image>() == true)
                {
                    endScreen.transform.GetChild(i).GetComponent<Image>().enabled = true;
                }
                if (endScreen.transform.GetChild(i).GetComponent<Text>() == true)
                {
                    endScreen.transform.GetChild(i).GetComponent<Text>().enabled = true;
                }
                if (endScreen.transform.GetChild(i).transform.childCount > 0)
                {
                    endScreen.transform.GetChild(i).GetComponentInChildren<Text>().enabled = true;
                }
            }
        }
    }

    private void CloseUI()
    {
        for (int i = 0; i < endScreen.transform.childCount; i++)
        {
            if (endScreen.transform.GetChild(i).GetComponent<Image>() == true)
            {
                endScreen.transform.GetChild(i).GetComponent<Image>().enabled = false;
            }
            if (endScreen.transform.GetChild(i).GetComponent<Text>() == true)
            {
                endScreen.transform.GetChild(i).GetComponent<Text>().enabled = false;
            }
            if (endScreen.transform.GetChild(i).transform.childCount > 0)
            {
                endScreen.transform.GetChild(i).GetComponentInChildren<Text>().enabled = false;
            }
        }
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
