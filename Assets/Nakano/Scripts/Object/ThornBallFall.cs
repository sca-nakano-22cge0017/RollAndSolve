using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// トゲボールの落下
/// </summary>
public class ThornBallFall : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    float distanceX = 0.1f; //プレイヤーとのX座標の差

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        //プレイヤーがオブジェクトの下付近に来たら
        if(Mathf.Abs(this.transform.position.x - player.transform.position.x) <= distanceX)
        {
            //落下させる
            rb.isKinematic = false;
        }
    }
}
