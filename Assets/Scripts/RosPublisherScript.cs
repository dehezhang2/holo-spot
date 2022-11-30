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

using RosMessageTypes.Std;
/// <summary>
///
/// </summary>
public class RosPublisherScript : MonoBehaviour
{
    public ROSConnection ros;
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
        ros.RegisterPublisher<IdMsg>(armOperationMode.status_topicName);

        var selectOperationMode = gameObject.transform.Find("SelectMode").gameObject.GetComponent(typeof(OperationMode)) as SelectMode;
        ros.RegisterPublisher<PosRotMsg>(selectOperationMode.topicName);
        ros.RegisterPublisher<IdMsg>(selectOperationMode.status_topicName);

        ros.RegisterPublisher<IdMsg>(topicAnchorId);

        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/sit");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/stand");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/power_on");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/power_off");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/claim");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/release");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/self_right");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/roll_over_left");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/roll_over_right");
        ros.RegisterRosService<TriggerRequest, TriggerResponse>("/spot/stop");
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
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if(!operationMode.isActivated()){
            originCursorColor = cursor.GetComponent<MeshRenderer>().material.color;
            cursor.GetComponent<MeshRenderer>().material.color = Color.green;
            operationMode.Activate();
        }
       
    }

    public void Terminate()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if(operationMode.isActivated()){
            cursor.GetComponent<MeshRenderer>().material.color = originCursorColor;
            operationMode.Terminate();
        }
        
    }

    public void ChangeMode(string mode)
    {
        this.Terminate();
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        operationMode.deSelect();
        this.mode = gameObject.transform.Find(mode).gameObject;
        operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        operationMode.selectMode();
    }

    public void Sit()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/sit", trigger, nothing);
        }
    }

    public void Stand()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/stand", trigger, nothing);
        }
    }

    public void Claim()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/claim", trigger, nothing);
        }
    }

    public void Release()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/release", trigger, nothing);
        }
    }

    public void Selfright()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/self_right", trigger, nothing);
        }
    }

    public void Poweron()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/power_on", trigger, nothing);
        }
    }

    public void Poweroff()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/power_off", trigger, nothing);
        }
    }

    public void Rolloverleft()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/roll_over_left", trigger, nothing);
        }
    }

    public void Rolloverright()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/roll_over_right", trigger, nothing);
        }
    }

    public void Stop()
    {
        var operationMode = this.mode.GetComponent(typeof(OperationMode)) as OperationMode;
        if (!operationMode.isActivated())
        {
            TriggerRequest trigger = new TriggerRequest();
            ros.SendServiceMessage<TriggerResponse>("/spot/stop", trigger, nothing);
        }
    }

    void nothing(TriggerResponse response) { 
    
    }
}