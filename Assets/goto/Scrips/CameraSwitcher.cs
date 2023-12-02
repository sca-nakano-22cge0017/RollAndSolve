using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;

    [SerializeField]
    private bool _isSwitchOn;
    [SerializeField]
    private bool _isSwitchOff;

    void Start()    
    {
     _isSwitchOn = true;
     _isSwitchOff = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
    }
  
}