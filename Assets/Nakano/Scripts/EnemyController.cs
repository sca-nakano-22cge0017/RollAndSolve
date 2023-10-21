using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")] float speed;
    [SerializeField, Header("�ǐՔ͈�")] float dis;
    
    GameObject player;
    Vector3 playerPos;
    Vector3 thisPos;

    Vector3 direction; //�ړ�����
    bool isChase; //�v���C���[��ǂ������Ă��邩�ǂ���

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        isChase = false;
        direction = Vector3.right;
    }

    void Update()
    {
        playerPos = player.transform.position;
        thisPos = this.transform.position;

        var distance = Vector3.Distance(playerPos, thisPos);

        if (distance <= dis)
        {
            isChase = true;
        }
        else if(distance > dis)
        {
            isChase = false;
        }

        if(isChase)
        {
            direction = (playerPos - thisPos).normalized;
            transform.Translate(new Vector3(direction.x, 0, 0) * speed * Time.deltaTime);
        }
    }

    
}
