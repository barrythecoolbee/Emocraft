using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CarInit : MonoBehaviour
{
    PathCreator pathCreator;
    public GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        pathCreator = GetComponent<PathCreator>();

        car.transform.position = pathCreator.path.GetPointAtDistance(2);
        car.transform.position += Vector3.up*2;
        car.transform.rotation = pathCreator.path.GetRotationAtDistance(2);
        car.transform.Rotate(new Vector3(0, 0, 90));
    }


}
