using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���N���A�V�[������^�C�g��/�{�[�i�X�X�e�[�W�ւ̑J��
/// </summary>
public class ClearToTitle : MonoBehaviour
{
    [SerializeField, Header("�^�C�g���ɑJ�ڂ���܂ł̎���(�b)")] float sec = 3; 

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(sec);

        //�B���A�C�e����9�������Ă�����{�[�i�X�X�e�[�W��
        if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
        {
            SceneManager.LoadScene("BonusStage");
        }
        else SceneManager.LoadScene("Title");
    }
}
