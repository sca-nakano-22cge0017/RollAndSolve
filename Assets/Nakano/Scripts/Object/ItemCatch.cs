using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// アイテム取得 各アイテムに付ける
/// </summary>
public class ItemCatch : MonoBehaviour
{
    HPController hpController;
    SecretCoin secretCoin;
    SEController seController;

    void Start()
    {
        hpController = GameObject.FindObjectOfType<HPController>();
        secretCoin = GameObject.FindObjectOfType<SecretCoin>();
        seController = GameObject.FindObjectOfType<SEController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //SE
            seController.ItemCatch();

            //HP回復アイテム
            if (this.gameObject.tag == "HealHeart")
            {
                hpController.IsHeal = true;
            }

            //HP上限解放アイテム
            if(this.gameObject.tag == "EmptyHeart")
            {
                hpController.IsLimitBreak = true;
            }

            //隠しコイン
            if (this.gameObject.tag == "SecretCoin")
            {
                secretCoin.CoinGet(this.gameObject.name);
            }

            //ボーナスステージ　最終コイン
            if(this.gameObject.name == "BigBonusCoin")
            {
                BonusClear bonusClear = GameObject.FindObjectOfType<BonusClear>();
                bonusClear.Clear(); //ボーナスステージクリア
            }

            //オブジェクト消去
            Destroy(this.gameObject);
        }
    }
}
