using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using UnityEngine.SocialPlatforms;

public class PoseHookReceiver : MonoBehaviour
{
    private OSCReceiver receiver;
    //MeshRenderer cube;
    int index1 = 15;
    int index2 = 16;
    int index3 = 13;
    int index4 = 14;
    bool raisedRightHand;
    bool raisedLeftHand;
    bool raisedRightElbow;
    bool raisedLeftElbow;
    Vector3 localScale = Vector3.one;
    public float speed = 5;
    Vector2 velocity;
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;
    [SerializeField]
    GroundCheck groundCheck;

    void Reset()
    {
        groundCheck = GetComponentInChildren<GroundCheck>();
        if (!groundCheck)
            groundCheck = GroundCheck.Create(transform);
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = 7001;
        receiver.Bind("/posehook/bea", MessageReceived);
        //rigidbody = GetComponent<Rigidbody>();
        //cube = GetComponentInChildren<MeshRenderer>();
    }

    protected void MessageReceived(OSCMessage message)
    {
        /*float x = message.Values[index * 4].FloatValue;
        float y = message.Values[index * 4 + 1].FloatValue;
        float z = message.Values[index * 4 + 2].FloatValue;*/
        float visibilityRight = message.Values[index1 * 4 + 3].FloatValue;

        raisedRightHand = (visibilityRight > 0.8);

        float visibilityLeft = message.Values[index2 * 4 + 3].FloatValue;

        raisedLeftHand = (visibilityLeft > 0.8);

        float visibilityElbowRight = message.Values[index3 * 4 + 3].FloatValue;

        raisedRightElbow = (visibilityElbowRight > 0.8);

        float visibilityElbowLeft = message.Values[index4 * 4 + 3].FloatValue;

        raisedLeftElbow = (visibilityElbowLeft > 0.8);
    }

    private void LateUpdate()
    {
        if (raisedLeftHand)
        {
            Debug.Log("ENTREI LEFT");
            //ocalScale = new Vector3(1, 5, 1);
            velocity.y = speed * Time.deltaTime;
            transform.Translate(0, 0, velocity.y);
        }
         else if (raisedLeftElbow)
        {

            velocity.x = -speed * Time.deltaTime;
            transform.Translate(velocity.x, 0, 0);


        }
        else if (raisedRightHand)
        {
            
            //localScale = Vector3.one;
            if (groundCheck.isGrounded)
            {
                rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
                Jumped?.Invoke();
            }
        }
        else if (raisedRightElbow)
        {

            Debug.Log("ENTREI RIGHT");
            velocity.x = speed * Time.deltaTime;
            transform.Translate(velocity.x, 0, 0);

        }

        //cube.transform.localScale = localScale;
    }

  
}
