using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectionManager : MonoBehaviour {
    
    //TODO
    //Get the right position to the level button so it fits with the screen size
    //Fix swipe. Distance? something different?
    //Swipe for wells need to center one of the wells when touch is ended
    
    private static string wellName;
    [Header("Button prefab for levels inside well")]
    [SerializeField]
    private GameObject buttonGameObject;
    [Header("Well empty GameObject in canvas")]
    [SerializeField]
    private GameObject wellEmpty;
    [Header("Levels empty GameObject in canvas")]
    [SerializeField]
    private GameObject levelsEmpty;

    private Vector2 touchStart;
    private Vector2 touchStartUpdate;
    private Vector2 touchEnd;
    private Vector2 secTouch;
    private Vector3 touchTemp;
    private bool inWell;
    private Vector3 wellEmptyStartLocation;
    private Vector3 levelsEmptyStartLocation;
    private int wellSelected;
    [Header("Number of wells in the scene")]
    [SerializeField]
    private int maxWells;

	// Use this for initialization
	void Start ()
    {
        wellEmptyStartLocation = wellEmpty.transform.position;
        levelsEmptyStartLocation = levelsEmpty.transform.position;
        inWell = true;
        wellSelected = 0;
        maxWells = 3;
    }
	
	// Update is called once per frame
	void Update () {
        if(inWell == true)
        {
            SwipeLeftOrRight();
        }else
        {
            SwipeUpOrDown();
        }
	}

    //Swipe left or right for picking wells
    private void SwipeLeftOrRight()
    {
        if(Input.touchCount == 1)
        {

            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    touchStart = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Moved:
                    secTouch = Input.GetTouch(0).position;
                    touchTemp = touchStart - secTouch;
                    touchTemp.Normalize();
                    wellEmpty.transform.localPosition = new Vector3(wellEmpty.transform.localPosition.x - (touchTemp.x * 50), wellEmpty.transform.localPosition.y, wellEmpty.transform.localPosition.z);              
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    if(touchTemp.x >= 0)
                    {
                        if(wellSelected < maxWells - 1)
                        {
                            wellSelected++;
                        }                    
                    }
                    else
                    {
                        if (wellSelected > 0)
                        {
                            wellSelected--;
                        }
                    }
                    wellEmpty.transform.localPosition = new Vector3(wellEmptyStartLocation.x - (((Screen.width / 2) + (Screen.width / 4)) * wellSelected), wellEmpty.transform.localPosition.y, wellEmpty.transform.localPosition.z);

                    Debug.Log("WellEmpty position : " + wellEmpty.transform.position);
                    Debug.Log("wellSelected : " + wellSelected);

                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }
    //Swipe up or down for picking levels inside wells
    private void SwipeUpOrDown()
    {
        if (Input.touchCount == 1)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    touchStart = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Moved:
                    secTouch = Input.GetTouch(0).position;
                    touchTemp = touchStart - secTouch;
                    float distance = Vector2.Distance(touchStart, secTouch);
                    if(distance >= 3)
                    {
                        distance = 3;
                    }
                    touchTemp.Normalize();
                    levelsEmpty.transform.Translate(0, -touchTemp.y * distance, 0);
                    touchStart = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }

    //When there is a well button clicked, this method wil run. It will remove and then populate canvas with new buttons.
    public void PickWell(int numberOfLevels)
    {
        wellName = EventSystem.current.currentSelectedGameObject.name;
        wellEmpty.SetActive(false);
        levelsEmpty.SetActive(true);
        inWell = false;
        for (int i = 1; i <= numberOfLevels; i++)
        {
            GameObject temp = Instantiate(buttonGameObject);
            temp.transform.SetParent(levelsEmpty.transform);
            temp.name = "Button_" + i.ToString();
            temp.transform.localScale = new Vector3(1,1,1);
            Debug.Log("Screen res : " + Screen.currentResolution);
            
            if(i % 2 == 0)
            {
                temp.transform.localPosition = new Vector3(Screen.width / 6, -Screen.height / 2 + (i * (Screen.height / 8)), 0);
            }
            else
            {
                temp.transform.localPosition = new Vector3(-(Screen.width / 6), -Screen.height / 2 + (i * (Screen.height / 8)), 0);
            }
            Debug.Log("Screen res : " + Screen.width + "x" + Screen.height);
            temp.GetComponentInChildren<Text>().text = i.ToString();
        }

        wellEmpty.transform.position = wellEmptyStartLocation;
    }

    //This changes to a level scene
    public void PickScene()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        SceneManager.LoadScene(wellName + "_" + EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
    }

    //Simple back button method. 
    public void BackFromSceneSelection()
    {
        if(inWell == true)
        {
            
        }else
        {
            foreach (Transform child in levelsEmpty.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            levelsEmpty.SetActive(false);
            wellEmpty.SetActive(true);
            levelsEmpty.transform.position = levelsEmptyStartLocation;
            inWell = true;
        }
      
    }
}
