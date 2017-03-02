using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour {

    [SerializeField]
    private GameObject pauseCanvas;

    public void OnPauseButtonClick()
    {
        pauseCanvas.SetActive(true);
        pauseCanvas.GetComponent<MenuScript>().CurrentlyActive = "pauseMenu";
    }
}
