using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPanel : MonoBehaviour
{
    private Vector3 localPos = new Vector3(0.0f, 0.5f, 1.2f);
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
