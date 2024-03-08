using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�E���g�_�E��
/// </summary>
public class CountDown : MonoBehaviour
{
    Animator anim;
    PlayerController player;

    PauseWindow pauseWindow;
    PauseController pauseController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();
        pauseWindow = GameObject.FindObjectOfType<PauseWindow>();
        pauseController = GameObject.FindObjectOfType<PauseController>();

        //�v���C���[����s��, Animation�Đ��ׂ̈�timeScale��1
        pauseController.Pause(true, 1);

        //�J�E���g�_�E��
        anim.SetBool("Start", true);
    }

    /// <summary>
    /// �J�E���g�_�E���I��
    /// </summary>
    public void CountEnd()
    {
        player.CountEnd = true; //�J�E���g�_�E�����I���������Ƃ�`����
        pauseWindow.GameStart = true;

        //�|�[�Y����
        pauseController.Pause(false, 1);
    }
}
