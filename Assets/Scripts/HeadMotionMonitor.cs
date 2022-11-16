using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMotionMonitor : MonoBehaviour
{
    public GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //var offset_in_world_frame = MainCamera.transform.TransformVector(Offset);
        //var position = MainCamera.transform.position + offset_in_world_frame;
        gameObject.transform.position = MainCamera.transform.position;
        gameObject.transform.rotation = MainCamera.transform.rotation;
    }
}
