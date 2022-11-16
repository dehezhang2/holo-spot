using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using RosMessageTypes.BuiltinInterfaces;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System.Collections.Generic;

using Microsoft.MixedReality.Toolkit;
using UnityEngine.Events;
/// <summary>
///
/// </summary>
public class RosPublisherScript : MonoBehaviour
{
    ROSConnection ros;
    public string topicIntersectionName = "hololens/pos_rot";
    public string topicOriginName = "hololens/pos_rot_origin";
    public string topicDirectionName = "hololens/eye_ray_direction";
    public string topicAnchorId = "hololens/anchor_id";


    // The game objects
    public GameObject cursor;
    // Publish the cursor's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;
    // The spatial anchor
    public GameObject anchorManager;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
    private bool isGo = false;

    //Store original cursor color
    private Color originCursorColor;

    private void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PosRotMsg>(topicIntersectionName);
        ros.RegisterPublisher<PosRotMsg>(topicOriginName);
        ros.RegisterPublisher<PosRotMsg>(topicDirectionName);
        ros.RegisterPublisher<IdMsg>(topicAnchorId);

    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            cursor.transform.rotation = Random.rotation;
            if (isGo)
            {
                SendOriginalPos();

                SendCursorPos();
                SendEyeGazeDirection();
            }
            timeElapsed = 0;
        }
    }

    private void SendOriginalPos()
    {
        cursor.transform.rotation = Random.rotation;
        PosRotMsg cursorPos = new PosRotMsg(
                     "shit",
                     cursor.transform.position.x,
                     cursor.transform.position.y,
                     cursor.transform.position.z,
                     0f,
                     0f,
                     0f,
                     1f
                 );


        ros.Publish(topicOriginName, cursorPos);
    }

    private void SendCursorPos()
    {
        List<GameObject> anchorList = anchorManager.GetComponent<AzureSpatialAnchorsScript>().getAnchorList();
        List<string> idList = new List<string>();
        List<Vector3> positionList = new List<Vector3>();
        idList.Add(anchorList[0].GetComponent<CloudNativeAnchor>().CloudAnchor.Identifier);

        Vector3 sentPosition = anchorList[0].transform.InverseTransformPoint(cursor.transform.position);
        sentPosition.Scale(anchorList[0].transform.localScale);
        positionList.Add(sentPosition);
        cursor.transform.rotation = Random.rotation;
        PosRotMsg cursorPos = new PosRotMsg(
                     idList[0],
                     positionList[0].z,
                     -positionList[0].x,
                     positionList[0].y,
                     0f,
                     0f,
                     0f,
                     1f
                 );


        ros.Publish(topicIntersectionName, cursorPos);
    }

    private void SendEyeGazeDirection()
    {
        var direction = CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized;
        PosRotMsg eyeDirection = new PosRotMsg(
            "fuck",
             direction.x,
             direction.y,
             direction.z,
             0f,
             0f,
             0f,
             0f
         );
        ros.Publish(topicDirectionName, eyeDirection);
    }

    public void SendAnchorID(string id_string)
    {
        IdMsg msg_id = new IdMsg(id_string);
        ros.Publish(topicAnchorId, msg_id);
    }

    public void Activate()
    {

        originCursorColor = cursor.GetComponent<MeshRenderer>().material.color;
        cursor.GetComponent<MeshRenderer>().material.color = Color.green;
        this.isGo = true;
    }

    public void Terminate()
    {
        cursor.GetComponent<MeshRenderer>().material.color = originCursorColor;
        this.isGo = false;
    }
}