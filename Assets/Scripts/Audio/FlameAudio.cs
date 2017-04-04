using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FlameAudio : MonoBehaviour
{
    private WorldManager worldManager;
    private GameObject activeFlame;

    private AudioClip flameThrowSound;
    private AudioClip flameIdleSound;

    private AudioSource[] audioSources;

    private AudioMixer audioMixer;

    [SerializeField]
    private AudioMixerSnapshot[] snapshots;
    private float[] weights = new float[2]; 

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnProjectileLaunched;
        EventManager.OnProjectileUpdated += OnProjectileUpdated;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnProjectileLaunched;
        EventManager.OnProjectileUpdated -= OnProjectileUpdated;
    }

    // Use this for initialization
    private void Start()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();
        activeFlame = worldManager.ActiveFlame;

        flameThrowSound = Resources.Load<AudioClip>("Audio/Flame/flameThrow");
        flameIdleSound = Resources.Load<AudioClip>("Audio/Flame/flameIdle");

        audioMixer = Resources.Load<AudioMixer>("Audio/Mixers/flameMixer");

        audioSources = GetComponents<AudioSource>();

        // Makes sure that the AudioSources are ordered correctly
        if (audioSources[0].priority != 128)
        {
            AudioSource temp = audioSources[0];
            audioSources[0] = audioSources[1];
            audioSources[1] = temp;
        }

        audioSources[0].clip = flameIdleSound;
        audioSources[1].clip = flameThrowSound;
    }

    // Update is called once per frame
    private void Update()
    {
        if (activeFlame)
        {
            if (audioSources[0])
            {
                audioSources[0].panStereo = activeFlame.transform.position.normalized.x;
            }

            if (audioSources[1])
            {
                audioSources[1].panStereo = activeFlame.transform.position.normalized.x;
            }
        } 
    }

    private void OnProjectileLaunched(Vector3 dir, float forceStrength)
    {
        // Plays Flame Throw Sound.
        audioSources[1].Play();

        // Alters Flame sound after the Flame has been shot.
        RecieveForceStrength(0);
    }

    private void OnProjectileUpdated(Vector3 dir, float forceStrength)
    {
        // Alters Flame sound depending on potential forceStrength.
        RecieveForceStrength(forceStrength);
    }

    public void RecieveForceStrength(float forceStrength)
    {
        if (forceStrength <= 3.5f)
        {
            BlendSnapshot(1);
        }
        else if (forceStrength > 3.5f && forceStrength <= 5f)
        {
            BlendSnapshot(2);
        }
        else if (forceStrength > 5f && forceStrength <= 7.75f)
        {
            BlendSnapshot(3);
        }
        else if (forceStrength > 7.75f && forceStrength <= 9.5f)
        {
            BlendSnapshot(4);
        }
        else if (forceStrength > 9.5f && forceStrength <= 10f)
        {
            BlendSnapshot(5);
        }
    }

    private void BlendSnapshot(int forceStrengthIndent)
    {
        switch (forceStrengthIndent)
        {
            case 1:
                weights[0] = 1.0f;
                weights[1] = 0.0f;
                audioMixer.TransitionToSnapshots(snapshots, weights, 2.0f);
                break;
            case 2:
                weights[0] = 0.75f;
                weights[1] = 0.25f;
                audioMixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
            case 3:
                weights[0] = 0.5f;
                weights[1] = 0.5f;
                audioMixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
            case 4:
                weights[0] = 0.25f;
                weights[1] = 0.75f;
                audioMixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
            case 5:
                weights[0] = 0.0f;
                weights[1] = 1.0f;
                audioMixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
        }
    }
}
