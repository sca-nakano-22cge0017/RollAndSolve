using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// クリア画面からタイトル画面/ボーナスステージへの遷移
/// </summary>
public class ClearToTitle : MonoBehaviour
{
    bool canChange = false; //遷移可能かどうか
    [SerializeField] Text explain;

    private void Start()
    {
        explain.enabled = false;
    }

    void Update()
    {
        if(canChange)
        {
            explain.enabled = true;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                //隠しコインが全て集まっていたらボーナスステージへ
                if(PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
                {
                    SceneManager.LoadScene("BonusStage");
                }
                else SceneManager.LoadScene("Title");
            }
        }
    }

    //アニメーションが終了したら遷移できるようにする
    public void Change()
    {
        canChange = true;
    }
}
