using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    [SerializeField, Header("メダル上昇スピード")] float medalSpeed;
    [SerializeField, Header("メダル縮小速度")] float redSpeed;
    [SerializeField, Header("プレイヤーに近付く速度")] float speed;

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

        if (transform.localPosition.y < 1.0f)
        {
            transform.Translate(Vector3.up * medalSpeed * Time.deltaTime);
        }
        if(transform.localPosition.y >= 1.0f) { isMove = true; }

        if(isMove)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(redSpeed, redSpeed, 0) * Time.deltaTime;
            }

            if (Vector3.Distance(medalPos, playerPos) >= 0)
            {
                Vector3 dis = (playerPos - medalPos).normalized;
                transform.Translate(dis * speed * Time.deltaTime);
            }
        }
    }
}
