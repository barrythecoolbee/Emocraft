using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("First person controller full");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + new Vector3(0, 22.36f, -12.1f);
    }
}
