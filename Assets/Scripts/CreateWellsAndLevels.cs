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
    [SerializeField]
    private GameObject levelButtonPrefab;

    [SerializeField]
    private GameObject wellButtonPrefab;

    [Header("GameObjects")]

    [SerializeField]
    private GameObject wellSwipeObject;

    [SerializeField]
    private GameObject levelSwipeObject;

    private GameObject[] levelMenuArray;
    private GameObject[] wellButtonArray;

    // Use this for initialization
    void Start()
    {
        numberOfLevelsInWell = new int[numberOfWells];
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfLevelsInWell.Length != numberOfWells)
        {
            numberOfLevelsInWell = new int[numberOfWells];
        }
    }

    public void Build()
    {
        if (levelMenuArray != null && levelMenuArray.Length != 0)
        {
            for (int i = 0; i < levelMenuArray.Length; i++)
            {
                DestroyImmediate(levelMenuArray[i]);
            }
        }

        if (wellButtonArray != null && wellButtonArray.Length != 0)
        {
            for (int i = 0; i < wellButtonArray.Length; i++)
            {
                DestroyImmediate(wellButtonArray[i]);
            }
        }

        wellButtonArray = new GameObject[numberOfWells];
        levelMenuArray = new GameObject[numberOfWells];

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
            GameObject well = Instantiate(wellButtonPrefab);
            
            well.transform.SetParent(wellSwipeObject.transform);
            well.name = "Well_" + (i + 1).ToString();
            well.transform.localScale = Vector3.one;
            well.transform.localPosition = new Vector3 (i * wellSpacing, 0);

            well.GetComponentInChildren<Text>().text = "WellName" + (i + 1).ToString();

            wellButtonArray[i] = well;

            for (int j = 1; j < numberOfLevelsInWell[i] + 1; j++)
            {
                GameObject level = Instantiate(levelButtonPrefab);

                level.transform.SetParent(levelMenuArray[i].transform);
                level.name = "Level_" + j.ToString();
                level.transform.localScale = Vector3.one;

                if (j % 2 == 0)
                {
                    level.transform.localPosition = new Vector3(levelSpacing, j * levelSpacing, 0);
                }
                else
                {
                    level.transform.localPosition = new Vector3(-levelSpacing, j * levelSpacing, 0);
                }

                level.GetComponentInChildren<Text>().text = j.ToString();
            }
        }
    }
}
