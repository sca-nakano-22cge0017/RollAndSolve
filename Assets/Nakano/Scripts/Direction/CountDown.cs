using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カウントダウン
/// </summary>
public class CountDown : MonoBehaviour
{
    Animator anim;
    PlayerController player;

    PauseWindow pauseWindow;
    PauseController pauseController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();
        pauseWindow = GameObject.FindObjectOfType<PauseWindow>();
        pauseController = GameObject.FindObjectOfType<PauseController>();

        //プレイヤー操作不可, Animation再生の為にtimeScaleは1
        pauseController.Pause(true, 1);

        //カウントダウン
        anim.SetBool("Start", true);
    }

    /// <summary>
    /// カウントダウン終了
    /// </summary>
    public void CountEnd()
    {
        player.CountEnd = true; //カウントダウンが終了したことを伝える
        pauseWindow.GameStart = true;

        //ポーズ解除
        pauseController.Pause(false, 1);
    }
}
