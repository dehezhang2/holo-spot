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
    public class SelectMode : MonoBehaviour, OperationMode
    {
        public float publishMessageFrequency = 0.5f;
        public GameObject cursor;
        public GameObject anchorManager;

        private Vector3 position;
        public GameObject visualizePosition;
        // 0 for terminate, 1 for activate, 2 for activate but message is already sent
        private bool sendMsg = false;
        private bool activated = false;
        private bool position_selected = false;
        private bool isSelected = false;
        public string topicName = "hololens/select_pos_rot";
        public string status_topicName = "hololens/select_status";


        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {

            if (sendMsg)
            {
                if (activated)
                {
                    if (position_selected)
                    {
                        //activate => send item position
                        List<GameObject> anchorList = anchorManager.GetComponent<AzureSpatialAnchorsScript>().getAnchorList();
                        GameObject anchor = anchorList[0];
                        Vector3 sentPosition = anchor.transform.InverseTransformPoint(position);
                        sentPosition.Scale(anchor.transform.localScale);

                        PosRotMsg cursorPos = new PosRotMsg(
                                     anchorList[0].GetComponent<CloudNativeAnchor>().CloudAnchor.Identifier,
                                     sentPosition.z,
                                     -sentPosition.x,
                                     sentPosition.y,
                                     0f,
                                     0f,
                                     0f,
                                     1f
                                 );
                    }
                } else
                {
                    // send stop msg
                    IdMsg msg_stop = new IdMsg("stop");
                    ros.Publish(status_topicName, msg_stop);

                }
                sendMsg = false;
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
        public void SelectPos()
        {
            
            if (this.isSelected)
            {
                this.Terminate();
                this.position = cursor.transform.position;
                Destroy(visualizePosition);
                visualizePosition = GameObject.CreatePrimitive(PrimitiveType.Cube);
                visualizePosition.transform.position = position;
                visualizePosition.transform.localScale = Vector3.one * 0.1f;
                visualizePosition.GetComponent<Collider>().enabled = false;

                this.position_selected = true;
            }
           
        }

        public void DeletePos()
        {
            if (this.isSelected)
            {
                this.Terminate();
                Destroy(visualizePosition);
                this.position_selected = false;
            }
        }
        public bool isActivated()
        {
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
            DeletePos();
            this.isSelected = false;
        }

    }

}