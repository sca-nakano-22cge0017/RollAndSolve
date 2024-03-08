using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BonusStageのアニメーション・画面遷移
/// </summary>
public class BonusClear : MonoBehaviour
{
    [SerializeField] Animator thank;
    [SerializeField] GameObject fade;

    PlayerController playerController;

    [SerializeField] float stopPos;

    bool isMove = false;

    PauseController pauseController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        pauseController = GameObject.FindObjectOfType<PauseController>();

        fade.SetActive(false);

        //隠しアイテム取得数を初期化
        PlayerPrefs.SetInt("SecretCoin", 0);
    }

    void Update()
    {
        //画面中央へ移動
        if(isMove)
        {
            if(playerController.gameObject.transform.position.x <= stopPos)
            {
                playerController.gameObject.transform.Translate(new Vector3(10, 0, 0) * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// ボーナスステージクリア
    /// ボーナスステージ最後のコインを獲得したら呼び出す
    /// </summary>
    public void Clear()
    {
        //プレイヤー操作不可 Animation再生の為にtimeScaleは1
        pauseController.Pause(true, 1);

        //クリア演出再生
        thank.SetBool("Clear", true);

        //画面中心へ移動
        isMove = true;

        StartCoroutine(SceneChange());
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(2);

        //フェードイン再生
        fade.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        //遷移
        SceneManager.LoadScene("Title");
    }
}
