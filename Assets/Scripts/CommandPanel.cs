using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPanel : MonoBehaviour
{
    private Vector3 localPos = new Vector3(1.0f, 1.0f, 1.0f);
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = localPos;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = localPos;
    }
}
