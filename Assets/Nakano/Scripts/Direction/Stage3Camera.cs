using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Stage3Camera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�؂�ւ���̃J����")]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] [Tooltip("�؂�ւ���̃J����")] private CinemachineVirtualCamera virtualCamera1;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
