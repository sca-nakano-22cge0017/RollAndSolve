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

    float move = 0;

    GameObject player;
    Vector3 playerPos;
    Vector3 medalPos;

    bool isMove = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        MedalDirection();
    }

    void MedalDirection()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        medalPos = transform.position;

        //�㏸
        if (move < 1.0f)
        {
            move += medalSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * medalSpeed * Time.deltaTime);
        }
        if(move >= 1.0f) { isMove = true; }

        if(isMove)
        {
            //�k��
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(redSpeed, redSpeed, 0) * Time.deltaTime;
            }

            //�v���C���[�̈ʒu�ֈړ�
            if (Vector3.Distance(medalPos, playerPos) >= 0)
            {
                Vector3 dis = (playerPos - medalPos).normalized;
                transform.Translate(dis * speed * Time.deltaTime);
            }
        }
    }
}
