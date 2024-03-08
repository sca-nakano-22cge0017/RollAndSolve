using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �B���A�C�e��(SecretCoin)�̎擾�@�l���󋵊m�F
/// </summary>
public class SecretCoin : MonoBehaviour
{
    [SerializeField, Tooltip("�B���R�C���̃I�u�W�F�N�g���́ASecretCoin + �ԍ��ɂ���B\n�ԍ��E�A�^�b�`�̏��Ԃ̓X�e�[�W�̃X�^�[�g�����珸��")] SpriteRenderer[] coin;

    void Start()
    {
        CoinCheck();
    }

    /// <summary>
    /// SecretCoin���l���������̏���
    /// </summary>
    /// <param name="name">�I�u�W�F�N�g��</param>
    public void CoinGet(string name)
    {
        //SecretCoin: + �V�[���� + �I�u�W�F�N�g�̖��O�@�I�u�W�F�N�g���� name �̕�������v����f�[�^�������Ă���
        //�l���ς݂��ǂ����𔻒肷�� bool�̑����int���g�p 0�̂Ƃ�false 1�̂Ƃ�true
        //���l���Ȃ�
        if (PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0) == 0)
        {
            //�l���ς݂ɂ���
            PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 1);

            //�l�������X�V
            int num = PlayerPrefs.GetInt("SecretCoin", 0);
            num++;
            PlayerPrefs.SetInt("SecretCoin", num);
        }
    }

    /// <summary>
    /// �}�b�v���SecretCoin���l���ς݂��m�F����
    /// </summary>
    void CoinCheck()
    {
        //�X�e�[�W��̉B���R�C���S�Ă��m�F
        for(int i = 1; i <= coin.Length; i++)
        {
            //�R�C���̖��O�� SecretCoin + �ԍ��ɂ��� �I�u�W�F�N�g���Ɠ������O��
            string name = "SecretCoin" + i.ToString();

            //�l���ς݂��ǂ����𔻒肷�� bool�̑����int���g�p 0�̂Ƃ�false 1�̂Ƃ�true
            //SecretCoin: + �V�[���� + �R�C���̖��O
            int num = PlayerPrefs.GetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);

            //�l���ς݂Ȃ�
            if (num == 1)
            {
                coin[i - 1].color = new Color(0.9f, 0.9f, 0.9f, 0.4f); //��������
            }
            //���l���Ȃ�
            else if (num == 0)
            {
                coin[i - 1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f); //�s����
            }
            //�����z��O�̒l�������Ă����ꍇ
            else
            {
                //���l����Ԃɒ���
                PlayerPrefs.SetInt("SecretCoin:" + SceneManager.GetActiveScene().name + "-" + name, 0);
            }
        }
    }
}
