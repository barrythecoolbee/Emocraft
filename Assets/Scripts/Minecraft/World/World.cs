using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System;

public class World : MonoBehaviour
{
    public GameObject player;
    public Material material;
   // public static int columnHeight = 4;
    public static int chunkSize = 16; //16 chunks
    public static int radius = 3;

    bool drawing;

    public static ConcurrentDictionary<string, Chunk> chunkDic;
    Vector3 lastBuildPos;

    public static List<String> toRemove = new List<string>();

    public static string CreateChunkNames(Vector3 v) //O nome de cada Chunk é a sua localização
    {
        return (int)v.x + " " + (int)v.y + " " + (int)v.z;
    }

    IEnumerator BuildRecursiveWorld(Vector3 chunkPos, int rad)
    {

        int x = (int)chunkPos.x;
        int y = (int)chunkPos.y;
        int z = (int)chunkPos.z;

        BuildChunkAt(chunkPos);
        yield return null;

        if (--rad < 0) yield break;


        Building(new Vector3(x + chunkSize, y, z), rad);
        yield return null;
        Building(new Vector3(x - chunkSize, y, z), rad);
        yield return null;
        Building(new Vector3(x, y + chunkSize, z), rad);
        yield return null;
        Building(new Vector3(x, y - chunkSize, z), rad);
        yield return null;
        Building(new Vector3(x, y, z + chunkSize), rad);
        yield return null;
        Building(new Vector3(x, y, z - chunkSize), rad);

    }

    void Building(Vector3 chunkPos, int rad)
    {
        StartCoroutine(BuildRecursiveWorld(chunkPos, rad));
    }

    void BuildChunkAt(Vector3 chunkPos)
    {
        string name = CreateChunkNames(chunkPos);
        Chunk c;
        if (!chunkDic.TryGetValue(name, out c))
        {
            c = new Chunk(chunkPos, material);
            c.gochunk.transform.parent = this.transform;
            chunkDic.TryAdd(c.gochunk.name, c);
        }
    }

    IEnumerator RemoveChunks()
    {
        for(int i=0; i<toRemove.Count; i++)
        {
            string name = toRemove[i];
            Chunk c;
            if (chunkDic.TryGetValue(name, out c))
            {
                
                toRemove.Remove(name);
                chunkDic.TryRemove(name, out c); 
                Destroy(c.gochunk);
                yield return null;
            }
        }
    }

    IEnumerator DrawChunks()
    {
        drawing = true;
        foreach (KeyValuePair<string, Chunk> c in chunkDic)
        {
            if(c.Value.status == Chunk.chunkStatus.DRAW)
            {
                c.Value.DrawChunk();
                yield return null;
            }

            Vector3 chunkPos = c.Value.gochunk.transform.position;
            Vector3 playerChunkPos = PlayerPosChunk(player.transform.position);
            if(c.Value.gochunk != null && Vector3.Distance(GetChunkCenter(playerChunkPos), GetChunkCenter(chunkPos)) > chunkSize * radius * 2)
            {
                toRemove.Add(c.Key);
            }
            
        }
        StartCoroutine(RemoveChunks());
        drawing = false;
    }

    void Drawing()
    {
        StartCoroutine(DrawChunks());
    }

    Vector3 WhichChunk(Vector3 position)
    {
        Vector3 chunkPos = new Vector3();

        chunkPos.x = Mathf.Floor(position.x / chunkSize) * chunkSize; //Qual o chunk na posição do jogador
        chunkPos.y = Mathf.Floor(position.y / chunkSize) * chunkSize;
        chunkPos.z = Mathf.Floor(position.z / chunkSize) * chunkSize;

        return chunkPos;
    }

    Vector3 PlayerPosChunk(Vector3 playerPos)
    {
        Vector3 chunkPos = new Vector3();

        chunkPos.x = Mathf.Floor(playerPos.x / chunkSize) * chunkSize;
        chunkPos.y = Mathf.Floor(playerPos.y / chunkSize) * chunkSize;
        chunkPos.z = Mathf.Floor(playerPos.z / chunkSize) * chunkSize;

        return chunkPos;

    }

    Vector3 GetChunkCenter(Vector3 chunkPos)
    {
        return chunkPos + new Vector3(chunkSize / 2, chunkSize / 2, chunkSize / 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
        chunkDic = new ConcurrentDictionary<string, Chunk>();

        //Garantir que o World começa no 0,0,0
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;

        Vector3 playerPos = player.transform.position;
        player.transform.position = new Vector3 (playerPos.x, Utils.GenerateHeight(playerPos.x, playerPos.z) + 1, playerPos.z);
        //BuildChunkAt(WhichChunk(player.transform.position));

        lastBuildPos = player.transform.position;
        Building(WhichChunk(lastBuildPos), radius);
        Drawing();
        player.SetActive(true);
    }

    void Update()
    {
        Vector3 movement = player.transform.position - lastBuildPos;

        if (movement.magnitude > chunkSize/2)
        {
            StopAllCoroutines();
            lastBuildPos = player.transform.position;
            Building(WhichChunk(lastBuildPos), radius);
            Drawing();
        }
        if(!drawing) Drawing();
    }
}
