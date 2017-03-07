using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour {

    //Folder name n, scene name n (n == a number) (for loop)
    //When well picked generate levels
    //Load scene
    //Prefab
    //

    [SerializeField]
    private Canvas wellCanvas;
    [SerializeField]
    private Canvas levelCanvas;
    private string wellName;
    [SerializeField]
    private GameObject buttonGameObject;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void PickWell(int numberOfLevels)
    {
        wellName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(wellName);
        wellCanvas.gameObject.SetActive(false);
        levelCanvas.gameObject.SetActive(true);
        for (int i = 1; i <= numberOfLevels; i++)
        {
            GameObject temp = Instantiate(buttonGameObject);
            temp.transform.SetParent(levelCanvas.transform);
            temp.name = i.ToString();
            temp.transform.position = new Vector3(0, -350 + ( i * 125 ), 0);
        }
        

        //Number is picked in the inspector???
        //Generate levels to the Well that is picked. If it the first well, the number will be 1 (to get the folder name later)
        //Levels will be generated with a number from 1-X. Prop max 10.
        //TODO
        //Prefab with level button (canvas will always be there)
        //For loop to create all the buttons the right places and give them a name (number) 

    }

    public void PickScene()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        SceneManager.LoadScene(wellName + EventSystem.current.currentSelectedGameObject.name);
        Debug.Log(wellName + "_" + EventSystem.current.currentSelectedGameObject.name);
        //Use wellFolder number to get the right folder for the level scenes
        //Get name, use it to find the right folder and then the name of the scene.
    }

    public void BackFromSceneSelection()
    {
        levelCanvas.gameObject.SetActive(false);
        wellCanvas.gameObject.SetActive(true);

    }
}
