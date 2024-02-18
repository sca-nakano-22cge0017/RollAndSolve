using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            seController.ItemCatch();
            if (this.gameObject.tag == "HealHeart" && hpController.Hp < hpController.HpLimit)
            {
                hpController.IsHeal = true;
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
