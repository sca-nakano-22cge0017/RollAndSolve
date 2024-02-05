using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームクリアシーンからタイトル/ボーナスステージへの遷移
/// </summary>
public class ClearToTitle : MonoBehaviour
{
    [SerializeField, Header("タイトルに遷移するまでの時間(秒)")] float sec = 3; 

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(sec);

        //隠しアイテムが9枚揃っていたらボーナスステージへ
        if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
        {
            SceneManager.LoadScene("BonusStage");
        }
        else SceneManager.LoadScene("Title");
    }
}
