using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluffballs : MonoBehaviour
{

    AudioSource audioSource;

    public float rms;
    public float[] bands;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("Mic").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rms = AudioAnalysis.ComputeRMS(audioSource);
        bands = AudioAnalysis.FreqBands(audioSource);
    }

    public float getRms()
    {
        return rms;
    }

    public float[] getBands()
    {
        return bands;
    }
}
