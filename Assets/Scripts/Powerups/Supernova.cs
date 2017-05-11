using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supernova : Powerup {

    private GameObject supernovaPrefab;

    private float speed;

    private GameObject player;

    private Vector3 startPosition;

    private Vector3 endPosition;

    private float distance;

    private float startTime;

    private float timeToEnd;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //PowerupInUse();
    }


    private void PowerupInUse()
    {
        timeToEnd -= Time.deltaTime;

        float distCovered = (Time.time - startTime) * speed;

        float fracJourney = distCovered / distance;

        player.transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);

        // Create fire everywhere or create fire on the way to world size. TODO
        
        // when hit world size, level complete
        if(timeToEnd <= 0)
        {
            EventManager.InvokeOnLevelCompleted();
            ResetValues();
        }
                      
    }

    public override void NextTurn()
    {
        throw new NotImplementedException();
    }

    public override void ResetValues()
    {
        //>Do shit before destroying?
        GameObject.Destroy(gameObject);
    }

    public override void UsePowerup()
    {
        //Speed need to be changed later TODO
        speed = 1337;
        player = GameObject.FindGameObjectWithTag("Projectile");
        startPosition = player.transform.position;
        //Ignore, will get updated later TODO (this is the "levelCompleted" position)
        endPosition = new Vector3(0, 0, 0);
        distance = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;
        //maybe the distance can help with this number? TODO
        timeToEnd = 3; //or
        timeToEnd = distance; //or something different. distance / 2?
    }

    
}
