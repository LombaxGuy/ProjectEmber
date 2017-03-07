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
    [SerializeField]
    private GameObject buttonGameObject;
    [SerializeField]
    private GameObject wellEmpty;
    [SerializeField]
    private GameObject levelsEmpty;

    private Vector2 touchStart;
    private Vector2 touchEnd;
    private bool inWell;
    private Vector3 wellEmptyStartLocation;
    private Vector3 levelsEmptyStartLocation;

	// Use this for initialization
	void Start ()
    {
        wellEmptyStartLocation = wellEmpty.transform.position;
        levelsEmptyStartLocation = levelsEmpty.transform.position;
        inWell = true;
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
                    //Need better swipe
                    Vector2 secTouch = Input.GetTouch(0).position;
                    Vector3 touchTemp = touchStart - secTouch;
                    touchTemp.Normalize();
                    wellEmpty.transform.position = new Vector3(wellEmpty.transform.position.x - (touchTemp.x * 3), wellEmpty.transform.position.y, wellEmpty.transform.position.z);
                    touchStart = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:        
                    //When touch is ended, one of the wells need to be centeret!       
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
                    //Need better swipe
                    Vector2 secTouch = Input.GetTouch(0).position;
                    Vector3 touchTemp = touchStart - secTouch;
                    touchTemp.Normalize();
                    levelsEmpty.transform.position = new Vector3(levelsEmpty.transform.position.x, levelsEmpty.transform.position.y - (touchTemp.y * 3), levelsEmpty.transform.position.z);
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
            //Screen.width;
            //Screen.height;
            
            if(i % 2 == 0)
            {
                temp.transform.localPosition = new Vector3(200, -900 + (i * 200), 0);
            }
            else
            {
                temp.transform.localPosition = new Vector3(-200, -900 + (i * 200), 0);
            }
            
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
