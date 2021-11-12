using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
//using PathCreation.Examples;

public class DragonPathCreator : MonoBehaviour
{
    PathCreator pathCreator;
    GameObject go;
    public GameObject dragons;
    float speed = 2f;
    float dist = 0f;
    VertexPath pathEditor, myPath, path;
   // VertexPath path;

    Vector3[] anchors;

    RoadMesh roadMesh;

   /* void RandomAnchors()
    {
        anchors = new Vector3[5];

        for(int i = 0; i<5; i++)
        {
            anchors[i] = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.localScale = Vector3.one * 0.2f;
            g.transform.position = anchors[i];
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        pathCreator = GetComponent<PathCreator>();
        pathEditor = pathCreator.path;
                

       // roadMesh = GetComponent<RoadMesh>();

        Debug.Log(pathCreator.path.length); //Tamanho da estrada

       /* RandomAnchors();

        // myPath = GeneratePath(anchors, false);
        path = GeneratePath(anchors, false);
        roadMesh.BuildRoad(path);*/

         path = pathEditor;

        for (int i = 0; i < (int)dragons.transform.childCount / 2; i++)
        {
            go = dragons.transform.GetChild(i).GetComponentInChildren<MeshRenderer>().gameObject;
            Vector3 v = new Vector3(+i * 3, 0, -i * 3);
            go.transform.localPosition =v;

           // go.transform.rotation = Quaternion.Euler(0, 0, 90f);
        }
        for (int i = 1; i < (int)dragons.transform.childCount / 2 + 1; i++)
        {
            if (i == (int)dragons.transform.childCount / 2)
            {
                dragons.transform.GetChild(i - 1 + (int)dragons.transform.childCount / 2).GetComponentInChildren<MeshRenderer>().gameObject.SetActive(false);
            }
            go = dragons.transform.GetChild(i - 1 + (int)dragons.transform.childCount / 2).GetComponentInChildren<MeshRenderer>().gameObject;
            Vector3 v = new Vector3(-i * 3, 0, -i * 3);
            go.transform.localPosition =  v;

          //  go.transform.rotation = Quaternion.Euler(0, 0, 90f);
        }

    }

    // Update is called once per frame
    void Update()
    {

        dragons.transform.position = path.GetPointAtDistance(dist);
        dragons.transform.rotation = path.GetRotationAtDistance(dist) * Quaternion.Euler(0, 0, 90f);

        dist += speed * Time.deltaTime;


        /*  Debug.DrawRay(go.transform.position, path.GetDirectionAtDistance(dist), Color.cyan);
          Debug.DrawRay(go.transform.position, path.GetNormalAtDistance(dist), Color.yellow);*/

        /* if (Input.GetKeyDown(KeyCode.Space))
         {
             if(path == pathEditor){

                 path = myPath;
                 roadMesh.BuildRoad(path);
             }
             else
             {
                 path = pathEditor;
             }
         }*/
    }

   /* VertexPath GeneratePath(Vector3[] points, bool closedPath)
    {
        BezierPath bezierPath = new BezierPath(points, closedPath, PathSpace.xz);

        return new VertexPath(bezierPath, this.transform, 0.05f);
    }*/
}
