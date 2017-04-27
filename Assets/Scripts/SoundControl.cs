using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundControl : MonoBehaviour {

    #region SoundVariables
    [SerializeField]
    private AudioMixer[] mixer;
    private bool sfxOff = false;
    private bool musicOff = false;
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Sound
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
    #endregion
}
