using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ポーズ画面の表示・非表示
/// </summary>
public class PauseWindow : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField, Header("初期選択ボタン")] Button StartButton;

    PauseController pauseController;

    bool isPause = false; //ポーズ画面のオンオフ

    bool gameStart = false;
    /// <summary>
    /// trueのときゲームスタート
    /// </summary>
    public bool GameStart { set { gameStart = value; } }

    private void Start()
    {
        pauseController = GameObject.FindObjectOfType<PauseController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseChange();
        }
    }

    public void PauseEnd()
    {
        PauseChange();
    }

    /// <summary>
    /// ポーズ状態の変更
    /// </summary>
    void PauseChange()
    {
        //ポーズ状態反転
        isPause = !isPause;

        //ウィンドウのアクティブ・非アクティブを反転
        menuPanel.SetActive(isPause);

        //ポーズ状態じゃなくなったとき
        if (!isPause)
        {
            //カウントダウンなどが終了してゲームが開始されている場合
            if (gameStart)
            {
                //プレイヤー操作可、timeScale = 1の状態にする
                pauseController.Pause(false, 1);
            }
            //カウントダウンなどがまだ終了していない場合
            else
            {
                //プレイヤー操作不可、timeScale = 1
                pauseController.Pause(true, 1);
            }
        }
        //ポーズ状態になったとき
        else
        {
            StartButton.Select(); //ボタンを選択しておく

            //プレイヤー操作不可、timeScale = 0
            pauseController.Pause(true, 0);
        }
    }
}
