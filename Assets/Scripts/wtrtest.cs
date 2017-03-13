using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wtrtest : MonoBehaviour {

    [SerializeField]
    GameObject currentCheckpoint;

    Vector3 startPosition;
    Vector3 goalPosition;
    Vector3 currentPosition;

    [SerializeField]
    int travelSteps = 3;
    float t;

    [SerializeField]
    float[] stepPositions;
    [SerializeField]
    float distance;
    [SerializeField]
    float jump;

    public bool go;

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShot;
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShot;
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }

    private void OnShot(Vector3 dir, float force)
    {

    }

    private void OnDeath(int health)
    {
        travelSteps = travelSteps - 1;
        t = 0.01f;
        StartCoroutine(MoveIt());
    }

    private void OnIgnite(int health, Vector3 newCheckpoint)
    {
        goalPosition = newCheckpoint;
        SetTarget();
        udregn();
    }


    // Use this for initialization
    void Start () {
		startPosition = gameObject.transform.position;
        goalPosition = currentCheckpoint.transform.position;
        travelSteps = 3;
        stepPositions = new float[travelSteps + 1];

        distance = Vector3.Distance( new Vector3(startPosition.x,startPosition.y,startPosition.z), new Vector3(startPosition.x, goalPosition.y, startPosition.z));
    }
	
	// Update is called once per frame
	void Update () {
        currentPosition = gameObject.transform.position;

        if (Input.GetKeyUp(KeyCode.G))
        {
            udregn();
        }

        if (Input.GetKey(KeyCode.H))
        {
            MoveIt();
        }
    }

    private void SetTarget()
    {
        startPosition = gameObject.transform.position;
        //goalPosition = currentCheckpoint.transform.position;
        travelSteps = 3;
        stepPositions = new float[travelSteps + 1];

        distance = Vector3.Distance(new Vector3(startPosition.x, startPosition.y, startPosition.z), new Vector3(startPosition.x, goalPosition.y, startPosition.z));

    }

    private void udregn()
    {            
            jump = (distance / travelSteps);
            int f = 0;
            for (int i = travelSteps; i >= 0; i--)
            {
           
                    stepPositions[i] = startPosition.y + (jump * f);

            f++;        
            }
    }    

     private IEnumerator MoveIt()
    {

        Debug.Log("courotine");
        while (currentPosition.y < stepPositions[travelSteps])
        {
            if (currentPosition.y <= goalPosition.y - 0.01f)
            {
                t += 0.1f * Time.deltaTime;
                gameObject.transform.position = new Vector3(startPosition.x, Mathf.Lerp(currentPosition.y, stepPositions[travelSteps], t), startPosition.z);
            }
            else
            {
                startPosition.y = currentPosition.y;
            }
            yield return null;
        }     
    }
}
