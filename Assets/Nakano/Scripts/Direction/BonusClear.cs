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
    [SerializeField] GameObject fade;

    PlayerController playerController;

    [SerializeField] float stopPos;

    bool isMove = false;
    bool isStart = false;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.IsPause = true;

        fade.SetActive(false);

        //隠しアイテム取得数を初期化
        PlayerPrefs.SetInt("SecretCoin", 0);

        Time.timeScale = 0;
        StartCoroutine(BonusStart());
    }

    void Update()
    {
        //BonusStageアニメーション終了後、プレイヤーを動かせるようにする
        if (start[0].GetCurrentAnimatorStateInfo(0).IsName("BonusStart") && !isStart)
        {
            Time.timeScale = 1;
            playerController.IsPause = false;
            isStart = true;
        }

        if(isMove)
        {
            if(playerController.gameObject.transform.position.x <= stopPos)
            {
                playerController.gameObject.transform.Translate(new Vector3(10, 0, 0) * Time.deltaTime);
            }
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
        playerController.IsPause = true;
        
        thank.SetBool("Clear", true);
        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        isMove = true;
        yield return new WaitForSeconds(2);
        fade.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Title");
    }
}
