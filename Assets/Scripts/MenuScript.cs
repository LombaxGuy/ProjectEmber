using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [SerializeField]
    private GameObject popupShop;
    [SerializeField]
    private GameObject popupPowerUpShop;
    [SerializeField]
    private GameObject popupSkinShop;
    [SerializeField]
    private GameObject popupSettings;
    [SerializeField]
    private GameObject popupCredits;
    [SerializeField]
    private GameObject popupMapSelect;

    private string currentlyActive = "";

    private bool soundOff = false;
    private bool musicOff = false;

    public string CurrentlyActive
    {
        get {return currentlyActive;}
        set{currentlyActive = value;}
    }

    public void PauseMenuExitButton()
    {
        //Loads the first scene in the build which should be our menu
        SceneManager.LoadScene(0);

    }

    public void SoundOnOff()
    {
        if (soundOff == true)
        {
            //Turn sound on 
            soundOff = false;
            PlayerPrefs.SetInt("sound", 0);
        }
        else
        {
            //Turn sound off
            soundOff = true;
            PlayerPrefs.SetInt("sound", 1);
        }

    }

    public void MusicOnOff()
    {
        if (musicOff == true)
        {
            //Turn music on 
            musicOff = false;
            PlayerPrefs.SetInt("music", 0);
        }
        else
        {
            //Turn music off
            musicOff = true;
            PlayerPrefs.SetInt("music", 1);
        }
    }


    public void CloseButton()
    {
        switch(CurrentlyActive)
        {
            case "shop":
                popupShop.SetActive(false);
                CurrentlyActive = "none";
                break;
            case "powerUp":
                popupPowerUpShop.SetActive(false);
                CurrentlyActive = "shop";
                break;
            case "skin":
                popupSkinShop.SetActive(false);
                CurrentlyActive = "shop";
                break;
            case "settings":
                popupSettings.SetActive(false);
                CurrentlyActive = "none";
                break;
            case "credits":
                popupCredits.SetActive(false);
                CurrentlyActive = "none";
                break;
            case "mapSelect":
                popupMapSelect.SetActive(false);
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
        popupShop.SetActive(true);
        CurrentlyActive = "shop";
    }

    public void PowerUpButton()
    {
        popupPowerUpShop.SetActive(true);
        CurrentlyActive = "powerUp";
    }

    public void SkinButton()
    {
        popupSkinShop.SetActive(true);
        CurrentlyActive = "skin";
    }

    public void SettingsButton()
    {
        popupSettings.SetActive(true);
        CurrentlyActive = "settings";
    }

    public void CreditsButton()
    {
        popupCredits.SetActive(true);
        CurrentlyActive = "credits";
    }

    public void MapSelectButton()
    {
        popupMapSelect.SetActive(true);
        CurrentlyActive = "mapSelect";
    }

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }
}
