using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// �X�e�[�W�R�̃J��������p CameraInstall.cs���x�[�X�ɂ��Ĉꎞ�I�ȏC���Ƃ��Ēǉ�
/// �񖇖ڂ̉B���R�C�����J�����ɉf��Ȃ������̂œˊтŒǉ��������� �v�C��
/// </summary>
public class Stage3Camera : MonoBehaviour
{
    [SerializeField, Tooltip("�����ʒu�̃J����")]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField, Tooltip("��{�̃J����")] private CinemachineVirtualCamera virtualCamera1;

    [SerializeField, Tooltip("�B���R�C���p�J����")] private CinemachineVirtualCamera virtualCamera2;

    private void OnTriggerStay2D(Collider2D other)
    {
        // �������������"Player"�^�O���t���Ă����ꍇ
        if (other.gameObject.tag == "Player")
        {
            // ����virtualCamera���������D��x�ɂ��邱�ƂŐ؂�ւ��
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 100;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // �������������"Player"�^�O���t���Ă����ꍇ
        if (other.gameObject.tag == "Player")
        {
            // ����priority�ɖ߂�]
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 100;
            virtualCamera2.Priority = 0;
        }

    }
}
