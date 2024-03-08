using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class SecretCoins
{
    public Image[] coin;
}

/// <summary>
/// �X�e�[�W������o�A�l���B���R�C�����W��
/// </summary>
public class StageRelease : MonoBehaviour
{
    //������o
    [SerializeField] Animator[] release;
    [SerializeField] Animator[] bg;

    //�B���R�C�����W��
    [SerializeField] SecretCoins[] stage; //�e�X�e�[�W3�����ݒ�@�񎟌��z��ɂ��Ă���
    [SerializeField, Tooltip("���l����ԁE�l���ςݏ�Ԃ̏���Sprite��ݒ�")] Sprite[] coinImages;

    int stageAmount = 3; //�X�e�[�W�̐�
    int coinAmount = 3; //�e�X�e�[�W�ɂ���B���R�C���̐�

    private void Awake()
    {
        Time.timeScale = 1;
        FirstClearCheck();

        //�e�X�e�[�W�̉B���R�C�����W����\��
        for (int i = 0; i < stageAmount; i++)
        {
            for(int j = 0; j < coinAmount; j++)
            {
                //�f�[�^�擾
                var num = PlayerPrefs.GetInt("SecretCoin:Stage" + (i + 1).ToString() + "-SecretCoin" + (j + 1).ToString(), 0);

                stage[i].coin[j].sprite = coinImages[num];
            }
        }
    }

    /// <summary>
    /// ���N���A���m�F
    /// </summary>
    void FirstClearCheck()
    {
        //�e�X�e�[�W�̃N���A�󋵂��m�F����
        for (int i = 1; i <= stageAmount; i++)
        {
            //�N���A���擾
            int firstClear = PlayerPrefs.GetInt("FirstClear" + i.ToString(), 0);
            int clear = PlayerPrefs.GetInt("Clear" + i.ToString(), 0);

            if (clear == 1 && firstClear == 0 && i < stageAmount) //���N���A����Ȃ��Ȃ�
            {
                //����ς݂̃X�e�[�W��Animation��S�ĉ�����o�����ς݂̏�Ԃɂ���
                for (int j = i; j > 0; j--)
                {
                    release[j - 1].SetTrigger("Released");
                    bg[j - 1].SetTrigger("Released");
                }
            }

            if (firstClear == 1) //���N���A�Ȃ�
            {
                if(i < stageAmount)
                {
                    release[i - 1].SetTrigger("Release"); //������o�Đ�
                    bg[i - 1].SetTrigger("Release");
                }
                
                //���N���A���ǂ�����false�ɂ���
                PlayerPrefs.SetInt("FirstClear" + i.ToString(), 0);
            }
        }
    }
}
