using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Block
{
    enum cubeSide { TOP, BOTTOM, LEFT, RIGHT, FRONT, BACK };

    static Vector2 grassSide_LBC = new Vector2(3f, 15f) / 16; //left bottom corner
    static Vector2 grassTop_LBC = new Vector2(2f, 7f) / 16;
    static Vector2 dirt_LBC = new Vector2(2f, 15f) / 16;
    static Vector2 stone_LBC = new Vector2(1f, 15f) / 16;
    static Vector2 snowSide_LBC = new Vector2(4f, 11f) / 16;
    static Vector2 snowTop_LBC = new Vector2(0f, 11f) / 16;
    static Vector2 water_LBC = new Vector2(3f, 11f) / 16;
    static Vector2 sand_LBC = new Vector2(2f, 14f) / 16;
    static Vector2 yellowflower_LBC = new Vector2(13f, 15f) / 16;
    static Vector2 redflower_LBC = new Vector2(12f, 15f) / 16;
    static Vector2 moss_LBC = new Vector2(4f, 13f) / 16;
    static Vector2 coal_LBC = new Vector2(2f, 13f) / 16;
    static Vector2 gold_LBC = new Vector2(0f, 13f) / 16;
    static Vector2 diamond_LBC = new Vector2(2f, 12f) / 16;
    static Vector2 lava_LBC = new Vector2(15f, 0f) / 16;
    static Vector2 fishTopYellow_LBC = new Vector2(7f, 2f) / 16;
    static Vector2 fishSideYellow_LBC = new Vector2(8f, 2f) / 16;
    static Vector2 fishTopRed_LBC = new Vector2(7f, 4f) / 16;
    static Vector2 fishSideRed_LBC = new Vector2(8f, 4f) / 16;
    static Vector2 fishTopGreen_LBC = new Vector2(7f, 3f) / 16;
    static Vector2 fishSideGreen_LBC = new Vector2(8f, 3f) / 16;

    int count = 0;

    Vector2[,] blockUVs =
    {
        { grassTop_LBC, grassTop_LBC + new Vector2(1f, 0f)/16, grassTop_LBC + new Vector2(0f, 1f)/16, grassTop_LBC + new Vector2(1f,1f)/16 },
        { grassSide_LBC, grassSide_LBC + new Vector2(1f, 0f)/16, grassSide_LBC + new Vector2(0f, 1f)/16, grassSide_LBC + new Vector2(1f,1f)/16 },

        { snowTop_LBC, snowTop_LBC + new Vector2(1f, 0f)/16, snowTop_LBC + new Vector2(0f, 1f)/16, snowTop_LBC + new Vector2(1f,1f)/16 },
        { snowSide_LBC, snowSide_LBC + new Vector2(1f, 0f)/16, snowSide_LBC + new Vector2(0f, 1f)/16, snowSide_LBC + new Vector2(1f,1f)/16 },

        { dirt_LBC, dirt_LBC + new Vector2(1f, 0f)/16, dirt_LBC + new Vector2(0f, 1f)/16, dirt_LBC + new Vector2(1f,1f)/16 },
        { stone_LBC, stone_LBC + new Vector2(1f, 0f)/16, stone_LBC + new Vector2(0f, 1f)/16, stone_LBC + new Vector2(1f,1f)/16 },
        { water_LBC, water_LBC + new Vector2(1f, 0f)/16, water_LBC + new Vector2(0f, 1f)/16, water_LBC + new Vector2(1f,1f)/16 },
        { sand_LBC, sand_LBC + new Vector2(1f, 0f)/16, sand_LBC + new Vector2(0f, 1f)/16, sand_LBC + new Vector2(1f,1f)/16 },
        { yellowflower_LBC, yellowflower_LBC + new Vector2(1f, 0f)/16, yellowflower_LBC + new Vector2(0f, 1f)/16, yellowflower_LBC + new Vector2(1f,1f)/16 },
        { redflower_LBC, redflower_LBC + new Vector2(1f, 0f)/16, redflower_LBC + new Vector2(0f, 1f)/16, redflower_LBC + new Vector2(1f,1f)/16 },
        { moss_LBC, moss_LBC + new Vector2(1f, 0f)/16, moss_LBC + new Vector2(0f, 1f)/16, moss_LBC + new Vector2(1f,1f)/16 },
        { coal_LBC, coal_LBC + new Vector2(1f, 0f)/16, coal_LBC + new Vector2(0f, 1f)/16, coal_LBC + new Vector2(1f,1f)/16 },
        { gold_LBC, gold_LBC + new Vector2(1f, 0f)/16, gold_LBC + new Vector2(0f, 1f)/16, gold_LBC + new Vector2(1f,1f)/16 },
        { diamond_LBC, diamond_LBC + new Vector2(1f, 0f)/16, diamond_LBC + new Vector2(0f, 1f)/16, diamond_LBC + new Vector2(1f,1f)/16 },
        { lava_LBC, lava_LBC + new Vector2(1f, 0f)/16, lava_LBC + new Vector2(0f, 1f)/16, lava_LBC + new Vector2(1f,1f)/16 },

        { fishTopYellow_LBC, fishTopYellow_LBC + new Vector2(1f, 0f)/16, fishTopYellow_LBC + new Vector2(0f, 1f)/16, fishTopYellow_LBC + new Vector2(1f,1f)/16 },
        { fishTopRed_LBC, fishTopRed_LBC + new Vector2(1f, 0f)/16, fishTopRed_LBC + new Vector2(0f, 1f)/16, fishTopRed_LBC + new Vector2(1f,1f)/16 },
        { fishTopGreen_LBC, fishTopGreen_LBC + new Vector2(1f, 0f)/16, fishTopGreen_LBC + new Vector2(0f, 1f)/16, fishTopGreen_LBC + new Vector2(1f,1f)/16 },
        { fishSideYellow_LBC, fishSideYellow_LBC + new Vector2(1f, 0f)/16, fishSideYellow_LBC + new Vector2(0f, 1f)/16, fishSideYellow_LBC + new Vector2(1f,1f)/16 },
        { fishSideRed_LBC, fishSideRed_LBC + new Vector2(1f, 0f)/16, fishSideRed_LBC + new Vector2(0f, 1f)/16, fishSideRed_LBC + new Vector2(1f,1f)/16 },
        { fishSideGreen_LBC, fishSideGreen_LBC + new Vector2(1f, 0f)/16, fishSideGreen_LBC + new Vector2(0f, 1f)/16, fishSideGreen_LBC + new Vector2(1f,1f)/16 },
    };

    public enum blockType { GRASS, SNOW, DIRT, STONE, WATER, SAND, FLOWERYELLOW, FLOWERRED, MOSS, COAL, GOLD, DIAMOND, LAVA, YELLOWFISH, REDFISH, GREENFISH, AIR };
    blockType bType;
    Chunk owner;
    public Vector3 pos;
    Material material;
    bool isSolid;

    public Block(blockType bType, Vector3 pos, Chunk owner, Material material)
    {
        
        this.owner = owner;
        this.pos = pos;
        this.material = material;

        SetType(bType);
    }

    void createQuad(cubeSide side)
    {
        Mesh mesh = new Mesh();

        bool drawFlower = false;
        float randomChance = UnityEngine.Random.Range(0.0f, 1.0f);
        if (randomChance < 0.7f)
        {
            drawFlower = false;
        }
        else
        {
            drawFlower = true;
        }
        if ((bType == blockType.FLOWERYELLOW || bType == blockType.FLOWERRED) && (side == cubeSide.BOTTOM || side == cubeSide.TOP || drawFlower == false))
        {
            return;
        }


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
            uv00 = blockUVs[4, 0]; 
            uv10 = blockUVs[4, 1];
            uv01 = blockUVs[4, 2];
            uv11 = blockUVs[4, 3];
        }
        else if (bType == blockType.GRASS && side != cubeSide.BOTTOM && side != cubeSide.TOP)
        {
            uv00 = blockUVs[(int)(bType + 1), 0];
            uv10 = blockUVs[(int)(bType + 1), 1];
            uv01 = blockUVs[(int)(bType + 1), 2];
            uv11 = blockUVs[(int)(bType + 1), 3];
        }
        else if (bType == blockType.SNOW && side == cubeSide.TOP)
        {
            uv00 = blockUVs[(int)bType +1, 0];
            uv10 = blockUVs[(int)bType +1, 1];
            uv01 = blockUVs[(int)bType +1, 2];
            uv11 = blockUVs[(int)bType +1, 3];
        }
        else if (bType == blockType.SNOW && side == cubeSide.BOTTOM)
        {
            uv00 = blockUVs[4, 0];
            uv10 = blockUVs[4, 1];
            uv01 = blockUVs[4, 2];
            uv11 = blockUVs[4, 3];
        }
        else if (bType == blockType.WATER)
        {
            uv00 = blockUVs[(int)(bType + 2), 0];
            uv10 = blockUVs[(int)(bType + 2), 1];
            uv01 = blockUVs[(int)(bType + 2), 2];
            uv11 = blockUVs[(int)(bType + 2), 3];
        }
   
        else if ((bType == blockType.FLOWERYELLOW || bType == blockType.FLOWERRED) && drawFlower == true)
        {
            uv00 = blockUVs[(int)(bType + 2), 0];
            uv10 = blockUVs[(int)(bType + 2), 1];
            uv01 = blockUVs[(int)(bType + 2), 2];
            uv11 = blockUVs[(int)(bType + 2), 3];
        }
        else if ((bType == blockType.YELLOWFISH || bType == blockType.REDFISH || bType == blockType.GREENFISH) && (side == cubeSide.BOTTOM || side == cubeSide.TOP))
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.50f)
            {
                uv00 = blockUVs[(int)(bType + 2), 0];
                uv10 = blockUVs[(int)(bType + 2), 2];
                uv01 = blockUVs[(int)(bType + 2), 1];
                uv11 = blockUVs[(int)(bType + 2), 3];
            }
            else
            {
                uv00 = blockUVs[(int)(bType + 2), 0];
                uv10 = blockUVs[(int)(bType + 2), 1];
                uv01 = blockUVs[(int)(bType + 2), 2];
                uv11 = blockUVs[(int)(bType + 2), 3];
            }

        }
        else if ((bType == blockType.YELLOWFISH || bType == blockType.REDFISH || bType == blockType.GREENFISH) && !(side == cubeSide.BOTTOM || side == cubeSide.TOP))
        {
            uv00 = blockUVs[(int)(bType + 5), 0];
            uv10 = blockUVs[(int)(bType + 5), 1];
            uv01 = blockUVs[(int)(bType + 5), 2];
            uv11 = blockUVs[(int)(bType + 5), 3];
        }
        else
        { 
            uv00 = blockUVs[(int)(bType + 2), 0];
            uv10 = blockUVs[(int)(bType + 2), 1];
            uv01 = blockUVs[(int)(bType + 2), 2];
            uv11 = blockUVs[(int)(bType + 2), 3];
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

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.position = this.pos;
        quad.transform.parent = owner.gochunk.transform;

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        mf.mesh = mesh;

    }
    
    int ConvertToLocalIndex(int i)
    {
        if(i == -1)
        {
            return World.chunkSize - 1;
        }
        if(i == World.chunkSize)
        {
            return 0;
        }
        else
        {
            return i;
        }
        
    }

    public bool HasSolidNeighbour(int x, int y, int z)
    {
        Block[,,] chunkData;

        if (x < 0 || x >= World.chunkSize || y < 0 || y >= World.chunkSize || z < 0 || z >= World.chunkSize)
        {
            Vector3 neighChunkPos = owner.gochunk.transform.position + new Vector3(
                (x - (int)pos.x) * World.chunkSize,
                (y - (int)pos.y) * World.chunkSize,
                (z - (int)pos.z) * World.chunkSize);

            string chunkName = World.CreateChunkNames(neighChunkPos);

            x = ConvertToLocalIndex(x);
            y = ConvertToLocalIndex(y);
            z = ConvertToLocalIndex(z);

            Chunk neighChunk;


            if (World.chunkDic.TryGetValue(chunkName, out neighChunk)) //Sai neighChunk
            {
                chunkData = neighChunk.chunkData;
            }
            else
            {
                return false;
            }
        }
        else
        {
            chunkData = owner.chunkData;
        }

        try
        {
            return chunkData[x, y, z].isSolid;
        }
        catch (System.IndexOutOfRangeException ex) //Por exemplo quando x, y ou z saem dos limites do chunk
        {
            Debug.Log(ex);
        }

        return false;
                
    }

    public void Draw()
    {

        if (bType == blockType.AIR) return; // AIR não desenha nada e não
  
        if (!HasSolidNeighbour((int)pos.x - 1, (int)pos.y, (int)pos.z)) //Se não houver um vizinho à sua direita, desenha essa face
        {
            createQuad(cubeSide.LEFT);
        }
        if (!HasSolidNeighbour((int)pos.x + 1, (int)pos.y, (int)pos.z)) 
        {
            createQuad(cubeSide.RIGHT);
        }
        if(!HasSolidNeighbour((int)pos.x, (int)pos.y - 1, (int)pos.z))
        {
            createQuad(cubeSide.BOTTOM);
        }
        if (!HasSolidNeighbour((int)pos.x, (int)pos.y + 1, (int)pos.z))
        {
            createQuad(cubeSide.TOP);
        }
        if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z - 1))
        {
            createQuad(cubeSide.BACK);
        }
        if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z + 1))
        {
            createQuad(cubeSide.FRONT);
        }

    }

    public void SetType(blockType bType)
    {
        this.bType = bType;

        if (bType == blockType.AIR || bType == blockType.FLOWERYELLOW || bType == blockType.FLOWERRED)
        {
            isSolid = false;
        }
        else isSolid = true;
    }
}
