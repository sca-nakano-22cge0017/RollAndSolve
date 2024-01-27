using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInstall : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�؂�ւ���̃J����")]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] [Tooltip("�؂�ւ���̃J����")] private CinemachineVirtualCamera virtualCamera1;
  
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera2;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera3;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera4;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera5;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerStay(Collider other)
    {
        // �������������"Player"�^�O���t���Ă����ꍇ
        if (other.gameObject.tag == "Player")
        {
            // ����virtualCamera���������D��x�ɂ��邱�ƂŐ؂�ւ��
            virtualCamera.Priority = 100;
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 0;
            virtualCamera3.Priority = 0;
            virtualCamera4.Priority = 0;
            virtualCamera5.Priority = 0;
            Debug.Log("�����Ă�");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // �������������"Player"�^�O���t���Ă����ꍇ
        if (other.gameObject.tag == "Player")
        {
            // ����priority�ɖ߂�
            virtualCamera1.Priority = 1;
            virtualCamera2.Priority = 0;
            virtualCamera3.Priority = 0;
            virtualCamera4.Priority = 0;
            virtualCamera5.Priority = 0;
            virtualCamera.Priority = 0;
            Debug.Log("�X�^�[�g����o��");
        }
    
    }

}