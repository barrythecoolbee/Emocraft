using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class Receiver : MonoBehaviour
{
    OSCReceiver receiver;


    void Start()
    {
        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = 7001;
        receiver.Bind("/oscControl/slider1", MessageReceiver);
        receiver.Bind("/oscControl/slider2", MessageReceiver2);
        receiver.Bind("/accxyz", ReceiveAcc); //oscHook
    }

    void ReceiveAcc(OSCMessage message)
    {
        float x = message.Values[0].FloatValue;
        float y = message.Values[1].FloatValue;
        float z = message.Values[2].FloatValue;
        Debug.Log("Acc = " + new Vector3(x, y, z));

        //Same thing
        /*Vector3 v;
        message.ToVector3(out v);
        Debug.Log(v);*/
    }

    void MessageReceiver(OSCMessage message)
    {
        Debug.Log("FADER1 = " + message.Values[0].FloatValue);
    }

    void MessageReceiver2(OSCMessage message)
    {
        Debug.Log("FADER2 = " + message.Values[0].FloatValue);
    }
}
