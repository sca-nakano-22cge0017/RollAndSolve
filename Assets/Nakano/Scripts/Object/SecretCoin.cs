using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �B���A�C�e��(SecretCoin)�̎擾
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

    //SecretCoin���l���������̏���
    public void CoinGet(string name)
    {
        //���l��
        if(PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0) == 0)
        {
            PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 1);

            int num = PlayerPrefs.GetInt("SecretCoin", 0); //�l�������X�V
            num++;
            PlayerPrefs.SetInt("SecretCoin", num);
        }
    }

    //�}�b�v���SecretCoin���l���ς݂��m�F����
    void CoinCheck()
    {
        for(int i = 1; i <= coin.Length; i++)
        {
            string name = "SecretCoin" + i.ToString();

            int num = PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);

            //�l���ς݂Ȃ�
            if (num == 1)
            {
                coin[i - 1].color = new Color(0.9f, 0.9f, 0.9f, 0.4f); //��������
            }
            else if(num == 0) coin[i - 1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            else
            {
                PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);
            }
        }
    }
}
