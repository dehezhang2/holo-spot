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
    public class DefaultMode : MonoBehaviour, OperationMode
    {
        private bool sendPose = false;
        private bool isSelected = true;
        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {
            return;
        }
        public void Activate()
        {
            this.sendPose = true;
            return;
        }
        public void Terminate()
        {
            this.sendPose = false;
            return;
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