using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk 
{
    public Block[,,] chunkData; //Array 3D
    private Material material;
    public GameObject gochunk;

    public enum chunkStatus { DRAW, DONE };
    public chunkStatus status;

    private int snowHeight = Utils.maxHeight - 75;
    private int waterHeight = Utils.maxHeight - 90;

   // private GameObject chicken = Instantiate

    public ChickenFoodPawner chickenFoodSpawner;
    public ChickenSpawner chickenSpawner;
    public MonsterSpawner monsterSpawner;
    //public PompomSpawner pompomSpawner;


    public Chunk(Vector3 pos, Material material)
    {
        gochunk = new GameObject(World.CreateChunkNames(pos));
        gochunk.transform.position = pos;
        this.material = material;

        chickenFoodSpawner = gochunk.AddComponent<ChickenFoodPawner>();
        chickenFoodSpawner.SetChunk(this);

        chickenSpawner = gochunk.AddComponent<ChickenSpawner>();
        chickenSpawner.SetChunk(this);

        monsterSpawner = gochunk.AddComponent<MonsterSpawner>();
        monsterSpawner.SetChunk(this);

        //pompomSpawner = gochunk.AddComponent<PompomSpawner>();
        //pompomSpawner.SetChunk(this);

        BuildChunk();
    }

    void combineQuads()
    {
        //1- Combine all children meshes
        MeshFilter[] meshFilters = gochunk.GetComponentsInChildren<MeshFilter>();


        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;

        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2- Create a new mesh on the parent object
        MeshFilter mf = gochunk.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        //3- Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4- Create a renderer for the parent
        MeshRenderer renderer = gochunk.AddComponent<MeshRenderer>();
        renderer.material = material;

        //5- Delete all uncombined children
        foreach (Transform quad in gochunk.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }

    }

    void BuildChunk()
    {
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];

        //Agora é preciso criar o chunk em dois passos:
        //1º construir todos os blocos e adicionar ao array
        //2º ver que face(s) é preciso desenhar em cada um, de acordo com os vizinhos
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);

                    int worldX = (int)gochunk.transform.position.x + x;
                    int worldY = (int)gochunk.transform.position.y + y;
                    int worldZ = (int)gochunk.transform.position.z + z;

                    int h = Utils.GenerateHeight(worldX, worldZ);
                    int hS = Utils.GenerateStoneHeight(worldX, worldZ);
                    
                    if (worldY <= hS)
                    {
                        if(Utils.fBM3D(worldX, worldY, worldZ, 1, 0.5f) < 0.8f)
                        {
                            if(worldY >= snowHeight && worldY > h){
                                chunkData[x, y, z] = new Block(Block.blockType.SNOW, pos, this, material);
                                //pompomSpawner.AddSurfacePos(chunkData[x, y, z]);
                            }
                            else if (worldY == hS) //superfície
                            {
                                if (UnityEngine.Random.Range(0f, 1f) < 0.20f)
                                {
                                    chunkData[x, y, z] = new Block(Block.blockType.MOSS, pos, this, material);
                                }
                                else
                                {
                                    chunkData[x, y, z] = new Block(Block.blockType.STONE, pos, this, material);
                                }
                            }
                            else
                            {
                                if (worldY < hS && worldY > hS - 17)
                                {
                                    if (UnityEngine.Random.Range(0f, 1f) < 0.85f)
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.STONE, pos, this, material);
                                    }
                                    else
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.COAL, pos, this, material);

                                    }
                                }
                                else if (worldY <= hS - 17 && worldY > hS - 25)
                                {
                                    if (UnityEngine.Random.Range(0f, 1f) < 0.85f)
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.STONE, pos, this, material);
                                    }
                                    else
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.GOLD, pos, this, material);

                                    }
                                }
                                else if (worldY <= hS - 25 && worldY > hS - 45)
                                {
                                    if (UnityEngine.Random.Range(0f, 1f) < 0.9f)
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.STONE, pos, this, material);
                                    }
                                    else
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.DIAMOND, pos, this, material);

                                    }
                                }
                                else if (worldY <= hS - 45)
                                {
                                    chunkData[x, y, z] = new Block(Block.blockType.LAVA, pos, this, material);
                                }
                                else
                                {
                                    chunkData[x, y, z] = new Block(Block.blockType.STONE, pos, this, material);

                                }

                            }

                        }
                        else
                        {
                            chunkData[x, y, z] = new Block(Block.blockType.AIR, pos, this, material);
                        }
                    }
                    else if(worldY == h) //na superfície
                    {

                        if(worldY >= snowHeight)
                        {
                            chunkData[x, y, z] = new Block(Block.blockType.SNOW, pos, this, material);
                        }
                        else if(worldY <= waterHeight)
                        {
                            chunkData[x, y, z] = new Block(Block.blockType.SAND, pos, this, material);
                        }
                        else
                        {
                            chunkData[x, y, z] = new Block(Block.blockType.GRASS, pos, this, material); //Terra superfície

                            chickenFoodSpawner.AddSurfacePos(chunkData[x, y, z]);
                            chickenSpawner.AddSurfacePos(chunkData[x, y, z]);
                            monsterSpawner.AddSurfacePos(chunkData[x, y, z]);
                            //pompomSpawner.AddSurfacePos(chunkData[x, y, z]);
                        }
                        
                    }
                    else if(worldY < h)
                    {
                        chunkData[x, y, z] = new Block(Block.blockType.DIRT, pos, this, material);
                    }
                    else
                    {
                        if (worldY <= waterHeight)
                        {
                             if(worldY == h + 1)
                             {
                                 chunkData[x, y, z] = new Block(Block.blockType.SAND, pos, this, material);

                                chickenFoodSpawner.AddSurfacePos(chunkData[x, y, z]);
                                chickenSpawner.AddSurfacePos(chunkData[x, y, z]);
                                monsterSpawner.AddSurfacePos(chunkData[x, y, z]);
                                //pompomSpawner.AddSurfacePos(chunkData[x, y, z]);

                            }
                            else
                             {
                                 if (UnityEngine.Random.Range(0f, 1f) < 0.9f)
                                 {
                                    chunkData[x, y, z] = new Block(Block.blockType.WATER, pos, this, material);
                                }
                                else
                                {
                                    if (UnityEngine.Random.Range(0f, 1f) < 0.333f)
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.YELLOWFISH, pos, this, material);

                                    }
                                    else if (UnityEngine.Random.Range(0f, 1f) < 0.666f)
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.REDFISH, pos, this, material);

                                    }
                                    else
                                    {
                                        chunkData[x, y, z] = new Block(Block.blockType.GREENFISH, pos, this, material);

                                    }

                                }

                            }
                        }
                        else if(worldY == (h + 1) && worldY < snowHeight && worldY > waterHeight + 1)
                        {
                            if (UnityEngine.Random.Range(0f, 1f) < 0.07f && !(worldY == hS +1))
                            {
                                if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
                                {
                                    chunkData[x, y, z] = new Block(Block.blockType.FLOWERYELLOW, pos, this, material);
                                }
                                else
                                {
                                    chunkData[x, y, z] = new Block(Block.blockType.FLOWERRED, pos, this, material);
                                }
                            }
                            else
                            {
                                chunkData[x, y, z] = new Block(Block.blockType.AIR, pos, this, material);
                            }
                                
                        }
                        else
                        {
                            chunkData[x, y, z] = new Block(Block.blockType.AIR, pos, this, material);
                        }
                        
                    }
                     
                }
            }
        }

        status = chunkStatus.DRAW;
    }

    public void DrawChunk() { 

        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x,y,z].Draw();
                }
            }
        }

        status = chunkStatus.DONE;

        combineQuads();
        MeshCollider collider = gochunk.AddComponent<MeshCollider>();
        collider.sharedMesh = gochunk.GetComponent<MeshFilter>().mesh;
    }

}
