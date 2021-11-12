using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSphere : MonoBehaviour
{
    AudioSource audioSource;
    MeshRenderer mr;
    Mesh mesh;
    Vector3[] vertices, originalVertices;
    float randomColorR;
    float randomColorG;
    float randomColorB;
    float r;

    public Material[] materials;

    //Fluffballs fluffballs;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("First person controller full");
        audioSource = GameObject.Find("Mic").GetComponent<AudioSource>();
        //fluffballs = GameObject.Find("Fluffballs").GetComponent<Fluffballs>();
        mr = GetComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;

        mr.material = materials[(int)Random.Range(0f, materials.Length)];

        randomColorR= Random.Range(0f, 1f);
        randomColorG = Random.Range(0f, 1f);
        randomColorB = Random.Range(0f, 1f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameObject.transform.position.y < player.transform.position.y)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y + 1, gameObject.transform.position.z);
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }

        r = 15f;

        float rms = AudioAnalysis.ComputeRMS(audioSource);
        float[] bands = AudioAnalysis.FreqBands(audioSource);

        //float rms = fluffballs.getRms();
        //float[] bands = fluffballs.getBands();
        transform.localScale = new Vector3(4f * rms, 4f * rms, 4f * rms);

        mr.material.color = new Color(bands[0] / r, bands[2] / r, bands[3] / r, bands[5] / r);
        //mr.material.color = new Color(bands[0] / r + randomColorR, bands[2] / r + randomColorG, bands[3] / r + randomColorB, bands[5] / r);


        /*Grapher.Log(bands[0] / r, "0", Color.red);
        Grapher.Log(bands[1] / r, "1", Color.green);
        Grapher.Log(bands[2] / r, "2", Color.blue);
        Grapher.Log(bands[3] / r, "3", Color.yellow);*/

        transform.Rotate(new Vector3(0, bands[4], 0));
        vertices = mesh.vertices;
        for(int p=0; p < vertices.Length; p++)
        {
            Vector3 dir = originalVertices[p] - Vector3.zero;
            vertices[p] = originalVertices[p] + dir * bands[p % bands.Length] / r;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
    }
}
