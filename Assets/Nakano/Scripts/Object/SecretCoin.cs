using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 隠しアイテム(SecretCoin)の取得　獲得状況確認
/// </summary>
public class SecretCoin : MonoBehaviour
{
    [SerializeField, Tooltip("隠しコインのオブジェクト名は、SecretCoin + 番号にする。\n番号・アタッチの順番はステージのスタート側から昇順")] SpriteRenderer[] coin;

    void Start()
    {
        CoinCheck();
    }

    /// <summary>
    /// SecretCoinを獲得した時の処理
    /// </summary>
    /// <param name="name">オブジェクト名</param>
    public void CoinGet(string name)
    {
        //SecretCoin: + シーン名 + オブジェクトの名前　オブジェクト名と name の部分が一致するデータを持ってくる
        //獲得済みかどうかを判定する boolの代わりにintを使用 0のときfalse 1のときtrue
        //未獲得なら
        if (PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0) == 0)
        {
            //獲得済みにする
            PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 1);

            //獲得枚数更新
            int num = PlayerPrefs.GetInt("SecretCoin", 0);
            num++;
            PlayerPrefs.SetInt("SecretCoin", num);
        }
    }

    /// <summary>
    /// マップ上のSecretCoinが獲得済みか確認する
    /// </summary>
    void CoinCheck()
    {
        //ステージ上の隠しコイン全てを確認
        for(int i = 1; i <= coin.Length; i++)
        {
            //コインの名前は SecretCoin + 番号にする オブジェクト名と同じ名前に
            string name = "SecretCoin" + i.ToString();

            //獲得済みかどうかを判定する boolの代わりにintを使用 0のときfalse 1のときtrue
            //SecretCoin: + シーン名 + コインの名前
            int num = PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);

            //獲得済みなら
            if (num == 1)
            {
                coin[i - 1].color = new Color(0.9f, 0.9f, 0.9f, 0.4f); //半透明に
            }
            //未獲得なら
            else if (num == 0)
            {
                coin[i - 1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f); //不透明
            }
            //もし想定外の値が入っていた場合
            else
            {
                //未獲得状態に直す
                PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);
            }
        }
    }
}
