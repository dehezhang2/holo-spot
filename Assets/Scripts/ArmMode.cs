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

namespace Accessiblecontrol
{
    public class ArmMode : MonoBehaviour, OperationMode
    {
        public float publishMessageFrequency = 0.5f;
        public GameObject headTracker;
        public GameObject virtualRobot;
        public Vector3 relativePosition;

        private bool sendPose = false;
        public string topicName = "hololens/arm_pos_rot";

        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {

            if (sendPose && timeElapsed > publishMessageFrequency)
            {
                // ros.Publish(topicName, cursorPos);

                timeElapsed = 0f;
            }

        }


        public void Activate()
        {
            virtualRobot.transform.position = headTracker.transform.position + relativePosition;
            virtualRobot.transform.rotation = headTracker.transform.rotation;
            this.sendPose = true;
        }
        public void Terminate()
        {
            this.sendPose = false;
        }


    }

}