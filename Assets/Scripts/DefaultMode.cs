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
    public class DefaultMode : MonoBehaviour, OperationMode
    {
        private bool activated = false;
        private bool isSelected = true;
        private bool sendMsg = false;
        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {
            if (sendMsg)
            {
                if (!activated)
                {
                    TriggerRequest trigger = new TriggerRequest();
                    ros.SendServiceMessage<TriggerResponse>("/spot/stop", trigger, nothing);
                    sendMsg = false;
                }
            }
            return;
        }
        public void Activate()
        {
            this.activated = true;
            return;
        }
        public void Terminate()
        {
            this.sendMsg = true;
            this.activated = false;

            return;
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