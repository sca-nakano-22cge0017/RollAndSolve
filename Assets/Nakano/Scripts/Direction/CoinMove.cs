using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージクリア演出の1つ
/// 宝箱から出てきたメダルの動きを制御
/// </summary>
public class CoinMove : MonoBehaviour
{
    [SerializeField, Header("メダル上昇スピード")] float medalSpeed;
    [SerializeField, Header("メダル縮小速度")] float redSpeed;
    [SerializeField, Header("プレイヤーに近付く速度")] float speed;

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

        //上昇
        if (move < 1.0f)
        {
            move += medalSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * medalSpeed * Time.deltaTime);
        }
        if(move >= 1.0f) { isMove = true; }

        if(isMove)
        {
            //縮小
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(redSpeed, redSpeed, 0) * Time.deltaTime;
            }

            //プレイヤーの位置へ移動
            if (Vector3.Distance(medalPos, playerPos) >= 0)
            {
                Vector3 dis = (playerPos - medalPos).normalized;
                transform.Translate(dis * speed * Time.deltaTime);
            }
        }
    }
}
