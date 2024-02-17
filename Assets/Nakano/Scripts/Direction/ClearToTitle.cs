using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���N���A�V�[������^�C�g��/�{�[�i�X�X�e�[�W�ւ̑J��
/// </summary>
public class ClearToTitle : MonoBehaviour
{
    bool canChange = false;

    private void Update()
    {
        if(canChange)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //�B���A�C�e����9�������Ă�����{�[�i�X�X�e�[�W��
                if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
                {
                    SceneManager.LoadScene("BonusStage");
                }
                else SceneManager.LoadScene("Title");
            }
        }
    }

    public void Change()
    {
        canChange = true;
    }
}
