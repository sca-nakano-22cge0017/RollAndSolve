using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �A�C�e���擾 �e�A�C�e���ɕt����
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

            //HP�񕜃A�C�e��
            if (this.gameObject.tag == "HealHeart")
            {
                hpController.IsHeal = true;
            }

            //HP�������A�C�e��
            if(this.gameObject.tag == "EmptyHeart")
            {
                hpController.IsLimitBreak = true;
            }

            //�B���R�C��
            if (this.gameObject.tag == "SecretCoin")
            {
                secretCoin.CoinGet(this.gameObject.name);
            }

            //�{�[�i�X�X�e�[�W�@�ŏI�R�C��
            if(this.gameObject.name == "BigBonusCoin")
            {
                BonusClear bonusClear = GameObject.FindObjectOfType<BonusClear>();
                bonusClear.Clear(); //�{�[�i�X�X�e�[�W�N���A
            }

            //�I�u�W�F�N�g����
            Destroy(this.gameObject);
        }
    }
}
