using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemCatch : MonoBehaviour
{
    HPController hpController;
    SecretCoin secretCoin;

    void Start()
    {
        hpController = GameObject.FindObjectOfType<HPController>();
        secretCoin = GameObject.FindObjectOfType<SecretCoin>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(this.gameObject.tag == "HealHeart" && hpController.Hp < hpController.HpLimit)
            {
                hpController.Hp++;
            }

            if(this.gameObject.tag == "EmptyHeart")
            {
                hpController.HpLimit++;
            }

            if (this.gameObject.tag == "SecretCoin")
            {
                secretCoin.CoinGet(this.gameObject.name);
            }

            if(this.gameObject.name == "BigBonusCoin")
            {
                BonusClear bonusClear = GameObject.FindObjectOfType<BonusClear>();
                bonusClear.Clear();
            }

            Destroy(this.gameObject);
        }
    }
}
