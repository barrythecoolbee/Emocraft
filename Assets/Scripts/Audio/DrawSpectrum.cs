using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpectrum : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        
    }

    void Update()
    {
        float[] samples = AudioAnalysis.GetSpectrum(audioSource);    

        for(int i = 1; i < samples.Length-1; i++)
        {
            // Debug.DrawLine(new Vector3(i - 1, samples[i - 1] * 30f, 0), new Vector3(i + 1, samples[i + 1] * 30f, 0), Color.cyan);
            Debug.DrawLine(new Vector3(i - 1, AudioAnalysis.ConvertToDB( samples[i - 1] * samples[i - 1]), 0), 
                new Vector3(i + 1, AudioAnalysis.ConvertToDB(samples[i + 1] * samples[i + 1]), 0), Color.cyan);
        }
    }
}
