using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// クリア時演出・シーン遷移
/// </summary>
public class Clear : MonoBehaviour
{
    //Animation
    [SerializeField] Image clearText;
    Animator textAnim;
    bool textAnimEnd = false;

    //「ステージクリア」の文字の演出が終了したかどうか
    public bool TextAnimEnd
    {
        set { textAnimEnd = value; }
    }

    [SerializeField] Image effectLeft;
    [SerializeField] Image effectRight;
    Animator effectLeftAnim;
    Animator effectRightAnim;

    //クリア判定
    bool isClear;
    TreasureController treasureController;

    float waitTime = 4.0f; //アニメーション終了からシーン遷移までの待ち時間

    //SE
    [SerializeField] AudioClip clearSE;
    AudioSource audioSource;
    bool isSound = true;

    PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        treasureController = GameObject.FindWithTag("Treasure").GetComponent<TreasureController>();

        audioSource = GetComponent<AudioSource>();
        textAnim = clearText.GetComponent<Animator>();
        effectLeftAnim = effectLeft.GetComponent<Animator>();
        effectRightAnim = effectRight.GetComponent<Animator>();

        clearText.enabled = false;
    }

    void Update()
    {
        //宝箱からクリア判定を貰う
        isClear = treasureController.IsClear;

        //クリアしたら
        if(isClear)
        {
            //一度だけSE再生
            if(isSound)
            {
                audioSource.PlayOneShot(clearSE);
                isSound = false;
            }
            
            //クリアテキスト表示
            clearText.enabled = true;
            textAnim.SetTrigger("TextMove");

            //プレイヤー操作不可に
            playerController.IsPause = true;
        }

        //「ステージクリア」の文字の演出が終わったら
        if(textAnimEnd)
        {
            //星がキラキラ光る演出を再生
            effectLeft.enabled = true;
            effectRight.enabled = true;
            effectLeftAnim.SetTrigger("Start");
            effectRightAnim.SetTrigger("Start");

            //シーン遷移
            StartCoroutine(ToSelect());
        }
    }

    /// <summary>
    /// ステージに応じたシーン遷移・ステージクリアについての情報保存
    /// </summary>
    IEnumerator ToSelect()
    {
        yield return new WaitForSeconds(waitTime);

        //プレイ中ステージの情報を初期化
        PlayerPrefs.SetString("PlayingStage", "Title");

        //シーン名によって処理分岐
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                DataSave(1);
                break;

            case "Stage2":
                DataSave(2);
                break;

            case "Stage3":
                DataSave(3);
                SceneManager.LoadScene("ClearScene"); //ゲームクリア画面へ
                break;
        }
    }

    /// <summary>
    /// ステージクリアについての情報保存
    /// </summary>
    /// <param name="num">ステージの番号</param>
    void DataSave(int num)
    {
        //初クリアかどうかを保存 boolの代わりにintを使用 0のときfalse 1のときtrue
        //Clear, FirstClear + ステージの番号で保存
        if (PlayerPrefs.GetInt("Clear" + num.ToString(), 0) == 0)
        {
            PlayerPrefs.SetInt("FirstClear" + num.ToString(), 1);
        }

        //各ステージクリアしたかどうかを保存
        PlayerPrefs.SetInt("Clear" + num.ToString(), 1);
        PlayerPrefs.Save();

        //ステージ1、2のとき、隠しコインが9枚揃っていたらボーナスステージへ飛ぶ
        //そうじゃなければステージ選択画面へ
        //ステージ3はゲームクリア画面を挟むので除外する
        if(num < 3)
        {
            if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
            {
                SceneManager.LoadScene("BonusStage");
            }
            else SceneManager.LoadScene("StageSelect");
        }
    }
}
