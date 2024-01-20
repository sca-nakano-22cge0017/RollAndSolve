using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInstall : MonoBehaviour
{
    public CinemachineVirtualCameraBase StartVcam;
    public CinemachineVirtualCameraBase StartVcam2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartVcam.Priority = 20;
            StartVcam2.Priority = 0;
        }
    }
}
