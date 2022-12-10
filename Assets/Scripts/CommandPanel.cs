using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPanel : MonoBehaviour
{
    private Vector3 localPos = new Vector3(-0.5f, 0.25f, 2.5f);
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = localPos;
    }

    // Update is called once per frame

    void Update()
    {
        //gameObject.transform.localPosition = localPos;
    }

    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
    }

    void OnEnable()
    {
        gameObject.transform.localPosition = localPos;
        Debug.Log("PrintOnEnable: script was enabled");
    }
}
