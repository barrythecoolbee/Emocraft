using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ProceduralRoad : MonoBehaviour
{

    public int numSegments;
    public float distanceBetweenAnchors;
    public Transform car;
    RoadMesh road;

    public float smooth;
    float angleOffset;
    

    // Start is called before the first frame update
    void Start()
    {
        angleOffset = car.localEulerAngles.y * Mathf.Deg2Rad - PerlinOrientation(car.position);

        road = GetComponent<RoadMesh>();
        BuildRoadAhead(car.position - car.forward*4); //começar 4 metros atrás do carro
        car.position += Vector3.up;
    }

    void BuildRoadAhead(Vector3 startPos)
    {
        List<Vector3> anchors = new List<Vector3>();
        anchors.Add(startPos);

        for(int i = 0; i < numSegments; i++)
        {
            Vector3 newAnchor = NextAnchor(anchors[i]);
            anchors.Add(newAnchor);
        }

        VertexPath vertexPath = GeneratePath(anchors, 0.2f, false, PathSpace.xz); //0.2 é a distância entre as ancoras

        road.BuildRoad(vertexPath);
    }

    VertexPath GeneratePath(List<Vector3> points, float vertexSpacing, bool closedPath, PathSpace space)
    {
        BezierPath bezierPath = new BezierPath(points, closedPath, space);
        return new VertexPath(bezierPath, this.transform, vertexSpacing);
    }

    Vector3 NextAnchor(Vector3 anchor)
    {
        float angle = PerlinOrientation(anchor);
        angle += angleOffset;
        Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

        Vector2 anchorXZ = new Vector2(anchor.x, anchor.z);
        anchorXZ += dir * distanceBetweenAnchors;

        return new Vector3(anchorXZ[0], 0, anchorXZ[1]);
    }

    float PerlinOrientation(Vector3 pos)
    {
        float offset = 24000f;
        return 2f * Mathf.PI * Mathf.PerlinNoise((pos.x + offset) * smooth, (pos.z + offset) * smooth);
    }

}
