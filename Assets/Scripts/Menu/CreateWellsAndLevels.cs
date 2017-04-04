using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CreateWellsAndLevels : MonoBehaviour
{
    [Header("Number of instances")]

    [Tooltip("The number of wells that should be created.")]
    [SerializeField]
    private int numberOfWells = 3;

    [Tooltip("The number of levels in each specific well.")]
    [SerializeField]
    private int[] numberOfLevelsInWell;

    [Header("Layout")]

    [Tooltip("The difference in x-position of each well in local space.")]
    [SerializeField]
    private float wellSpacing = 800;

    [Tooltip("The difference in y-position of each level in local space.")]
    [SerializeField]
    private float levelSpacing = 200;

    [Header("Prefabs")]

    [Tooltip("The button prefab used to create the well buttons.")]
    [SerializeField]
    private GameObject wellButtonPrefab;

    [Tooltip("The button prefab used to create the level buttons.")]
    [SerializeField]
    private GameObject levelButtonPrefab;

    [Header("GameObjects")]

    [Tooltip("The UI object that should move when in the well menu.")]
    [SerializeField]
    private GameObject wellSwipeObject;

    [Tooltip("The UI object that should move when in the level menu.")]
    [SerializeField]
    private GameObject levelSwipeObject;

    // Array of GameObjects that represents the level menu for each well.
    private GameObject[] levelMenuArray;

    // Array of GameObjects containing the GameObjects of the well-buttons.
    private GameObject[] wellButtonArray;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        // Initializes the array.
        numberOfLevelsInWell = new int[numberOfWells];
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // If the number in the array is changed a new array is initialized.
        if (numberOfLevelsInWell.Length != numberOfWells)
        {
            numberOfLevelsInWell = new int[numberOfWells];
        }
    }

    /// <summary>
    /// Used to destroy any GameObjects under the two menues. 
    /// </summary>
    private void DestroyOldInstances()
    {
        // If the child count of the level obejct is not 0... 
        if (wellSwipeObject.transform.childCount != 0)
        {
            //... all the children are destroyed.
            for (int i = wellSwipeObject.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(wellSwipeObject.transform.GetChild(i).gameObject);
            }
        }

        // Same as above.
        if (levelSwipeObject.transform.childCount != 0)
        {
            for (int i = levelSwipeObject.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(levelSwipeObject.transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// Used to build the UI buttons and elements.
    /// </summary>
    public void Build()
    {
        // Calls the DestroyOldInstances method.
        DestroyOldInstances();

        // Initializes the arrays.
        wellButtonArray = new GameObject[numberOfWells];
        levelMenuArray = new GameObject[numberOfWells];

        // Adds level menu objects to the array.
        for (int i = 0; i < numberOfWells; i++)
        {
            levelMenuArray[i] = new GameObject("LevelMenu" + (i + 1), typeof(RectTransform));
            levelMenuArray[i].transform.SetParent(levelSwipeObject.transform);
            levelMenuArray[i].AddComponent<UIVisibilityControl>();
            levelMenuArray[i].transform.localScale = Vector3.one;
            levelMenuArray[i].transform.localPosition = Vector3.zero;
        }

        
        for (int i = 0; i < levelMenuArray.Length; i++)
        {
            // Creates a well button.
            GameObject well = Instantiate(wellButtonPrefab);
            
            well.transform.SetParent(wellSwipeObject.transform);
            well.name = "Well_" + (i + 1).ToString();
            well.transform.localScale = Vector3.one;
            well.transform.localPosition = new Vector3 (i * wellSpacing, 0);

            well.GetComponentInChildren<Text>().text = "WellName" + (i + 1).ToString();
            
            // Adds the well to the array
            wellButtonArray[i] = well;

            for (int j = 1; j < numberOfLevelsInWell[i] + 1; j++)
            {
                // Adds levels to the level menues.
                GameObject level = Instantiate(levelButtonPrefab);

                level.transform.SetParent(levelMenuArray[i].transform);
                level.name = "Level_" + j.ToString();
                level.transform.localScale = Vector3.one;

                // Sets the positions of the levels
                if (j % 2 == 0)
                {
                    level.transform.localPosition = new Vector3(levelSpacing, j * levelSpacing, 0);
                }
                else
                {
                    level.transform.localPosition = new Vector3(-levelSpacing, j * levelSpacing, 0);
                }

                // Sets the text on the level button to the level number.
                level.GetComponentInChildren<Text>().text = j.ToString();
            }
        }
    }
}
