using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour
{

    private Rigidbody rigid;

    [SerializeField]
    private ParticleSystem sparks;

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

    private ParticleSystem[] particles;
    
    private Light flameLight;
    private Color originalColor;

    private void OnEnable()
    {
        EventManager.OnGameWorldReset += OnReset;
        EventManager.OnLevelLost += OnLost;
    }

    private void OnDisable()
    {
        EventManager.OnGameWorldReset -= OnReset;
        EventManager.OnLevelLost -= OnLost;
    }

    private void OnReset()
    {
        ControlParticles(true);
    }

    private void OnLost()
    {
        ControlParticles(false);
    }

    // Use this for initialization
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        flameLight = GetComponent<Light>();
        originalColor = flameLight.color;

        particles = GetComponentsInChildren<ParticleSystem>();
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
            // Obsolete
            sparks.startSize = Mathf.Clamp(impactForce, 0.3f, 0.6f);
            sparks.Emit(5);
            sparks.startSize = 0.3f;
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

    private void ControlParticles(bool isActive)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            var temp = particles[i].emission;
            temp.enabled = isActive;
        }

        flameLight.enabled = isActive;
    }
}
