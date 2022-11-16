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
using Accessiblecontrol;

/// <summary>
///
/// </summary>
public class RosPublisherScript : MonoBehaviour
{
    ROSConnection ros;
    //public string topicIntersectionName = "hololens/pos_rot";
    //public string topicArmName = "hololens/arm_pos_rot";
    //// public string topicOriginName = "hololens/pos_rot_origin";
    //// public string topicDirectionName = "hololens/eye_ray_direction";
    public string topicAnchorId = "hololens/anchor_id";


    // The game objects
    public GameObject cursor;
    public GameObject mode;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed = 0f;
    //Store original cursor color
    private Color originCursorColor;

    private void Start()
    {
        originCursorColor = cursor.GetComponent<MeshRenderer>().material.color;
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        var followOperationMode = gameObject.transform.Find("FollowMode").gameObject.GetComponent(typeof(OperationMode)) as FollowMode;
        ros.RegisterPublisher<PosRotMsg>(followOperationMode.topicName);

        var armOperationMode = gameObject.transform.Find("ArmMode").gameObject.GetComponent(typeof(OperationMode)) as ArmMode;
        ros.RegisterPublisher<PosRotMsg>(armOperationMode.topicName);

        ros.RegisterPublisher<IdMsg>(topicAnchorId);

    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        operationMode.SendPose(ros, ref timeElapsed);
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
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        operationMode.Activate();
    }

    public void Terminate()
    {
        cursor.GetComponent<MeshRenderer>().material.color = originCursorColor;
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        operationMode.Terminate();
    }

    public void ChangeMode(string mode)
    {
        this.Terminate();
        this.mode = gameObject.transform.Find(mode).gameObject;
    }

}