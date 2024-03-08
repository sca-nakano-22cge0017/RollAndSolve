using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 宝箱の制御
/// </summary>
public class TreasureController : MonoBehaviour
{
    Animator anim;

    [SerializeField, Header("メダル")] GameObject medal;

    bool isOpen; //宝箱を開くか
    bool isClear; //クリア判定

    /// <summary>
    /// クリア判定
    /// trueのときステージクリア
    /// </summary>
    public bool IsClear
    {
        get { return isClear; }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        
        isOpen = false;
        isClear = false;
    }

    void Update()
    {
        //宝箱に触れたらクリア
        if (isOpen)
        {
            //アニメーション
            anim.SetTrigger("Open");

            //メダル獲得演出
            Instantiate(medal, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.1f, 0), Quaternion.identity);

            isClear = true;
            isOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーが当たったら、宝箱を開くかどうかのフラグをtrueにする
        if(collision.gameObject.tag == "Player")
        {
            isOpen = true;
        }
        
        //もし高所から落下したとき地面で止まる
        if (collision.gameObject.tag == "Ground")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.mass = 0;
            rb.gravityScale = 0;
        }
    }
}
