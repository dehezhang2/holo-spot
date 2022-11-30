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
    public class FollowMode : MonoBehaviour, OperationMode
    {
        public float publishMessageFrequency = 0.5f;
        public GameObject cursor;
        public GameObject anchorManager;
        private bool isSelected = false;
        private bool sendPose = false;
        public string topicName = "hololens/pos_rot";


        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {

            if (sendPose && timeElapsed > publishMessageFrequency)
            {
                //List<GameObject> anchorList = anchorManager.GetComponent<AzureSpatialAnchorsScript>().getAnchorList();
                //List<string> idList = new List<string>();
                //List<Vector3> positionList = new List<Vector3>();
                //idList.Add(anchorList[0].GetComponent<CloudNativeAnchor>().CloudAnchor.Identifier);

                //Vector3 sentPosition = anchorList[0].transform.InverseTransformPoint(cursor.transform.position);
                //sentPosition.Scale(anchorList[0].transform.localScale);
                //positionList.Add(sentPosition);

                //PosRotMsg cursorPos = new PosRotMsg(
                //             idList[0],
                //             positionList[0].z,
                //             -positionList[0].x,
                //             positionList[0].y,
                //             0f,
                //             0f,
                //             0f,
                //             1f
                //         );
                //ros.Publish(topicName, cursorPos);
                List<GameObject> anchorList = anchorManager.GetComponent<AzureSpatialAnchorsScript>().getAnchorList();
                GameObject anchor = anchorList[0];
                Vector3 sentPosition = anchor.transform.InverseTransformPoint(cursor.transform.position);
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
                ros.Publish(topicName, cursorPos);

                timeElapsed = 0f;
            }

        }


        public void Activate()
        {
            this.sendPose = true;
        }
        public void Terminate()
        {
            this.sendPose = false;
        }

        public bool isActivated(){
            return this.sendPose;
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
    }

}