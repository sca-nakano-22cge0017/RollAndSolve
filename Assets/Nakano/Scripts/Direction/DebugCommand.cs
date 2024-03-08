using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 展示用　データ編集
/// </summary>
public class DebugCommand : MonoBehaviour
{
    int bonus = 0;
    int clear = 0;

    void Update()
    {
        //進行状況のデータを削除
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            bonus++;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            clear++;
        }

        //別キー入力で入力回数初期化
        if(bonus > 0 && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.B))
        {
            bonus = 0;
        }
        if (clear > 0 && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.C))
        {
            clear = 0;
        }

        //B5回連打でボーナスステージに移行
        if (bonus >= 5)
        {
            SceneManager.LoadScene("BonusStage");
        }

        //C5回連打でステージ全開放
        if(clear >= 5)
        {
            for(int num = 1; num <= 3; num++)
            {
                if (PlayerPrefs.GetInt("Clear" + num.ToString(), 0) == 0)
                {
                    PlayerPrefs.SetInt("FirstClear" + num.ToString(), 1);
                }

                PlayerPrefs.SetInt("Clear" + num.ToString(), 1);

                PlayerPrefs.Save();
            }
        }
    }
}
