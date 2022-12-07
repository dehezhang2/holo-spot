using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{   
    public Quaternion localRotation;
    float degree = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        degree = degree + 1;
        var axis = new Vector3(1, 0, 0);
        //gameObject.transform.localRotation = Quaternion.AngleAxis(degree, axis);
        gameObject.transform.localRotation = Quaternion.Euler(degree, -90, -270);
        localRotation = gameObject.transform.localRotation;
    }
}
