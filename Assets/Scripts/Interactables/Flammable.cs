﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{

    private bool onFire = false;
    private Vector3 spawnPoint;

    [SerializeField]
    private Texture burntTexture;

    private Color B_color;
    private Color W_color;

    private Material myMaterial;

    public bool OnFire
    {
        get { return onFire; }
        set { onFire = value; }
    }

    public Vector3 SpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }

    private void OnEnable()
    {
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnGameWorldReset += OnWorldReset;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnGameWorldReset -= OnWorldReset;
    }

    private void OnIgnite(Flammable flammableObject)
    {
        if (flammableObject.gameObject == this.gameObject)
        {
            FlameHitTransition();
        }
    }

    private void OnWorldReset()
    {
        Reset();
    }

    // Use this for initialization
    void Start()
    {
        myMaterial = gameObject.GetComponent<Renderer>().material;
        B_color = new Color(0, 0, 0, 1);
        W_color = new Color(255, 255, 255, 1);

        spawnPoint = transform.GetChild(0).transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Reset()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        onFire = false;
    }

    private void FlameHitTransition()
    {
        StartCoroutine(Burn());
        
        gameObject.GetComponent<Collider>().enabled = false;
    }


    private IEnumerator Burn()
    {
        float t = 0;

        while (myMaterial.color != B_color)
        {
            t += Time.deltaTime / 4f;
            myMaterial.color = Color.Lerp(myMaterial.color, Color.black, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        myMaterial.mainTexture = burntTexture;
        t = 0;

        while (myMaterial.color != W_color)
        {
            t += Time.deltaTime / 4f;
            myMaterial.color = Color.Lerp(myMaterial.color, Color.white, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
    }

}
