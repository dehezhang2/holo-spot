using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMotionMonitor : MonoBehaviour
{
    public GameObject MainCamera;
    public Vector3 Offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 Offset = new Vector3(-0.34f, 0.1f, 0.857f);
        gameObject.transform.position = MainCamera.transform.position + Offset;
        gameObject.transform.rotation = MainCamera.transform.rotation;
    }
}
