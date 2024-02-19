using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Stage3Camera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("切り替え後のカメラ")]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] [Tooltip("切り替え後のカメラ")] private CinemachineVirtualCamera virtualCamera1;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // 当たった相手に"Player"タグが付いていた場合
        if (other.gameObject.tag == "Player")
        {
            // 他のvirtualCameraよりも高い優先度にすることで切り替わる
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 100;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 当たった相手に"Player"タグが付いていた場合
        if (other.gameObject.tag == "Player")
        {
            // 元のpriorityに戻す]
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 100;
            virtualCamera2.Priority = 0;
        }

    }
}
