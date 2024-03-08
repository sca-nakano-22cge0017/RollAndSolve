using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�N���A���o��1��
/// �󔠂���o�Ă������_���̓����𐧌�
/// </summary>
public class CoinMove : MonoBehaviour
{
    [SerializeField, Header("���_���㏸�X�s�[�h")] float medalSpeed;
    [SerializeField, Header("���_���k�����x")] float redSpeed;
    [SerializeField, Header("�v���C���[�ɋߕt�����x")] float speed;

    float move = 0; //���ΓI�Ȉړ�����

    GameObject player;
    Vector3 playerPos;
    Vector3 medalPos;
    Vector3 dis;

    bool isApproach = false; //�v���C���[�ɐڋ߂���

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        MedalDirection();
    }

    /// <summary>
    /// ���_���ړ�
    /// </summary>
    void MedalDirection()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        medalPos = transform.position;

        //�㏸ y��������+1�ړ�
        if (move < 1.0f)
        {
            move += medalSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * medalSpeed * Time.deltaTime);
        }
        //�㏸���I�������v���C���[�ɋߕt��
        else if(move >= 1.0f && !isApproach)
        {
            isApproach = true;
        }

        if(isApproach)
        {
            //�k��
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(redSpeed, redSpeed, 0) * Time.deltaTime;
            }

            //�v���C���[�̈ʒu�ֈړ�
            if (Vector3.Distance(medalPos, playerPos) >= 0)
            {
                dis = (playerPos - medalPos).normalized;
                transform.Translate(dis * speed * Time.deltaTime);
            }
        }
    }
}
