using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour
{
    //private Rigidbody rigid;

    [SerializeField]
    private GameObject sparkParticleGameObject;
    private ParticleSystem[] sparkParticles;

    [SerializeField]
    private GameObject flameParticleGameObject;
    private ParticleSystem[] flameParticles;

    [SerializeField]
    private GameObject extinguishParticleGameObject;
    private ParticleSystem[] extinguishParticles;

    [SerializeField]
    private float forceSparkThreshhold = 3f;

    [SerializeField]
    private float baseStart = 0.0f;

    [SerializeField]
    private float amplitude = 1.0f;

    [SerializeField]
    private float phase = 0.0f;

    [SerializeField]
    private float frequency = 0.5f;
    
    private Light flameLight;
    private Color originalColor;

    private void OnEnable()
    {
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileRespawn += OnRespawn;
        EventManager.OnGameWorldReset += OnReset;
        EventManager.OnLevelLost += OnLost;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileRespawn -= OnRespawn;
        EventManager.OnGameWorldReset -= OnReset;
        EventManager.OnLevelLost -= OnLost;
    }

    private void OnDeath()
    {
        for (int i = 0; i < extinguishParticles.Length; i++)
        {
            extinguishParticles[i].Play();
        }

        ControlFireParticles(false);
    }

    private void OnRespawn()
    {
        ControlFireParticles(true);
    }

    private void OnReset()
    {
        ControlFireParticles(true);
    }

    private void OnLost()
    {
        ControlFireParticles(false);
    }

    // Use this for initialization
    private void Start()
    {
        //rigid = GetComponent<Rigidbody>();

        flameLight = GetComponent<Light>();
        originalColor = flameLight.color;

        sparkParticles = sparkParticleGameObject.GetComponentsInChildren<ParticleSystem>();
        flameParticles = flameParticleGameObject.GetComponentsInChildren<ParticleSystem>();
        extinguishParticles = extinguishParticleGameObject.GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        flameLight.color = originalColor * (EvaluateLightWave());
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= forceSparkThreshhold)
        {
            for (int i = 0; i < sparkParticles.Length; i++)
            {
                ParticleSystem.MinMaxCurve temp = sparkParticles[i].main.startSize;
                temp = Mathf.Clamp(impactForce, 0.3f, 0.6f);
                sparkParticles[i].Emit(5);
                temp = 0.3f;
            }

            // Obsolete
            //sparks.startSize = Mathf.Clamp(impactForce, 0.3f, 0.6f);
            //sparks.Emit(5);
            //sparks.startSize = 0.3f;
        }
    }

    private float EvaluateLightWave()
    {
        float x = (Time.time + phase) * frequency;
        float y;
        x = x - Mathf.Floor(x);

        y = 1f - (Random.value * 2);

        return (y * amplitude) + baseStart;
    }

    private void ControlFireParticles(bool isActive)
    {
        for (int i = 0; i < sparkParticles.Length; i++)
        {
            var temp = sparkParticles[i].emission;
            temp.enabled = isActive;
        }

        for (int i = 0; i < flameParticles.Length; i++)
        {
            var temp = flameParticles[i].emission;
            temp.enabled = isActive;
        }

        flameLight.enabled = isActive;
    }
}
