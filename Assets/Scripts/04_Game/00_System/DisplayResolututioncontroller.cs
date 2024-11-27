using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResolututioncontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        DisplaySwitcher.instance.ApplySettingsToCameras();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
