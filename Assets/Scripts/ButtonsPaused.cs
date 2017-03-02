using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsPaused : MonoBehaviour {

    private bool soundOff = false;
    private bool musicOff = false;

    public void ResumeButton()
    {
        this.gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        //Loads the first scene in the build which should be our menu
        //SceneManager.LoadScene(0);

    }

    public void SoundOnOff()
    {
        if(soundOff == true)
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
}
