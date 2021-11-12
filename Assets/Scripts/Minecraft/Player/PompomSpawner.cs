using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PompomSpawner : MonoBehaviour
{
    List<Vector3> possiblePompomPositions;
   // private GameObject Coins;// = (GameObject)Resources.Load("Prefabs/Coins");

    public GameObject pompom;// = (GameObject) Resources.Load("Prefabs/Platform");

    private GameObject fluff;
    private GameObject pompomInstance;

    private Chunk chunk;

    public GameObject fluffballs;

    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        /*food = GameObject.Find("Food");
        var instace = Instantiate(Coins);
        instace.transform.parent = food.transform;*/

      //  Coins = (GameObject)Resources.Load("Prefabs/Coins");

        pompom = Resources.Load("Pompom") as GameObject;

        player = GameObject.Find("First person controller full");

    }

    //public ChickenFoodPawner(Chunk chunk)
    public void SetChunk(Chunk chunk)
    {
        this.chunk = chunk;
        possiblePompomPositions = new List<Vector3>();

        fluffballs = GameObject.Find("Fluffballs");

        /*instance = GameObject.Instantiate(Coins);
        instance.transform.parent = food.transform;*/

        fluff = new GameObject();
        fluff.name = "fluff";

        fluff.transform.parent = fluffballs.transform;


    }

   

    // Update is called once per frame
    void Update()
    {
 
        if (chunk.status == Chunk.chunkStatus.DONE)
        {
            if (fluff.transform.childCount < (possiblePompomPositions.Count * 0.0015))
            {
                GenerateChickens();
            }
            ByeByeChicken();
        }
        
    }

    public void ByeByeChicken()
    {
        foreach (Transform child in fluff.transform)
        {
            if(Vector3.Distance(child.position, player.transform.position) >= 100)
            {
                Destroy(child.gameObject);
            }
                
        }
    }

    public void AddSurfacePos(Block block)
    {
        possiblePompomPositions.Add(block.pos);
    }

    public void GenerateChickens()
    {
        for (int i = 0; i < possiblePompomPositions.Count; i++)
        {

            if (Random.Range(0f, 1f) < 0.003f)
            {
                pompomInstance = Instantiate(pompom, possiblePompomPositions[i], Quaternion.identity);
                pompomInstance.transform.parent = fluff.transform;
                pompomInstance.transform.localPosition = chunk.gochunk.transform.localPosition + new Vector3(possiblePompomPositions[i].x, possiblePompomPositions[i].y + 3, possiblePompomPositions[i].z);
            }
        }
    }
}
