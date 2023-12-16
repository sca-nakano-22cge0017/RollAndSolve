using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    PlayerController playerController;

    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;
    public CinemachineVirtualCameraBase vcam3;
    public CinemachineVirtualCameraBase vcam4;
    public CinemachineVirtualCameraBase vcam5;

    [SerializeField]
    private bool _isSwitchOn;
    [SerializeField]
    private bool _isSwitchOff;

    float timer;
    float timer2;

    void Start()    
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
            if (playerController.playerstate == PlayerController.PlayerState.Circle&& playerController.Speed >= 7.0f&&timer > 0.5f)
            {
                vcam1.Priority = 0;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 1;
                vcam5.Priority = 0;
            }
            else if(timer > 0.5f)
            {
                vcam1.Priority = 1;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                vcam5.Priority = 0;
            }
            else
            {
                timer += Time.deltaTime;  //タイマー加算
            }

            if (playerController.Speed <= 6.9f)
            {
                vcam1.Priority = 1;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                vcam5.Priority = 0;
            }



        }
        if (Input.GetKey(KeyCode.D))
        {
            if (playerController.playerstate == PlayerController.PlayerState.Circle && playerController.Speed >= 7.0f&& timer2 > 0.5f)
            {
                vcam1.Priority = 0;
                vcam2.Priority = 0;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                vcam5.Priority = 1;
            }
            else if (timer2 > 0.5f)
            {
                vcam1.Priority = 0;
                vcam2.Priority = 1;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                vcam5.Priority = 0;
            }
            else
            {
                timer2 += Time.deltaTime;  //タイマー加算
              
            }
            if (playerController.Speed <= 6.9f)
            {
                vcam1.Priority = 0;
                vcam2.Priority = 2;
                vcam3.Priority = 0;
                vcam4.Priority = 0;
                vcam5.Priority = 0;
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
