using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInstall : MonoBehaviour
{
    [SerializeField]
    [Tooltip("切り替え後のカメラ")]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] [Tooltip("切り替え後のカメラ")] private CinemachineVirtualCamera virtualCamera1;
  
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera2;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera3;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera4;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera5;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerStay(Collider other)
    {
        // 当たった相手に"Player"タグが付いていた場合
        if (other.gameObject.tag == "Player")
        {
            // 他のvirtualCameraよりも高い優先度にすることで切り替わる
            virtualCamera.Priority = 100;
            virtualCamera.Priority1 = 0;
            virtualCamera.Priority2 = 0;
            virtualCamera.Priority3 = 0;
            virtualCamera.Priority4 = 0;
            virtualCamera.Priority5 = 0;
            Debug.Log("入ってる");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // 当たった相手に"Player"タグが付いていた場合
        if (other.gameObject.tag == "Player")
        {
            // 元のpriorityに戻す
            virtualCamera.Priority1 = 1;
            virtualCamera.Priority2 = 0;
            virtualCamera.Priority3 = 0;
            virtualCamera.Priority4 = 0;
            virtualCamera.Priority5 = 0;
            virtualCamera.Priority = 0;
            Debug.Log("スタートから出た");
        }
    
    }

}