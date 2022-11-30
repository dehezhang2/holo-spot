using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CompressedImage = RosMessageTypes.Sensor.CompressedImageMsg;
using Image = RosMessageTypes.Sensor.ImageMsg;
using Unity.Robotics.ROSTCPConnector;

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
    public float rotate;
    public Vector3 axis;
    void Start()
    {   
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<CompressedImage>("/spot/camera/hand_color/compressed", ShowImage);
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
        gameObject.transform.localRotation = localRotation;
        gameObject.transform.Rotate(axis.x, axis.y, axis.z, Space.Self);
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
        //gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", texture2D);
        isMessageReceived = false;
    }
}