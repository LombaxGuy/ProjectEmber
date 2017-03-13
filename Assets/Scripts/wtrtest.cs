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
        health = health - 1;
        t = 0.01f;
        StartCoroutine(MoveIt());
    }

    private void OnIgnite(int health)
    {
        udregn();
    }


    // Use this for initialization
    void Start () {
		A = gameObject.transform.position;
        B = target.transform.position;
        health = 3;
        points = new float[health];

        distance = Vector3.Distance( new Vector3(A.x,A.y,A.z), new Vector3(A.x, B.y, A.z));
    }
	
	// Update is called once per frame
	void Update () {
        C = gameObject.transform.position;

        if (Input.GetKeyUp(KeyCode.G))
        {
            udregn();
        }

        if (Input.GetKey(KeyCode.H))
        {
            MoveIt();
        }
    }

    private void udregn()
    {
            
            jump = (distance / health);
            int f = 1;
            for (int i = health; i > 0; i--)
            {
            
                if (i > 0)
                {
                    points[i - 1] = A.y + (jump * f);
                }

            f++;        
            }
    }    

    private void move()
    {


    }

     private IEnumerator MoveIt()
    {
        Debug.Log("courotine");
        while (C.y <= B.y - 0.01f)
        {
            t += 0.1f * Time.deltaTime;
            gameObject.transform.position = new Vector3(A.x, Mathf.Lerp(C.y, points[health - 1], t), A.z);
        }
        if (C.y >= B.y - 0.01f)
        {
            A.y = C.y;
        }
        yield return new WaitForSeconds(0.1f);
    }
}
