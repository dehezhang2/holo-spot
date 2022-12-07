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
    public class FollowMode : MonoBehaviour, OperationMode
    {
        public float publishMessageFrequency = 0.5f;
        public GameObject cursor;
        public GameObject anchorManager;
        private bool isSelected = false;
        private bool sendMsg = false;
        private bool activated = false;
        public string topicName = "hololens/pos_rot";


        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {
            if (sendMsg)
            {
                if (activated)
                {
                    if (timeElapsed > publishMessageFrequency)
                    {
                        List<GameObject> anchorList = anchorManager.GetComponent<AzureSpatialAnchorsScript>().getAnchorList();
                        GameObject anchor = anchorList[0];
                        Vector3 sentPosition = anchor.transform.InverseTransformPoint(cursor.transform.position);
                        sentPosition.Scale(anchor.transform.localScale);

                        PosRotMsg cursorPos = new PosRotMsg(
                                     anchorList[0].GetComponent<CloudNativeAnchor>().CloudAnchor.Identifier,
                                     sentPosition.z,
                                     -sentPosition.x,
                                     sentPosition.y,
                                     0.5f,
                                     0f,
                                     0f,
                                     1f
                                 );
                        ros.Publish(topicName, cursorPos);

                        timeElapsed = 0f;
                    }
                }
                else
                {
                    TriggerRequest trigger = new TriggerRequest();
                    ros.SendServiceMessage<TriggerResponse>("/spot/stop", trigger, nothing);
                    sendMsg = false;
                }
            }
            

        }


        public void Activate()
        {
            this.sendMsg = true;
            this.activated = true;
        }
        public void Terminate()
        {
            this.sendMsg = true;
            this.activated = false;
        }

        public bool isActivated(){
            return this.activated;
        }

        //public bool isSelected()
        //{
        //    mode = ros_publisher.GetComponent<RosPublisherScript>().mode;
        //    var operationMode = mode.GetComponent(typeof(OperationMode)) as OperationMode;
        //    return Object.ReferenceEquals(this, operationMode);
        //    //return operationMode.GetType() == typeof(DefaultMode);
        //}

        public void selectMode()
        {
            this.isSelected = true;
        }
        public void deSelect()
        {
            this.isSelected = false;
        }
        private void nothing(TriggerResponse response)
        {

        }
    }

}