using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TimeScale�A�|�[�Y��Ԃ̐���
/// </summary>
public class PauseController : MonoBehaviour
{
    bool isPause = false; //�|�[�Y��Ԃ�

    public bool IsPause { get { return isPause; } }

    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    /// <summary>
    /// �|�[�Y��Ԃ̐ݒ�Ɖ���
    /// </summary>
    /// <param name="playerPause">�v���C���[�̑�����~�߂�@true�Œ�~</param>
    /// <param name="scale">timeScale�̒l</param>
    public void Pause(bool playerPause, int scale)
    {
        if(playerPause) playerController.IsPause = true;
        else playerController.IsPause = false;

        Time.timeScale = scale;
    }
}
