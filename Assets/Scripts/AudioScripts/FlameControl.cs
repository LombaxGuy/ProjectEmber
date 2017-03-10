using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FlameControl : MonoBehaviour {

    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private AudioMixerSnapshot[] snapshots;
    private float[] weights = new float[2];

    public void Start()
    {
        Debug.Log(weights[0].ToString() + " " + weights[1].ToString());
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
                mixer.TransitionToSnapshots(snapshots, weights, 2.0f);
                break;
            case 2:
                weights[0] = 0.75f;
                weights[1] = 0.25f;
                mixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
            case 3:
                weights[0] = 0.5f;
                weights[1] = 0.5f;
                mixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
            case 4:
                weights[0] = 0.25f;
                weights[1] = 0.75f;
                mixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
            case 5:
                weights[0] = 0.0f;
                weights[1] = 1.0f;
                mixer.TransitionToSnapshots(snapshots, weights, 0.2f);
                break;
        }
    }
}
