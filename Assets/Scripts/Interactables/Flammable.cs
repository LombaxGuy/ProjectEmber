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

        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            var temp = fireParticleSystems[i].emission;
            temp.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnPoint != spawn.transform.position)
        {
            spawnPoint = spawn.transform.position;
        }
    }

    private void Reset()
    {
        //gameObject.GetComponent<Collider>().enabled = true;
        onFire = false;

        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            var temp = fireParticleSystems[i].emission;
            temp.enabled = false;
        }
    }

    private void FlameHitTransition()
    {
        StartCoroutine(Burn());

        //gameObject.GetComponent<Collider>().enabled = false;

        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            var temp = fireParticleSystems[i].emission;
            temp.enabled = true;
        }

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


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(spawn.transform.position, 0.25f);

        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawWireSphere(spawn.transform.position, 0.25f);
    }
}
