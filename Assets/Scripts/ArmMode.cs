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
        public GameObject ros_manager;

        public GameObject MainCamera;
        private bool sendPose = false;
        private bool isSelected = false;
        private bool isGrasping = false;
        public string topicName = "hololens/arm_pos_rot";
        public string status_topicName = "hololens/arm_status";

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

        }


        public void Activate()
        {
            virtualRobot.transform.position = MainCamera.transform.TransformPoint(robotOffset);
            virtualRobot.transform.rotation = MainCamera.transform.rotation;

            //visualizeHead = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //visualizeHead.transform.position = MainCamera.transform.TransformPoint(visualizationOffset);
            //visualizeHead.transform.rotation = MainCamera.transform.rotation;
            //visualizeHead.transform.localScale = Vector3.one * 0.1f;
            //visualizeHead.GetComponent<Collider>().enabled = false;

            this.sendPose = true;
        }
        public void Terminate()
        {
            this.sendPose = false;
            //Destroy(visualizeHead);
        }

        public bool isActivated(){
            return this.sendPose;
        }

        public void selectMode()
        {
            this.isSelected = true;
        }

        public void deSelect()
        {
            this.isSelected = false;
        }
        
        public void Grasp()
        {
            if (this.isSelected)
            {
                ROSConnection ros = ros_manager.GetComponent<RosPublisherScript>().ros;
                if (!isGrasping)
                {
                    IdMsg msg = new IdMsg("open hand");
                    ros.Publish(status_topicName, msg);
                }
                else
                {
                    IdMsg msg = new IdMsg("close hand");
                    ros.Publish(status_topicName, msg);
                }
                isGrasping = !isGrasping;
            }
           
        }

        public void ResetArm()
        {
            if (this.isSelected)
            {
                ROSConnection ros = ros_manager.GetComponent<RosPublisherScript>().ros;
                IdMsg msg = new IdMsg("reset");
                ros.Publish(status_topicName, msg);
            }
        }
    }

}