using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.Events;

public class EyeGazeCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.GetComponent<Transform>().position =
        //CoreServices.InputSystem.EyeGazeProvider.GazeOrigin +
        //CoreServices.InputSystem.EyeGazeProvider.GazeDirection.normalized * 0.5f;
        var pHit = CoreServices.InputSystem.EyeGazeProvider.HitPosition;
        var p = gameObject.transform.position;
        if ((pHit-p).magnitude>=gameObject.transform.localScale.x)
            gameObject.transform.position = CoreServices.InputSystem.EyeGazeProvider.HitPosition;
    }
}
