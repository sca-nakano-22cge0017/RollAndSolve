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

    float move = 0; //相対的な移動距離

    GameObject player;
    Vector3 playerPos;
    Vector3 medalPos;
    Vector3 dis;

    bool isApproach = false; //プレイヤーに接近する

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        MedalDirection();
    }

    /// <summary>
    /// メダル移動
    /// </summary>
    void MedalDirection()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        medalPos = transform.position;

        //上昇 y軸方向に+1移動
        if (move < 1.0f)
        {
            move += medalSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * medalSpeed * Time.deltaTime);
        }
        //上昇が終わったらプレイヤーに近付く
        else if(move >= 1.0f && !isApproach)
        {
            isApproach = true;
        }

        if(isApproach)
        {
            //縮小
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(redSpeed, redSpeed, 0) * Time.deltaTime;
            }

            //プレイヤーの位置へ移動
            if (Vector3.Distance(medalPos, playerPos) >= 0)
            {
                dis = (playerPos - medalPos).normalized;
                transform.Translate(dis * speed * Time.deltaTime);
            }
        }
    }
}
