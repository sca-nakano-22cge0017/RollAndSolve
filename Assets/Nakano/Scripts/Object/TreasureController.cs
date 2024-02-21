using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureController : MonoBehaviour
{
    Animator anim;

    [SerializeField, Header("メダル")] GameObject medal;

    bool isOpen; //宝箱が開けるか
    bool isClear; //クリア判定

    PlayerController playerController;

    /// <summary>
    /// クリア判定
    /// trueのとき、クリア
    /// </summary>
    public bool IsClear
    {
        get { return isClear; }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.FindObjectOfType<PlayerController>();

        isOpen = false;
        isClear = false;
    }

    void Update()
    {
        //宝箱に触れたらクリア
        if (isOpen)
        {
            anim.SetTrigger("Open");

            //メダル獲得演出
            Instantiate(medal, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.1f, 0), Quaternion.identity);
            isClear = true;
            isOpen = false;
            playerController.IsPause = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーが当たったら指示ウィンドウを表示し、宝箱を開くことができるかどうかのフラグをtrueにする
        if(collision.gameObject.tag == "Player")
        {
            isOpen = true;
        }
        
        //宝箱の落下
        if (collision.gameObject.tag == "Ground")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.mass = 0;
            rb.gravityScale = 0;
        }
    }
}
