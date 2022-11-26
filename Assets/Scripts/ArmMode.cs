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
        public Vector3 robotOffset;
        public Vector3 visualizationOffset;

        public GameObject MainCamera;
        public GameObject visualizeHead;
        private bool sendPose = false;
        public string topicName = "hololens/arm_pos_rot";

        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {

            if (sendPose && timeElapsed > publishMessageFrequency)
            {
                Vector3 sentPosition = headTracker.transform.localPosition;

                sentPosition.Scale(virtualRobot.transform.localScale);

                PosRotMsg headPos = new PosRotMsg(
                             "body",
                             sentPosition.z,
                             -sentPosition.x,
                             sentPosition.y,
                             -headTracker.transform.localRotation.z,
                             headTracker.transform.localRotation.x,
                             -headTracker.transform.localRotation.y,
                             headTracker.transform.localRotation.w
                         );
                ros.Publish(topicName, headPos);
                timeElapsed = 0f;
            }
            if (sendPose) {
                visualizeHead.transform.rotation = headTracker.transform.rotation;
            }

        }


        public void Activate()
        {
            virtualRobot.transform.position = MainCamera.transform.TransformPoint(robotOffset);
            virtualRobot.transform.rotation = MainCamera.transform.rotation;

            visualizeHead = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visualizeHead.transform.position = MainCamera.transform.TransformPoint(visualizationOffset);
            visualizeHead.transform.rotation = MainCamera.transform.rotation;
            visualizeHead.transform.localScale = Vector3.one * 0.1f;
            visualizeHead.GetComponent<Collider>().enabled = false;

            this.sendPose = true;
        }
        public void Terminate()
        {
            this.sendPose = false;
            Destroy(visualizeHead);
        }

        public bool isActivated(){
            return this.sendPose;
        }

    }

}