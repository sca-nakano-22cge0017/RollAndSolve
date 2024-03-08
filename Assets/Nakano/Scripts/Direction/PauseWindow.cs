using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �|�[�Y��ʂ̕\���E��\��
/// </summary>
public class PauseWindow : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField, Header("�����I���{�^��")] Button StartButton;

    PauseController pauseController;

    bool isPause = false; //�|�[�Y��ʂ̃I���I�t

    bool gameStart = false;
    /// <summary>
    /// true�̂Ƃ��Q�[���X�^�[�g
    /// </summary>
    public bool GameStart { set { gameStart = value; } }

    private void Start()
    {
        pauseController = GameObject.FindObjectOfType<PauseController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseChange();
        }
    }

    public void PauseEnd()
    {
        PauseChange();
    }

    /// <summary>
    /// �|�[�Y��Ԃ̕ύX
    /// </summary>
    void PauseChange()
    {
        //�|�[�Y��Ԕ��]
        isPause = !isPause;

        //�E�B���h�E�̃A�N�e�B�u�E��A�N�e�B�u�𔽓]
        menuPanel.SetActive(isPause);

        //�|�[�Y��Ԃ���Ȃ��Ȃ����Ƃ�
        if (!isPause)
        {
            //�J�E���g�_�E���Ȃǂ��I�����ăQ�[�����J�n����Ă���ꍇ
            if (gameStart)
            {
                //�v���C���[����AtimeScale = 1�̏�Ԃɂ���
                pauseController.Pause(false, 1);
            }
            //�J�E���g�_�E���Ȃǂ��܂��I�����Ă��Ȃ��ꍇ
            else
            {
                //�v���C���[����s�AtimeScale = 1
                pauseController.Pause(true, 1);
            }
        }
        //�|�[�Y��ԂɂȂ����Ƃ�
        else
        {
            StartButton.Select(); //�{�^����I�����Ă���

            //�v���C���[����s�AtimeScale = 0
            pauseController.Pause(true, 0);
        }
    }
}
