using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// �X�e�[�W�R�̃J��������p CameraInstall.cs���x�[�X�ɁA�񖇖ڂ̉B���R�C�����J�����ɉf��Ȃ������̂ŏC���������̂��쐬
/// </summary>
public class Stage3Camera : MonoBehaviour
{
    [SerializeField, Tooltip("�����ʒu�̃J����")]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField, Tooltip("��{�̃J����")] private CinemachineVirtualCamera virtualCamera1;

    [SerializeField, Tooltip("�B���R�C���p�J����")] private CinemachineVirtualCamera virtualCamera2;

    private void OnTriggerStay2D(Collider2D other)
    {
        //����͈̔͂ɓ�������J�����ύX
        //�����ʒu�J����
        if(other.gameObject.name == "Camera1")
        {
            virtualCamera.Priority = 1;
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 0;
        }

        //�f�t�H���g�̃J����
        if (other.gameObject.name == "Camera2")
        {
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 1;
            virtualCamera2.Priority = 0;
        }

        //�B���R�C���\���p�J����
        if (other.gameObject.name == "Camera3")
        {
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //�͈͊O�ɏo����f�t�H���g�̃J�����ɖ߂�
        if(other.gameObject.name == "Camera1" || other.gameObject.name == "Camera3")
        {
            virtualCamera.Priority = 0;
            virtualCamera1.Priority = 1;
            virtualCamera2.Priority = 0;
        }
    }
}
