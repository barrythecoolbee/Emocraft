using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    Image target;
 
    void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;

        target = GameObject.Find("Target").GetComponent<Image>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;

            if(cam2.enabled == true)
            {
                target.enabled = false;
            }

            else if (cam1.enabled == true)
            {
                target.enabled = true;
            }
        }
    }
}
