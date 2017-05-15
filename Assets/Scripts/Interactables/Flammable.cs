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

    private Renderer thisRenderer;

    private float shaderCutThreshold = 0.5f;

    [SerializeField]
    private float burnTransitionTime = 2;

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
        thisRenderer = GetComponent<Renderer>();

        spawnPoint = spawn.transform.position;

        fireParticleSystems = GetComponentsInChildren<ParticleSystem>();

        ControlParticles(false);
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
        onFire = false;
        thisRenderer.material.SetFloat("_DissolveAmount", shaderCutThreshold);
        ControlParticles(false);
    }

    private void FlameHitTransition()
    {
        StartCoroutine(Burn());

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
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / burnTransitionTime;

            thisRenderer.material.SetFloat("_DissolveAmount", Mathf.Lerp(shaderCutThreshold, 0, t));

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(spawn.transform.position, 0.25f);

        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawWireSphere(spawn.transform.position, 0.25f);
    }
}
