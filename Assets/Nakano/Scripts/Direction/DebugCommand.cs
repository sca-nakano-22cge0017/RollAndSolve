using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �W���p�@�f�[�^�ҏW
/// </summary>
public class DebugCommand : MonoBehaviour
{
    int bonus = 0;
    int clear = 0;

    void Update()
    {
        //�i�s�󋵂̃f�[�^���폜
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            bonus++;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            clear++;
        }

        //�ʃL�[���͂œ��͉񐔏�����
        if(bonus > 0 && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.B))
        {
            bonus = 0;
        }
        if (clear > 0 && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.C))
        {
            clear = 0;
        }

        //B5��A�łŃ{�[�i�X�X�e�[�W�Ɉڍs
        if (bonus >= 5)
        {
            SceneManager.LoadScene("BonusStage");
        }

        //C5��A�łŃX�e�[�W�S�J��
        if(clear >= 5)
        {
            for(int num = 1; num <= 3; num++)
            {
                if (PlayerPrefs.GetInt("Clear" + num.ToString(), 0) == 0)
                {
                    PlayerPrefs.SetInt("FirstClear" + num.ToString(), 1);
                }

                PlayerPrefs.SetInt("Clear" + num.ToString(), 1);

                PlayerPrefs.Save();
            }
        }
    }
}
