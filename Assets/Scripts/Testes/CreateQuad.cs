using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuad : MonoBehaviour
{
    public Material material;
    /*private Shader shaderTransparente;*/
    enum cubeSide { TOP, BOTTOM, LEFT, RIGHT, FRONT, BACK };

   // private static Vector2 yellowFlower = new Vector2(13f, 15f) / 16;
   // private static Vector2 redFlower = new Vector2(12f, 15f) / 16;
    //private static Vector2[] flowerList = new Vector2[] { yellowFlower, redFlower };
    

    static Vector2 grassSide_LBC = new Vector2(3f, 15f) / 16; //left bottom corner
    static Vector2 grassTop_LBC = new Vector2(2f, 6f) / 16;
    static Vector2 dirt_LBC = new Vector2(2f, 15f) / 16;
    static Vector2 stone_LBC = new Vector2(0f, 14f) / 16;
    static Vector2 fishTop_LBC = new Vector2(7f, 2f) / 16;
    static Vector2 fishSide_LBC = new Vector2(8f, 2f) / 16;
    /*static Vector2 flower_LBC = (Vector2)flowerList.GetValue((int)UnityEngine.Random.Range(0, flowerList.Length-1));
    static Vector2 empty_LBC = new Vector2(11f, 0f) / 16;*/

    Vector2[,] blockUVs =
    {
        { grassTop_LBC, grassTop_LBC + new Vector2(1f, 0f)/16, grassTop_LBC + new Vector2(0f, 1f)/16, grassTop_LBC + new Vector2(1f,1f)/16 },
        { grassSide_LBC, grassSide_LBC + new Vector2(1f, 0f)/16, grassSide_LBC + new Vector2(0f, 1f)/16, grassSide_LBC + new Vector2(1f,1f)/16 },
        { dirt_LBC, dirt_LBC + new Vector2(1f, 0f)/16, dirt_LBC + new Vector2(0f, 1f)/16, dirt_LBC + new Vector2(1f,1f)/16 },
        { stone_LBC, stone_LBC + new Vector2(1f, 0f)/16, stone_LBC + new Vector2(0f, 1f)/16, stone_LBC + new Vector2(1f,1f)/16 },
        { fishTop_LBC, fishTop_LBC + new Vector2(1f, 0f)/16, fishTop_LBC + new Vector2(0f, 1f)/16, fishTop_LBC + new Vector2(1f,1f)/16 },
        { fishSide_LBC, fishSide_LBC + new Vector2(1f, 0f)/16, fishSide_LBC + new Vector2(0f, 1f)/16, fishSide_LBC + new Vector2(1f,1f)/16 },
       // { flower_LBC, flower_LBC + new Vector2(1f, 0f)/16, flower_LBC + new Vector2(0f, 1f)/16, flower_LBC + new Vector2(1f,1f)/16 },
       // { empty_LBC, empty_LBC + new Vector2(1f, 0f)/16, empty_LBC + new Vector2(0f, 1f)/16, empty_LBC + new Vector2(1f,1f)/16 },
    };

    public enum blockType { GRASS, DIRT, STONE, FISH };
    public blockType bType;

    void Quad(cubeSide side)
    {
        Mesh mesh = new Mesh();

        Vector3 v0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v7 = new Vector3(-0.5f, 0.5f, -0.5f);

        Vector3 n = new Vector3(0, 0, 1);

        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv10 = new Vector2(1, 0);
        Vector2 uv11 = new Vector2(1, 1);

        if(bType == blockType.GRASS && side == cubeSide.TOP)
        {
            uv00 = blockUVs[0, 0]; //É o bloco uv 0
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }else if (bType == blockType.GRASS && side == cubeSide.BOTTOM)
        {
            uv00 = blockUVs[2, 0]; 
            uv10 = blockUVs[2, 1];
            uv01 = blockUVs[2, 2];
            uv11 = blockUVs[2, 3];
        }
        /* else if (bType == blockType.FLOWER && (side == cubeSide.BOTTOM || side == cubeSide.TOP || side == cubeSide.BACK || side == cubeSide.LEFT))
         {
             uv00 = blockUVs[5, 0];
             uv10 = blockUVs[5, 1];
             uv01 = blockUVs[5, 2];
             uv11 = blockUVs[5, 3];
         }*/
        /* else if(bType == blockType.FLOWER)
         {

         }*/
        else if (bType == blockType.FISH && (side == cubeSide.BOTTOM || side == cubeSide.TOP))
        {
            uv00 = blockUVs[(int)(bType + 1), 0];
            uv10 = blockUVs[(int)(bType + 1), 2];
            uv01 = blockUVs[(int)(bType + 1), 1];
            uv11 = blockUVs[(int)(bType + 1), 3];
           /* uv00 = blockUVs[(int)(bType + 1), 0];
            uv10 = blockUVs[(int)(bType + 1), 1];
            uv01 = blockUVs[(int)(bType + 1), 2];
            uv11 = blockUVs[(int)(bType + 1), 3];*/
        }
        else if (bType == blockType.FISH && !(side == cubeSide.BOTTOM || side == cubeSide.TOP))
        {
            uv00 = blockUVs[(int)(bType + 2), 0];
            uv10 = blockUVs[(int)(bType + 2), 1];
            uv01 = blockUVs[(int)(bType + 2), 2];
            uv11 = blockUVs[(int)(bType + 2), 3];
        }
        else
        { //Como o Grass ocupa duas posições, o resto vai para a frente 1
            uv00 = blockUVs[(int)(bType+1), 0];
            uv10 = blockUVs[(int)(bType + 1), 1];
            uv01 = blockUVs[(int)(bType + 1), 2];
            uv11 = blockUVs[(int)(bType + 1), 3];
        }

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        int[] triangles = new int[6];
        Vector2[] uv = new Vector2[4];

        switch (side)
        {
            case (cubeSide.TOP):
                vertices = new Vector3[] { v7, v6, v5, v4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                
                break;

            case (cubeSide.BOTTOM):
                vertices = new Vector3[] { v0, v1, v2, v3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;

            case (cubeSide.LEFT):
                vertices = new Vector3[] { v7, v4, v0, v3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;

            case (cubeSide.RIGHT):
                vertices = new Vector3[] { v5, v6, v2, v1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;

            case (cubeSide.FRONT):
                vertices = new Vector3[] { v4, v5, v1, v0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;

            case (cubeSide.BACK):
                vertices = new Vector3[] { v6, v7, v3, v2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;
        }

        triangles = new int[] { 3, 1, 0, 3, 2, 1 };
        uv = new Vector2[] { uv11, uv01, uv00, uv10 };


        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uv;

        GameObject quad = new GameObject("quad");
        quad.transform.parent = this.gameObject.transform;

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        mf.mesh = mesh;

    }

    void createCube()
    {
        foreach (cubeSide side in Enum.GetValues(typeof(cubeSide)))
        {
            Quad(side);
        }
        combineQuads();
    }

    void combineQuads()
    {
        //1- Combine all children meshes
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;

        while(i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2- Create a new mesh on the parent object
        MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        //3- Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4- Create a renderer for the parent
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material = material;

        /*if (bType == blockType.FLOWER)
        {
            renderer.material.shader = shaderTransparente;
        }*/
             

        //5- Delete all uncombined children
        foreach(Transform quad in this.transform)
        {
            Destroy(quad.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
      //  shaderTransparente = Shader.Find("Sprites/Default");

        createCube();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
