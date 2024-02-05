using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BonusStageのアニメーション・画面遷移
/// </summary>
public class BonusClear : MonoBehaviour
{
    [SerializeField] Animator[] start;
    [SerializeField] Animator thank;

    void Start()
    {
        //隠しアイテム取得数を初期化
        PlayerPrefs.SetInt("SecretCoin", 0);

        Time.timeScale = 0;
        //PlayerのStateに操作を受け付けないモードを作って、それに変更する方が良いかも
        StartCoroutine(BonusStart());
    }

    void Update()
    {
        //BonusStageアニメーション終了後、プレイヤーを動かせるようにする
        if (start[0].GetCurrentAnimatorStateInfo(0).IsName("BonusStart"))
        {
            Time.timeScale = 1;
        }
    }

    IEnumerator BonusStart()
    {
        yield return new WaitForEndOfFrame();
        //AnimationのUpdateModeをUnscaledTimeに変えた状態で、
        //Start()で再生すると予期しない挙動になるのでワンクッション挟む

        start[0].SetBool("Start", true);
        start[1].SetBool("Start", true);
    }

    public void Clear()
    {
        Time.timeScale = 0;

        thank.SetBool("Clear", true);
        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        //ToDo 円形フェードアウト
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Title");
    }
}
