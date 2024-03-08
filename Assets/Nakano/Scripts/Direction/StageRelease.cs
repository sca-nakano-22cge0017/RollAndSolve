using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class SecretCoins
{
    public Image[] coin;
}

/// <summary>
/// ステージ解放演出、獲得隠しコイン収集率
/// </summary>
public class StageRelease : MonoBehaviour
{
    //解放演出
    [SerializeField] Animator[] release;
    [SerializeField] Animator[] bg;

    //隠しコイン収集率
    [SerializeField] SecretCoins[] stage; //各ステージ3枚ずつ設定　二次元配列にしている
    [SerializeField, Tooltip("未獲得状態・獲得済み状態の順にSpriteを設定")] Sprite[] coinImages;

    int stageAmount = 3; //ステージの数
    int coinAmount = 3; //各ステージにある隠しコインの数

    private void Awake()
    {
        Time.timeScale = 1;
        FirstClearCheck();

        //各ステージの隠しコイン収集率を表示
        for (int i = 0; i < stageAmount; i++)
        {
            for(int j = 0; j < coinAmount; j++)
            {
                //データ取得
                var num = PlayerPrefs.GetInt("SecretCoin:Stage" + (i + 1).ToString() + "-SecretCoin" + (j + 1).ToString(), 0);

                stage[i].coin[j].sprite = coinImages[num];
            }
        }
    }

    /// <summary>
    /// 初クリアか確認
    /// </summary>
    void FirstClearCheck()
    {
        //各ステージのクリア状況を確認する
        for (int i = 1; i <= stageAmount; i++)
        {
            //クリア情報取得
            int firstClear = PlayerPrefs.GetInt("FirstClear" + i.ToString(), 0);
            int clear = PlayerPrefs.GetInt("Clear" + i.ToString(), 0);

            if (clear == 1 && firstClear == 0 && i < stageAmount) //初クリアじゃないなら
            {
                //解放済みのステージのAnimationを全て解放演出完了済みの状態にする
                for (int j = i; j > 0; j--)
                {
                    release[j - 1].SetTrigger("Released");
                    bg[j - 1].SetTrigger("Released");
                }
            }

            if (firstClear == 1) //初クリアなら
            {
                if(i < stageAmount)
                {
                    release[i - 1].SetTrigger("Release"); //解放演出再生
                    bg[i - 1].SetTrigger("Release");
                }
                
                //初クリアかどうかをfalseにする
                PlayerPrefs.SetInt("FirstClear" + i.ToString(), 0);
            }
        }
    }
}
