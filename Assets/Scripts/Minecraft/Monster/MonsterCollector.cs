using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;

public class MonsterCollector : Agent
{
    private Vector3 startPosition;
    private SimpleCharacterControllerMC characterController;
    new private Rigidbody rigidbody;
    public ParticleSystem dead;
    //private bool hasPlayer = false;

    private GameObject player;

    public override void Initialize()
    {
        startPosition = transform.position;
        characterController = GetComponent<SimpleCharacterControllerMC>();
        rigidbody = GetComponent<Rigidbody>();

        player = GameObject.Find("First person controller full");

        //Env = GameObject.Find("Player");

        // Reset agent position, rotation

        /*  transform.localPosition = new Vector3(Random.Range(-39, 39), 4, Random.Range(-39, 39));
          transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0.5f, 360f));
          rigidbody.velocity = Vector3.zero;*/

        //   platforms = Coins.transform.GetComponentsInChildren<Plataforma>;

        // var teste = Coins.GetComponentsInChildren<Plataforma>();

        // GeneratePlatforms(2);

        // GeneratePlayer();


    }

    /*private void GeneratePlayer()
    {
        //for (int i = 0; i < number; i++)
        //{
        //platforms[i].transform.localPosition = 

            var instancia = Instantiate(player, new Vector3(Random.Range(-15, 15), 5f, Random.Range(-15, 15)), Quaternion.identity);
            hasPlayer = true;

            

            instancia.transform.parent = Env.transform;
            instancia.transform.localPosition = new Vector3(Random.Range(-15, 15), 5f, Random.Range(-15, 15));

        // platforms.Add(platform);

        // platforms.Add()
        //}
    }*/

    /// <summary>
    /// Called every time an episode begins. This is where we reset the challenge.
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //if (hasPlayer == false) { GeneratePlayer(); }

       /* int platformsNum = Coins.transform.childCount;

        if (platformsNum <= 1)
        {    
            GeneratePlatforms(2);
        }*/
        // Reset agent position, rotation
          //transform.localPosition = new Vector3(Random.Range(-15, 15), 5, Random.Range(-15, 15));
          //transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0.5f, 360f));
          //rigidbody.velocity = Vector3.zero;


         

        // Reset platform position (5 meters away from the agent in a random direction)
        //platform.transform.position = startPosition + new Vector3(Random.Range(0f, 5f), 0.5f, Random.Range(0f, 5f));//Quaternion.Euler(Vector3.up * Random.Range(0.5f, 360f)) * Vector3.forward * 5f;
    }

    /// <summary>
    /// Controls the agent with human input
    /// </summary>
    /// <param name="actionsOut">The actions parsed from keyboard input</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Read input values and round them. GetAxisRaw works better in this case
        // because of the DecisionRequester, which only gets new decisions periodically.
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        bool jump = Input.GetKey(KeyCode.Space);

        // Convert the actions to Discrete choices (0, 1, 2)
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        actions[2] = jump ? 1 : 0;
    }

    /// <summary>
    /// React to actions coming from either the neural net or human input
    /// </summary>
    /// <param name="actions">The actions received</param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Punish and end episode if the agent strays too far
       /* if (Vector3.Distance(platform.transform.localPosition, transform.localPosition) > 10f)
        {
            AddReward(-1f);
            EndEpisode();
        }*/

        // Convert actions from Discrete (0, 1, 2) to expected input values (-1, 0, +1)
        // of the character controller
        float vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        float horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
        bool jump = actions.DiscreteActions[2] > 0;

        characterController.ForwardInput = vertical;
        characterController.TurnInput = horizontal;
        characterController.JumpInput = jump;

        if (jump)
        {
            AddReward(-0.5f / MaxStep);
        }

        AddReward(-1f / MaxStep);
    }

    /// <summary>
    /// Respond to entering a trigger collider
    /// </summary>
    /// <param name="other">The object (with trigger collider) that was touched</param>
    /*private void OnTriggerEnter(Collider other)
    {
        // If the other object is a collectible, reward and end episode
        if (other.tag == "Player")
        {
            AddReward(1f);

            //Destroy(other.transform.gameObject);
            //hasPlayer = false;

            EndEpisode();
            
        }
    }*/
}