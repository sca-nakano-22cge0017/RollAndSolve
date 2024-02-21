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
    [SerializeField] Image clearText;
    Animator textAnim;

    [SerializeField] Image effectLeft;
    [SerializeField] Image effectRight;
    Animator effectLeftAnim;
    Animator effectRightAnim;

    bool isClear;
    TreasureController treasureController;

    bool textAnimEnd = false;

    [SerializeField] AudioClip clearSE;
    AudioSource audioSource;
    bool isSound = true;

    //「ステージクリア」の文字の演出が終了したかどうかを貰う
    public bool TextAnimEnd
    {
        set { textAnimEnd = value; }
    }

    void Start()
    {
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
            if(isSound)
            {
                audioSource.PlayOneShot(clearSE);
                isSound = false;
            }
            
            clearText.enabled = true;
            textAnim.SetTrigger("TextMove");
        }

        //「ステージクリア」の文字の演出が終わったら
        if(textAnimEnd)
        {
            //星がキラキラ光る演出を再生
            effectLeft.enabled = true;
            effectRight.enabled = true;
            effectLeftAnim.SetTrigger("Start");
            effectRightAnim.SetTrigger("Start");

            StartCoroutine(ToSelect());
        }

        //Debug用 削除必須
        //if(Input.GetKey(KeyCode.C)) { StartCoroutine(ToSelect()); }
    }

    //ステージに応じたシーン遷移・ステージクリアについての情報保存
    IEnumerator ToSelect()
    {
        yield return new WaitForSeconds(4f);
        PlayerPrefs.SetInt("PlayingStage", 0);

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
                SceneManager.LoadScene("ClearScene");
                break;
        }
    }

    /// <summary>
    /// ステージクリアについての情報保存
    /// </summary>
    /// <param name="num">ステージの番号</param>
    void DataSave(int num)
    {
        //初クリアかどうかを保存
        if (PlayerPrefs.GetInt("Clear" + num.ToString(), 0) == 0)
        {
            PlayerPrefs.SetInt("FirstClear" + num.ToString(), 1);
        }

        //各ステージクリアしたかどうかを保存 boolが入れられないのでintで代用
        PlayerPrefs.SetInt("Clear" + num.ToString(), 1);

        PlayerPrefs.Save();

        //ステージ１、２のとき、隠しコインが9枚揃っていたらボーナスステージへ飛ぶ
        //そうじゃなければステージ選択画面へ
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
