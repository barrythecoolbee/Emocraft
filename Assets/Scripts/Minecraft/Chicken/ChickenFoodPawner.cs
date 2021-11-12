using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFoodPawner : MonoBehaviour
{
    List<Vector3> possibleFoodPositions;
   // private GameObject Coins;// = (GameObject)Resources.Load("Prefabs/Coins");

    public GameObject platform;// = (GameObject) Resources.Load("Prefabs/Platform");

    private GameObject coins;
    private GameObject platformInstance;

    private Chunk chunk;

    public GameObject food;

    // Start is called before the first frame update
    void Start()
    {
        /*food = GameObject.Find("Food");
        var instace = Instantiate(Coins);
        instace.transform.parent = food.transform;*/

      //  Coins = (GameObject)Resources.Load("Prefabs/Coins");

        platform = Resources.Load("Platform") as GameObject;

    }

    //public ChickenFoodPawner(Chunk chunk)
    public void SetChunk(Chunk chunk)
    {
        this.chunk = chunk;
        possibleFoodPositions = new List<Vector3>();

        food = GameObject.Find("Food");

        /*instance = GameObject.Instantiate(Coins);
        instance.transform.parent = food.transform;*/

        coins = new GameObject();
        coins.name = "coins";

        coins.transform.parent = food.transform;


    }

   

    // Update is called once per frame
    void Update()
    {
 
        if (chunk.status == Chunk.chunkStatus.DONE)
        {
            if (coins.transform.childCount < (possibleFoodPositions.Count * 0.005))
            {
                GenerateFood();
            }
        }
    }

    public void AddSurfacePos(Block block)
    {
        possibleFoodPositions.Add(block.pos);
    }

    public void GenerateFood()
    {
        for (int i = 0; i < possibleFoodPositions.Count; i++)
        {

            if (Random.Range(0f, 1f) < 0.01f)
            {
                platformInstance = Instantiate(platform, possibleFoodPositions[i], Quaternion.identity);
                platformInstance.transform.parent = coins.transform;
                platformInstance.transform.localPosition = chunk.gochunk.transform.localPosition + possibleFoodPositions[i];
            }
        }
    }
}
