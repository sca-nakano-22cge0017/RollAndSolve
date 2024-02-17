using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 隠しアイテム(SecretCoin)の取得
/// </summary>
public class SecretCoin : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] coin;

    void Start()
    {
        CoinCheck();
    }

    void Update()
    {
        
    }

    //SecretCoinを獲得した時の処理
    public void CoinGet(string name)
    {
        //初獲得
        if(PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0) == 0)
        {
            PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 1);

            int num = PlayerPrefs.GetInt("SecretCoin", 0); //獲得枚数更新
            num++;
            PlayerPrefs.SetInt("SecretCoin", num);
        }
    }

    //マップ上のSecretCoinが獲得済みか確認する
    void CoinCheck()
    {
        for(int i = 1; i <= coin.Length; i++)
        {
            string name = "SecretCoin" + i.ToString();

            int num = PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);

            //獲得済みなら
            if (num == 1)
            {
                coin[i - 1].color = new Color(0.9f, 0.9f, 0.9f, 0.4f); //半透明に
            }
            else if(num == 0) coin[i - 1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else
            {
                PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);
            }
        }
    }
}
