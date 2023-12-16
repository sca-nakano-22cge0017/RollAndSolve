using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;
    public CinemachineVirtualCameraBase vcam3;

    [SerializeField]
    private bool _isSwitchOn;
    [SerializeField]
    private bool _isSwitchOff;

    float timer;
    float timer2;

    void Start()    
    {
        timer = 0;
        timer2 = 0;
        _isSwitchOn = true;
     _isSwitchOff = false;
    }
    void Update()
    {
        Debug.Log(timer);
        if (Input.GetKey(KeyCode.A))
        {
            if (timer > 0.5f)
            {  
                vcam1.Priority = 1;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
            }
            else
            {
                timer += Time.deltaTime;  //タイマー加算
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (timer2 > 0.5f)
            { 
                vcam1.Priority = 0;
                vcam2.Priority = 1;
                vcam3.Priority = 0;
                Debug.Log("タイム");
            }
            else
            {
                timer2 += Time.deltaTime;  //タイマー加算
              
            }
           
        }
          
        if (Input.GetKeyUp(KeyCode.A))
        {
            timer = 0.0f;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {

            timer2 = 0.0f;
        }
    }
}
