using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    [SerializeField]
    private GameObject pauseCanvas;
    [SerializeField]
    private GameObject dropDownPowerUp;
    [SerializeField]
    private GameObject powerUpUI;

    private float visiblePowerUpUIPosition = 213;

    private float hiddenPowerUpUIPosition = 128;

    private bool isHidden = true;

    public void OnPauseButtonClick()
    {
        pauseCanvas.SetActive(true);
        pauseCanvas.GetComponent<MenuScript>().CurrentlyActive = "pauseMenu";
    }

    public void OnChangePowerUpButtonClick()
    {
        if (dropDownPowerUp.activeInHierarchy == true)
        {
            dropDownPowerUp.SetActive(false);
        }
        else
        {
            dropDownPowerUp.SetActive(true);
        }
    }

    public void OnUsePowerUpButtonClick()
    {
        //Start UsePowerUp Event
    }

    public void OnShowPowerUpUiButtonClick()
    {
        if (isHidden == true)
        {
            powerUpUI.transform.position = Vector3.MoveTowards(powerUpUI.transform.position, new Vector3(powerUpUI.transform.position.x, visiblePowerUpUIPosition, powerUpUI.transform.position.z), 500);
            isHidden = false;
            Debug.Log(isHidden);
        }
        else
        {
            powerUpUI.transform.position = Vector3.MoveTowards(powerUpUI.transform.position, new Vector3(powerUpUI.transform.position.x, hiddenPowerUpUIPosition, powerUpUI.transform.position.z), 500);
            dropDownPowerUp.SetActive(false);
            isHidden = true;
        }

    }
}
