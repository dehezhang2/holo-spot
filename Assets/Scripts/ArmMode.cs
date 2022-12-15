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

namespace Accessiblecontrol
{
    public class ArmMode : MonoBehaviour, OperationMode
    {
        public float publishMessageFrequency = 0.5f;
        public GameObject headTracker;
        public GameObject virtualRobot;
        public Vector3 robotOffset;
        public Quaternion robotOffsetRot;
        public Vector3 visualizationOffset;
        public GameObject ros_manager;

        public GameObject MainCamera;
        private bool activated = false;
        private bool isSelected = false;
        private bool isGrasping = false;
        private bool sendMsg = false;
        public string topicName = "hololens/arm_pos_rot";
        public string status_topicName = "hololens/arm_status";

        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {
            if (sendMsg)
            {
                if (activated)
                {
                    if (timeElapsed > publishMessageFrequency)
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
                } else
                {
                    TriggerRequest trigger = new TriggerRequest();
                    ros.SendServiceMessage<TriggerResponse>("/spot/stop", trigger, nothing);
                    if (isGrasping)
                    {
                        ros.SendServiceMessage<TriggerResponse>("/spot/gripper_close", trigger, nothing);
                        this.isGrasping = false;
                    }
                    
                    sendMsg = false;
                }
            }
            

        }


        public void Activate()
        {
            virtualRobot.transform.position = MainCamera.transform.TransformPoint(robotOffset);
            virtualRobot.transform.rotation = MainCamera.transform.rotation * this.robotOffsetRot;
            
            //virtualRobot.transform.rotation = MainCamera.transform.rotation;

            //visualizeHead = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //visualizeHead.transform.position = MainCamera.transform.TransformPoint(visualizationOffset);
            //visualizeHead.transform.rotation = MainCamera.transform.rotation;
            //visualizeHead.transform.localScale = Vector3.one * 0.1f;
            //visualizeHead.GetComponent<Collider>().enabled = false;

            this.activated = true;
            this.sendMsg = true;
        }
        public void Terminate()
        {
            this.activated = false;
            this.sendMsg = true; 
            this.robotOffset = MainCamera.transform.InverseTransformPoint(virtualRobot.transform.position);
            this.robotOffsetRot = Quaternion.Inverse(MainCamera.transform.rotation) * virtualRobot.transform.rotation;

            //Destroy(visualizeHead);
        }

        public bool isActivated(){
            return this.activated;
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
            if (this.isSelected && this.activated)
            {
                ROSConnection ros = ros_manager.GetComponent<RosPublisherScript>().ros;
                if (!isGrasping)
                {
                    //IdMsg msg = new IdMsg("open hand");
                    //ros.Publish(status_topicName, msg);
                    TriggerRequest trigger = new TriggerRequest();
                    ros.SendServiceMessage<TriggerResponse>("/spot/gripper_open", trigger, nothing);
                }
                else
                {
                    //IdMsg msg = new IdMsg("close hand");
                    //ros.Publish(status_topicName, msg);
                    TriggerRequest trigger = new TriggerRequest();
                    ros.SendServiceMessage<TriggerResponse>("/spot/gripper_close", trigger, nothing);
                }
                isGrasping = !isGrasping;
            }
           
        }

        public void RotateHand()
        {
            if (this.isSelected && this.activated)
            {
                ROSConnection ros = ros_manager.GetComponent<RosPublisherScript>().ros;
                IdMsg msg = new IdMsg("rotate hand");
                ros.Publish(status_topicName, msg);
            }
            
        }

        public void StopRotate()
        {
            if (this.isSelected && this.activated) {
                ROSConnection ros = ros_manager.GetComponent<RosPublisherScript>().ros;
                IdMsg msg = new IdMsg("stop rotate hand");
                ros.Publish(status_topicName, msg);
            }
            
        }

        private void nothing(TriggerResponse response)
        {

        }
    }

}