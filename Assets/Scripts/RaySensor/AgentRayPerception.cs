using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class AgentRayPerception : Agent
{
    public Transform targetTransform;
    private int i = 0;
    bool targetRoom;

    public override void OnEpisodeBegin()
    {
        targetRoom = false;
        transform.localPosition = new Vector3(Random.Range(-8.5f, 8.5f), 0, Random.Range(4f, 7f));
        targetTransform.localPosition = new Vector3(Random.Range(-8.5f, 8.5f), 0, Random.Range(-4f, -7f));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveSpeed = 15f;
        float rotateSpeed = 150f;

        float move = actions.ContinuousActions[0];
        float rotate = actions.ContinuousActions[1];

        transform.Rotate(new Vector3(0, rotate * Time.fixedDeltaTime * rotateSpeed, 0));
        transform.localPosition += transform.forward * move * Time.fixedDeltaTime * moveSpeed;

        if(!targetRoom && transform.localPosition.z < -1)
        {
            AddReward(+0.5f); //Já passou as paredes pequenas
            targetRoom = true;
        }
        else if (targetRoom && transform.localPosition.z < 1)
        {
            AddReward(-1f); //Voltou para trás
            targetRoom = false;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> ca = actionsOut.ContinuousActions;
        ca[0] = Input.GetAxis("Vertical");
        ca[1] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            AddReward(+1f);
        }
        else if (other.gameObject.CompareTag("External Wall") || other.gameObject.CompareTag("Internal Wall"))
        {
            AddReward(-1f);
        }
        EndEpisode();
    }

    private void FixedUpdate() 
    {
        if(i++ % 5 == 0)
        {
            RequestDecision(); //Em vez de usar o DecisionRequester
        }
    }

}
