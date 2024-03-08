using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �g�Q�{�[���̗���
/// </summary>
public class ThornBallFall : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    float distanceX = 0.1f; //�v���C���[�Ƃ�X���W�̍�

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        //�v���C���[���I�u�W�F�N�g�̉��t�߂ɗ�����
        if(Mathf.Abs(this.transform.position.x - player.transform.position.x) <= distanceX)
        {
            //����������
            rb.isKinematic = false;
        }
    }
}
