using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mic : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioClip;
    public bool useMic;
    public string deviceName;

    public ParticleSystem ps1;
    public ParticleSystem ps2;

    GameObject character;
    MeshRenderer mr;
    public Material[] materials;


    private void Awake()
    {
        ps1.Stop();
        ps2.Stop();
    }

    void Start()
    {
        character = GameObject.Find("Capsule mesh");
        mr = character.GetComponent<MeshRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (useMic)
        {
            if (Microphone.devices.Length > 0)
            {
                deviceName = Microphone.devices[0];
                audioSource.clip = Microphone.Start(deviceName, true, 10, AudioSettings.outputSampleRate);
                //print(AudioSettings.outputSampleRate);
                while (Microphone.GetPosition(deviceName) < 48 * 30) ; //fica preso de modo a haver um ligeiro atraso entre a gravação e o play
            }
            else
            {
                useMic = false;
            }
        }
        if (!useMic) //if em vez de else para se mudar a meio
        {
            audioSource.clip = audioClip;
        }
        audioSource.loop = true;
        audioSource.Play();

        //SetCubes();
        //StartCoroutine(Interact());
        InvokeRepeating("Interact", 0f, 0.5f);
    }

    void Update()
    {
        /*float energy = AudioAnalysis.MeanEnergy(audioSource);
        if(AudioAnalysis.ConvertToDB(energy) > 40)
        {
            print(AudioAnalysis.ComputeSpectrumPeak(audioSource, true));
        }*/
        //Interact();
    }

    void SetCubes()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.Rotate(new Vector3(0, 180f / 12f * i, 0));
            cube.transform.position += cube.transform.forward * 15;
            cube.name = "Band " + i;
            cube.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(i / 12f, 0.7f, 0.7f);
        }
    }

    void Interact()
    {

        float energy = AudioAnalysis.MeanEnergy(audioSource);
        //print(AudioAnalysis.ConvertToDB(energy));
        if (AudioAnalysis.ConvertToDB(energy) > 40)
        {
            float peakFrequency = AudioAnalysis.ComputeSpectrumPeak(audioSource, true);
            float concentration = AudioAnalysis.ConcentrationAroundPeak(peakFrequency);

            if (concentration > 0.8f)
            {
                ps1.Play();
                ps2.Play();
                //transform.Rotate(new Vector3(0, 180f / 12f, 0));
            }
            else
            {
                Debug.Log("PALMAS");

                mr.material = materials[(int)Random.Range(0f, materials.Length)];


            }

        }

    
    }
}
