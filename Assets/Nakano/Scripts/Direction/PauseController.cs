using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TimeScale、ポーズ状態の制御
/// </summary>
public class PauseController : MonoBehaviour
{
    bool isPause = false; //ポーズ状態か

    public bool IsPause { get { return isPause; } }

    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    /// <summary>
    /// ポーズ状態の設定と解除
    /// </summary>
    /// <param name="playerPause">プレイヤーの操作を止める　trueで停止</param>
    /// <param name="scale">timeScaleの値</param>
    public void Pause(bool playerPause, int scale)
    {
        if(playerPause) playerController.IsPause = true;
        else playerController.IsPause = false;

        Time.timeScale = scale;
    }
}
