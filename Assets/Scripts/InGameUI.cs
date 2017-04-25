using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    private GameObject pauseCanvas;

    private Transform dropDownPowerUp;

    private Transform powerUpUI;

    private float uiStartPosition;

    private Vector3 uiStartPosition2;

    private float uiOpenPosition = 100;

    private bool isHidden = true;

    private void Start()
    {
        pauseCanvas = GameObject.FindGameObjectWithTag("PauseMenu");
        dropDownPowerUp = gameObject.transform.GetChild(1);
        powerUpUI = gameObject.transform.GetChild(2);

        uiStartPosition2 = transform.position;
    }

    public void OnPauseButtonClick()
    {
        Debug.Log(pauseCanvas);
        if (pauseCanvas.GetComponent<Canvas>().enabled == false)
        {
            pauseCanvas.GetComponent<Canvas>().enabled = true;
            OnShowPowerUpUiButtonClick();
        }
        else
        {
            pauseCanvas.GetComponent<Canvas>().enabled = false;
            pauseCanvas.GetComponent<MenuScript>().CurrentlyActive = "";
        }
    }

    public void OnChangePowerUpButtonClick()
    {
        Image image = dropDownPowerUp.GetComponent<Image>();
        if (image.enabled == true || isHidden == true)
        {
            image.enabled = false;
            dropDownPowerUp.Find("Arrow").GetComponent<Image>().enabled = false;
            dropDownPowerUp.GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            image.enabled = true;
            dropDownPowerUp.Find("Arrow").GetComponent<Image>().enabled = true;
            dropDownPowerUp.GetComponentInChildren<Text>().enabled = true;
        }
    }

    public void OnUsePowerUpButtonClick()
    {
        //Start UsePowerUp Event
    }

    public void OnShowPowerUpUiButtonClick()
    {
        Debug.Log(powerUpUI);
        if (isHidden == true && pauseCanvas.GetComponent<Canvas>().enabled == false)
        {
            isHidden = false;
            powerUpUI.position = Vector3.MoveTowards(powerUpUI.transform.position, new Vector3(powerUpUI.transform.position.x, uiStartPosition, powerUpUI.transform.position.z), 500);
        }
        else if (isHidden == false || pauseCanvas.GetComponent<Canvas>().enabled == true)
        {
            isHidden = true;
            powerUpUI.position = Vector3.MoveTowards(powerUpUI.transform.position, new Vector3(powerUpUI.transform.position.x, uiOpenPosition, powerUpUI.transform.position.z), 500);
            OnChangePowerUpButtonClick();
        }

    }
}
