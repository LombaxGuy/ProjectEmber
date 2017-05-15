using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{

    private bool onFire = false;
    private Vector3 spawnPoint;

    [SerializeField]
    private GameObject spawn;

    private ParticleSystem[] fireParticleSystems;

    [SerializeField]
    private Texture burntTexture;

    float burntState;

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

        spawnPoint = spawn.transform.position;

        fireParticleSystems = GetComponentsInChildren<ParticleSystem>();

        ControlParticles(false);
    }

    // Update is called once per frame
    void Update()
    {
        burntState = this.GetComponent<Renderer>().material.GetFloat("_DissolveAmount");
        if (spawnPoint != spawn.transform.position)
        {
            spawnPoint = spawn.transform.position;
        }
    }

    private void Reset()
    {
        //gameObject.GetComponent<Collider>().enabled = true;
        onFire = false;
        this.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", 0.5f);
        ControlParticles(false);
    }

    private void FlameHitTransition()
    {
        StartCoroutine(Burn());

        //gameObject.GetComponent<Collider>().enabled = false;

        ControlParticles(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillerObject")
        {
            ControlParticles(false);

            onFire = false;
        }
    }

    private void ControlParticles(bool isActive)
    {
        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            var temp = fireParticleSystems[i].emission;
            temp.enabled = isActive;
        }
    }

    private IEnumerator Burn()
    {
        //float t = 0;

        //while (myMaterial.color != B_color)
        //{
        //    t += Time.deltaTime / 4f;
        //    myMaterial.color = Color.Lerp(myMaterial.color, Color.black, t);

        //    yield return null;
        //}

        //yield return new WaitForSeconds(0.1f);
        //myMaterial.mainTexture = burntTexture;
        //t = 0;

        //while (myMaterial.color != W_color)
        //{
        //    t += Time.deltaTime / 4f;
        //    myMaterial.color = Color.Lerp(myMaterial.color, Color.white, t);

        //    yield return null;
        //}

        float t = 0;


        while (burntState > 0)
        {
            Debug.Log("hej");
            t += Time.deltaTime / 4f;

            this.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", Mathf.Lerp(0.5f, 0, t));
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(spawn.transform.position, 0.25f);

        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawWireSphere(spawn.transform.position, 0.25f);
    }
}
