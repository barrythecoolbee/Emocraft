using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToTarget : Agent
{
    public Transform targetTransform;
    public Material winMaterial;
    public Material loseMaterial;
    public MeshRenderer renderFloor;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-3.5f, 3.5f), 0, Random.Range(-3.5f, -0.5f));
        targetTransform.localPosition = new Vector3(Random.Range(-3.5f, 3.5f), 0, Random.Range(0.5f, 3.5f));

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 5f;

        transform.localPosition += new Vector3(moveX, 0f, moveZ) * Time.deltaTime * moveSpeed;

        // Debug.Log(actions.DiscreteActions[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target")){
            SetReward(+1f); //Recompensa positiva
            renderFloor.material = winMaterial;
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            SetReward(-1f); //Recompensa negativa
            renderFloor.material = loseMaterial;
        }

        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> ca = actionsOut.ContinuousActions;
        ca[0] = Input.GetAxis("Horizontal");
        ca[1] = Input.GetAxis("Vertical");
    }
}
