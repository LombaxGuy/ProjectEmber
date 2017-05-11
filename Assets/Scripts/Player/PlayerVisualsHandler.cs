using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour
{
    [Tooltip("The GameObject that has the ParticleSystem component used to make spark particles.")]
    [SerializeField]
    private GameObject sparkParticleGameObject;
    private ParticleSystem[] sparkParticles;

    [Tooltip("The GameObject that has the ParticleSystem component used to make fire particles.")]
    [SerializeField]
    private GameObject flameParticleGameObject;
    private ParticleSystem[] flameParticles;

    [Tooltip("The GameObject that has the ParticleSystem component used to make smoke particles.")]
    [SerializeField]
    private GameObject extinguishParticleGameObject;
    private ParticleSystem[] extinguishParticles;

    //[SerializeField]
    private float forceSparkThreshold = 3f;

    //[SerializeField]
    private float baseStart = 0.0f;

    //[SerializeField]
    private float amplitude = 1.0f;

    //[SerializeField]
    private float phase = 0.0f;

    //[SerializeField]
    private float frequency = 0.5f;

    // The number of spark particles emittet.
    private int sparkCount = 5;
    
    private Light flameLight;
    private Color originalColor;

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileRespawn += OnRespawn;
        EventManager.OnGameWorldReset += OnReset;
        EventManager.OnLevelLost += OnLost;
    }

    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileRespawn -= OnRespawn;
        EventManager.OnGameWorldReset -= OnReset;
        EventManager.OnLevelLost -= OnLost;
    }

    /// <summary>
    /// Called when the flame is extinguished.
    /// </summary>
    private void OnDeath()
    {
        // Plays the particles in the extinguishParticles ParticleSystems.
        for (int i = 0; i < extinguishParticles.Length; i++)
        {
            extinguishParticles[i].Play();
        }

        // Calls this method to turn off the fire particles.
        ControlFireParticles(false);
    }

    /// <summary>
    /// Called when the flame respawns.
    /// </summary>
    private void OnRespawn()
    {
        // Calls this method to turn on the fire particles.
        ControlFireParticles(true);
    }

    /// <summary>
    /// Called when the game world is reset.
    /// </summary>
    private void OnReset()
    {
        // Calls this method to turn on the fire particles.
        ControlFireParticles(true);
    }

    /// <summary>
    /// Called when the game is lost.
    /// </summary>
    private void OnLost()
    {
        // Calls this method to turn off the fire particles.
        ControlFireParticles(false);
    }

    private void Start()
    {
        // Gets the light component and sets the color of the light.
        flameLight = GetComponent<Light>();
        originalColor = flameLight.color;

        // Gets the ParticleSystems.
        sparkParticles = sparkParticleGameObject.GetComponentsInChildren<ParticleSystem>();
        flameParticles = flameParticleGameObject.GetComponentsInChildren<ParticleSystem>();
        extinguishParticles = extinguishParticleGameObject.GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // Makes the light flicker.
        flameLight.color = originalColor * (EvaluateLightWave());
    }

    /// <summary>
    /// Called when the flame collides with something.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        // If the force of the impact is greater than the threshold sparks are emitted.
        if (impactForce >= forceSparkThreshold)
        {
            for (int i = 0; i < sparkParticles.Length; i++)
            {
                // Sets the size of the sparks and emmits some particles.
                ParticleSystem.MinMaxCurve temp = sparkParticles[i].main.startSize;
                temp = Mathf.Clamp(impactForce, 0.3f, 0.6f);
                sparkParticles[i].Emit(sparkCount);
                temp = 0.3f;
            }
        }
    }

    /// <summary>
    /// Used to create a flickering ligth effect.
    /// </summary>
    /// <returns></returns>
    private float EvaluateLightWave()
    {
        float x = (Time.time + phase) * frequency;
        float y;
        x = x - Mathf.Floor(x);

        y = 1f - (Random.value * 2);

        return (y * amplitude) + baseStart;
    }

    /// <summary>
    /// Used to control the visibility fire and spark ParticleSystems and the light.
    /// </summary>
    /// <param name="isActive">Should the visibility be turned on or off.</param>
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
