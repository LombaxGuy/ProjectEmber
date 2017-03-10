using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wtrtest : MonoBehaviour {

    [SerializeField]
    GameObject target;

    Vector3 A;
    Vector3 B;
    Vector3 C;

    [SerializeField]
    int health;
    float t;

    [SerializeField]
    float[] points;
    float distance;

    public bool go;



	// Use this for initialization
	void Start () {
		A = gameObject.transform.position;
        B = target.transform.position;
        health = 3;
        points = new float[4];

        distance = Vector3.Distance( new Vector3(A.x,A.y,A.z), new Vector3(A.x, B.y, A.z));

    }
	
	// Update is called once per frame
	void Update () {
        C = gameObject.transform.position;

        if (Input.GetKey(KeyCode.G))
        {
            udregn();
        }

        if (Input.GetKey(KeyCode.H))
        {
            move();
        }
    }

    private void udregn()
    {

        distance = ((distance) / health);
        Debug.Log("go");
            for (int i = health; i >= 0; i--)
            {

                if (i > 0)
                {
                    points[i] = (A.y + distance) * i;
                }
                else
                {
                 points[i] = B.y;
                }
                
             
            }

    }

    

private void move()
    {
        if (C.y <= B.y - 0.01f)
        {
            t += 0.1f * Time.deltaTime;
            gameObject.transform.position = new Vector3(A.x, Mathf.Lerp(C.y, points[health], t), A.z);
        }
        else
        {
            A.y = C.y;
        }


    }
}
