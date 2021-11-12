using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using UnityEngine.Video;
using UnityEngine.SocialPlatforms;

public class TouchOSCReceiver : MonoBehaviour
{
    private OSCReceiver receiver;
    private GameObject cube;
    private Vector3[] buffer = new Vector3[32];
    private int num = 0;
    public bool fader = true;
    private Vector3 localScale;


    void Start()
    {
        cube = GetComponentInChildren<MeshRenderer>().gameObject;

        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = 7001;
        receiver.Bind("/oscControl/slider1", CubeScaling);
        receiver.Bind("/accxyz", RecordAcc); //oscHook

        localScale = Vector3.one;
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

    protected void CubeScaling(OSCMessage message)
    {
        message.ToFloat(out localScale.y);
    }

    protected void RecordAcc(OSCMessage message)
    {
        Vector3 acc;
        message.ToVector3(out acc);
        buffer[num++] = acc;
        if (num == 32) num = 0;
    }

    Vector3 GetMeanAcc()
    {
        Vector3 sum = Vector3.zero;
        for(int i= 0; i< 32; i++)
        {
            sum += buffer[i];
        }
        return sum / 32;
    }

    enum Orientation { FLAT, UP, SIDE};
    Orientation ori;

    private void Update()
    {
        if (!fader)
        {
            Vector3 v = GetMeanAcc();

            float vx = Mathf.Abs(v.x);
            float vy = Mathf.Abs(v.y);
            float vz = Mathf.Abs(v.z);

            ori = Orientation.FLAT;

            if(vx > vy && vy > vz)
            {
                ori = Orientation.SIDE;
            }
            if(vy > vx && vy> vz)
            {
                ori = Orientation.UP;
            }

            Debug.Log(ori);

            switch (ori)
            {
                case Orientation.FLAT:
                    localScale = new Vector3(1, 0.1f, 1);
                    break;
                case Orientation.UP:
                    localScale = new Vector3(1, 1, 0.1f);
                    break;
                case Orientation.SIDE:
                    localScale = new Vector3(0.1f, 1f, 1f);
                    break;
            }
        }
        cube.transform.localScale = localScale;
    }

}
