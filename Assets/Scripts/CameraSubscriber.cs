using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CompressedImage = RosMessageTypes.Sensor.CompressedImageMsg;
using Image = RosMessageTypes.Sensor.ImageMsg;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using System;

public class CameraSubscriber : MonoBehaviour
{
    ROSConnection ros;
    //public GameObject dome;
    private Texture2D texture2D;
    private byte[] imageData;
    private bool isMessageReceived;
    // Start is called before the first frame update
    private Vector3 localPos = new Vector3(0.22f, 0.1f, 1f);
    public Quaternion localRotation = Quaternion.Euler(90, 180, 0);
    private float currentImgAngle;
    public float handAngle;
    public bool isRotateMsgReceived;
    public bool activated = false;
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<CompressedImage>("/spot/camera/hand_color/compressed", ShowImage);
        ros.Subscribe<JointStateMsg>("/joint_states", ChangeImagePlaneAngle);
        texture2D = new Texture2D(640, 480);
        gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));

        gameObject.transform.localPosition = localPos;
        gameObject.transform.localRotation = localRotation;
    }

    private void Update()
    {
        if (isMessageReceived)
            ProcessImage();
            gameObject.transform.localPosition = localPos;
        if (isRotateMsgReceived)
        {
            gameObject.transform.localRotation = Quaternion.Euler(90 - handAngle, -90, -270);
            localRotation = gameObject.transform.localRotation;
            currentImgAngle = handAngle;
            isRotateMsgReceived = false;
        }
        else 
        { 
            gameObject.transform.localRotation = localRotation; 
        }

    }

    void ShowImage(CompressedImage ImgMsg)
    {
        imageData = ImgMsg.data;
        isMessageReceived = true;
        Debug.Log(ImgMsg.format);
    }

    void ProcessImage()
    {
        texture2D.LoadImage(imageData);
        texture2D.Apply();
        gameObject.GetComponent<Renderer>().material.mainTexture = texture2D;
        isMessageReceived = false;
    }

    void ChangeImagePlaneAngle(JointStateMsg jointState) {
        //handAngle = (float)(jointState.position[jointState.position.Length - 1]);
        handAngle = (float)((jointState.position[jointState.position.Length - 2]+1.57) * 180.0 / Math.PI);
        float angleToRotate = handAngle - currentImgAngle;
        if (angleToRotate > 3 || angleToRotate < -3) {
            isRotateMsgReceived = true;
        }
    }
}
