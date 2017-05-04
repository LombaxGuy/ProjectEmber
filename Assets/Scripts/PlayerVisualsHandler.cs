using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour {

    Rigidbody rigid;

    [SerializeField]
    ParticleSystem sparks;

    [SerializeField]
    float forceSparkThreshhold = 3f;

    [SerializeField]
    float baseStart = 0.0f;

    [SerializeField]
    float amplitude = 1.0f;

    [SerializeField]
    float phase = 0.0f;

    [SerializeField]
    float frequency = 0.5f;


    Light flameLight;
    Color originalColor;

	// Use this for initialization
	void Start ()
    {
        rigid = GetComponent<Rigidbody>();

        flameLight = GetComponent<Light>();
        originalColor = flameLight.color;
	}
	
	// Update is called once per frame
	void Update ()
    {
        flameLight.color = originalColor * (EvaluateLightWave());
	}

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= forceSparkThreshhold)
        {
            Debug.Log("Spark!");

// Obsolete
            sparks.startSize = Mathf.Clamp(impactForce, 0.3f, 0.6f);
            sparks.Emit(5);
            sparks.startSize = 0.3f;
        }
    }

    float EvaluateLightWave()
    {
        float x = (Time.time + phase) * frequency;
        float y;
        x = x - Mathf.Floor(x);

        y = 1f - (Random.value * 2);

        return (y * amplitude) + baseStart;
    }

}
