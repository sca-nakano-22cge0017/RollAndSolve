using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームクリアシーンからタイトル/ボーナスステージへの遷移
/// </summary>
public class ClearToTitle : MonoBehaviour
{
    bool canChange = false;

    private void Update()
    {
        if(canChange)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //隠しアイテムが9枚揃っていたらボーナスステージへ
                if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
                {
                    SceneManager.LoadScene("BonusStage");
                }
                else SceneManager.LoadScene("Title");
            }
        }
    }

    public void Change()
    {
        canChange = true;
    }
}
