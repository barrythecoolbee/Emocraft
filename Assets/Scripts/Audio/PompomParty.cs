using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PompomParty : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
