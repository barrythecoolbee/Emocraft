using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class LightReceiver : MonoBehaviour
{
    private OSCReceiver receiver;
    MeshRenderer cubeRenderer;
    void Start()
    {
        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = 7001;
        receiver.Bind("/light", ReadLight);
        cubeRenderer = GetComponentInChildren<MeshRenderer>();
    }

    protected void ReadLight(OSCMessage message)
    {
        float light = message.Values[0].FloatValue;

        if(light > 200)
        {
            cubeRenderer.material.color = Color.yellow;
        } else
        {
            cubeRenderer.material.color = Color.black;
        }
    }
}
