using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �N���A��ʂ���^�C�g�����/�{�[�i�X�X�e�[�W�ւ̑J��
/// </summary>
public class ClearToTitle : MonoBehaviour
{
    bool canChange = false; //�J�ډ\���ǂ���

    void Update()
    {
        if(canChange)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                //�B���R�C�����S�ďW�܂��Ă�����{�[�i�X�X�e�[�W��
                if(PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
                {
                    SceneManager.LoadScene("BonusStage");
                }
                else SceneManager.LoadScene("Title");
            }
        }
    }

    //�A�j���[�V�������I��������J�ڂł���悤�ɂ���
    public void Change()
    {
        canChange = true;
    }
}
