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
        public void SendPose(ROSConnection ros, ref float timeElapsed)
        {
            return;
        }
        public void Activate()
        {
            return;
        }
        public void Terminate()
        {
            return;
        }
        public bool isActivated(){
            return this.sendPose;
        }

    }
}