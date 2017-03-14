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

    private string currentlyActive = "";

    private bool sfxOff = false;
    private bool musicOff = false;
    private bool mainMenuHidden = false;

    public string CurrentlyActive
    {
        get { return currentlyActive; }
        set { currentlyActive = value; }
    }

    private void Start()
    {
        mainMenuObject = GameObject.Find("MainMenuObject");
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

    private GameObject levelSelectCanvas;


    public void ContinueButton()
    {
        levelSelectCanvas.SetActive(true);
    }

    public void ShopButton()
    {
        ToogleMainMenuWindow("ShopObject", false, false);
        CurrentlyActive = "ShopObject";
    }

    public void PowerUpButton()
    {
        ToogleMainMenuWindow("PowerupObject", false, false);
        ToogleMainMenuWindow("ShopObject", true, false);
        CurrentlyActive = "PowerupObject";
    }

    public void SkinButton()
    {
        ToogleMainMenuWindow("SkinObject", false, false);
        ToogleMainMenuWindow("ShopObject", true, false);
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

    private void ToogleMainMenuWindow(string name, bool closeWindow, bool showMainMenu)
    {
        Debug.Log("Opening/Closing " + name);

        GameObject tempObject = GameObject.Find(name);

        for (int i = 0; i < tempObject.transform.childCount; i++)
        {
            if (closeWindow == false)
            {
                tempObject.transform.GetChild(i).GetComponent<Image>().enabled = true;
                if (tempObject.transform.GetChild(i).transform.childCount > 0)
                {
                    tempObject.transform.GetChild(i).GetComponentInChildren<Text>().enabled = true;
                }
            }
            else
            {
                tempObject.transform.GetChild(i).GetComponent<Image>().enabled = false;
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
