using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// ステージ３のカメラ制御用 CameraInstall.csをベースに、二枚目の隠しコインがカメラに映らなかったので修正したものを作成
/// </summary>
public class Stage3Camera : MonoBehaviour
{
    [SerializeField, Tooltip("初期位置のカメラ")]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField, Tooltip("基本のカメラ")] private CinemachineVirtualCamera virtualCamera1;

    [SerializeField, Tooltip("隠しコイン用カメラ")] private CinemachineVirtualCamera virtualCamera2;

    private void OnTriggerStay2D(Collider2D other)
    {
        //特定の範囲に入ったらカメラ変更
        //初期位置カメラ
        if(other.gameObject.name == "Camera1")
        {
            virtualCamera.Priority = 1;
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 0;
        }

        //デフォルトのカメラ
        if (other.gameObject.name == "Camera2")
        {
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 1;
            virtualCamera2.Priority = 0;
        }

        //隠しコイン表示用カメラ
        if (other.gameObject.name == "Camera3")
        {
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //範囲外に出たらデフォルトのカメラに戻す
        if(other.gameObject.name == "Camera1" || other.gameObject.name == "Camera3")
        {
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 1;
            virtualCamera2.Priority = 0;
        }
    }
}
