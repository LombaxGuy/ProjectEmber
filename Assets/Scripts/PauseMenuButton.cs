using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour {

    [SerializeField]
    private GameObject pauseCanvas;

    public void OnButtonClick()
    {
        pauseCanvas.SetActive(true);
    }
}
