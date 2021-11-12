using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    List<Vector3> possibleChickenPositions;
   // private GameObject Coins;// = (GameObject)Resources.Load("Prefabs/Coins");

    public GameObject chicken;// = (GameObject) Resources.Load("Prefabs/Platform");

    private GameObject nest;
    private GameObject chickenInstance;

    private Chunk chunk;

    public GameObject hennery;

    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        /*food = GameObject.Find("Food");
        var instace = Instantiate(Coins);
        instace.transform.parent = food.transform;*/

      //  Coins = (GameObject)Resources.Load("Prefabs/Coins");

        chicken = Resources.Load("Chicken") as GameObject;

        player = GameObject.Find("First person controller full");

    }

    //public ChickenFoodPawner(Chunk chunk)
    public void SetChunk(Chunk chunk)
    {
        this.chunk = chunk;
        possibleChickenPositions = new List<Vector3>();

        hennery = GameObject.Find("Hennery");

        /*instance = GameObject.Instantiate(Coins);
        instance.transform.parent = food.transform;*/

        nest = new GameObject();
        nest.name = "nest";

        nest.transform.parent = hennery.transform;


    }

   

    // Update is called once per frame
    void Update()
    {
 
        if (chunk.status == Chunk.chunkStatus.DONE)
        {
            if (nest.transform.childCount < (possibleChickenPositions.Count * 0.00075))
            {
                GenerateChickens();
            }
            ByeByeChicken();
        }
        
    }

    public void ByeByeChicken()
    {
        foreach (Transform child in nest.transform)
        {
            if(Vector3.Distance(child.position, player.transform.position) >= 100)
            {
                Destroy(child.gameObject);
            }
                
        }
    }

    public void AddSurfacePos(Block block)
    {
        possibleChickenPositions.Add(block.pos);
    }

    public void GenerateChickens()
    {
        for (int i = 0; i < possibleChickenPositions.Count; i++)
        {

            if (Random.Range(0f, 1f) < 0.0015f)
            {
                chickenInstance = Instantiate(chicken, possibleChickenPositions[i], Quaternion.identity);
                chickenInstance.transform.parent = nest.transform;
                chickenInstance.transform.localPosition = chunk.gochunk.transform.localPosition + possibleChickenPositions[i];
            }
        }
    }
}
