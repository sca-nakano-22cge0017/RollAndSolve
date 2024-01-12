using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageRelease : MonoBehaviour
{
    [SerializeField] Animator[] release;

    private void Awake()
    {
        FirstClearCheck();
    }

    void FirstClearCheck()
    {
        for (int i = 1; i <= 3; i++) //3�̓X�e�[�W��
        {
            int firstClear = PlayerPrefs.GetInt("FirstClear" + i.ToString(), 0);
            int clear = PlayerPrefs.GetInt("Clear" + i.ToString(), 0);

            if (clear == 1 && firstClear == 0 && i < 3) //���N���A����Ȃ��Ȃ�
            {
                for (int j = i; j > 0; j--)
                {
                    release[j - 1].SetTrigger("Released");
                }
            }

            if (firstClear == 1) //���N���A�Ȃ�
            {
                if(i < 3)
                {
                    release[i - 1].SetTrigger("Release"); //������o�Đ�
                }
                
                PlayerPrefs.SetInt("FirstClear" + i.ToString(), 0);
            }
        }
    }

    private void Update()
    {
        //Debug�p�@�i�s�󋵂̃f�[�^���폜
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
