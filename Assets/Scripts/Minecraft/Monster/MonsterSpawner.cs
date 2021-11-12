using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    List<Vector3> possibleMonsterPositions;

    public GameObject monster1;
    public GameObject monster2;

    private GameObject hoard;
    private GameObject monsterInstance;

    private Chunk chunk;

    public GameObject party;

    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        monster1 = Resources.Load("Monster1") as GameObject;

        monster2 = Resources.Load("Monster2") as GameObject;

        player = GameObject.Find("First person controller full");

    }

    //public ChickenFoodPawner(Chunk chunk)
    public void SetChunk(Chunk chunk)
    {
        this.chunk = chunk;
        possibleMonsterPositions = new List<Vector3>();

        party = GameObject.Find("Party");

        /*instance = GameObject.Instantiate(Coins);
        instance.transform.parent = food.transform;*/

        hoard = new GameObject();
        hoard.name = "hoard";

        hoard.transform.parent = party.transform;


    }

   

    // Update is called once per frame
    void Update()
    {
 
        if (chunk.status == Chunk.chunkStatus.DONE)
        {
            if (hoard.transform.childCount < (possibleMonsterPositions.Count * 0.000001))
            {
                GenerateMonsters();
            }
            ByeByeMonster();
        }
        
    }

    public void ByeByeMonster()
    {
        foreach (Transform child in hoard.transform)
        {
            if(Vector3.Distance(child.position, player.transform.position) >= 100)
            {
                Destroy(child.gameObject);
            }
                
        }
    }

    public void AddSurfacePos(Block block)
    {
        possibleMonsterPositions.Add(block.pos);
    }

    public void GenerateMonsters()
    {
        for (int i = 0; i < possibleMonsterPositions.Count; i++)
        {

            if (Random.Range(0f, 1f) < 0.000001f)
            {
                GameObject monster;

                if (Random.Range(0f, 1f) < 0.5f)
                {
                    monster = monster1;
                } else
                {
                    monster = monster2;
                }
                 
                monsterInstance = Instantiate(monster, possibleMonsterPositions[i], Quaternion.identity);
                monsterInstance.transform.parent = hoard.transform;
                monsterInstance.transform.localPosition = chunk.gochunk.transform.localPosition + possibleMonsterPositions[i];
            }
        }
    }
}
