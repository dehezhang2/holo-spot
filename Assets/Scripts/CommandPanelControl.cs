using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPanelControl : MonoBehaviour
{
    public GameObject helpPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void helpOn()
    {
        helpPanel.active = true;
    }
    public void helpOff()
    {
        helpPanel.active = false;
    }
}
