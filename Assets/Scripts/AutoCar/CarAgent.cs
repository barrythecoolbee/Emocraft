using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class CarAgent : Agent
{
    // public GameObject platform;
    public GameObject CheckPoints;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private SimpleCarController characterController;
    new private Rigidbody rigidbody;

    private int numberOfCheckPoints;

  //  public GameObject platform;

    /// <summary>
    /// Called once when the agent is first initialized
    /// </summary>
    public override void Initialize()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        characterController = GetComponent<SimpleCarController>();
        rigidbody = GetComponent<Rigidbody>();

        

    }

   /* private void GeneratePlatforms(int number)
    {
        for (int i = 0; i < number; i++)
        {

            var instancia = Instantiate(platform, new Vector3(UnityEngine.Random.Range(-9, 9), 0.5f, UnityEngine.Random.Range(-9, 9)), Quaternion.identity);

            instancia.transform.parent = Coins.transform;

            // platforms.Add(platform);

            // platforms.Add()
        }
    }*/

    /// <summary>
    /// Called every time an episode begins. This is where we reset the challenge.
    /// </summary>
    public override void OnEpisodeBegin()
    {

        /*  int platformsNum = Coins.transform.childCount;

          if (platformsNum <= 1)
          {
              GeneratePlatforms(2);
          }*/

        transform.position = startPosition;
        transform.rotation = startRotation;
        rigidbody.velocity = Vector3.zero;

        numberOfCheckPoints = CheckPoints.transform.childCount;

        Debug.Log(CheckPoints.transform.childCount);

        foreach (Transform checkP in CheckPoints.transform)
        {
            checkP.gameObject.SetActive(true);
        }
       
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
        //bool jump = Input.GetKey(KeyCode.Space);

        // Convert the actions to Discrete choices (0, 1, 2)
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        //actions[2] = jump ? 1 : 0;
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
        //bool jump = actions.DiscreteActions[2] > 0;

        characterController.ForwardInput = vertical;
        characterController.TurnInput = horizontal;
        //   characterController.JumpInput = jump;

        AddReward(-1f / MaxStep);

    }

    /// <summary>
    /// Respond to entering a trigger collider
    /// </summary>
    /// <param name="other">The object (with trigger collider) that was touched</param>
    private void OnCollisionEnter(Collision other)
    {
       // Debug.Log(other.gameObject.tag);
        // If the other object is a collectible, reward and end episode
        
        if(other.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);

           // Destroy(other.gameObject);

            EndEpisode();
        }
       /* else if (other.tag == "FinishLine")
        {
            AddReward(1f);

            // Destroy(other.gameObject);

            EndEpisode();
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);

            // Destroy(other.gameObject);

            EndEpisode();
        }
        else if (other.gameObject.CompareTag("CheckP"))
        {
            AddReward(1f);



            // Destroy(other.gameObject);
            other.gameObject.SetActive(false);

            numberOfCheckPoints--;

            if (numberOfCheckPoints == 0)
            {
                EndEpisode();
            }


            //  EndEpisode();

        }
    }

}
