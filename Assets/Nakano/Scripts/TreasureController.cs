using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureController : MonoBehaviour
{
    Animator anim;
    [SerializeField, Header("獲得確認ウィンドウ")] GameObject window;

    [SerializeField, Header("メダル")] GameObject medal;

    bool isOpen; //宝箱が開けるか
    bool isClear; //クリア判定

    public bool IsClear // プロパティ
    {
        get { return isClear; }  // 通称ゲッター。呼び出した側がscoreを参照できる
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        window.SetActive(false);
        isOpen = false;
        isClear = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isOpen)
        {
            anim.SetTrigger("Open");
            Instantiate(medal, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.1f, 0), Quaternion.identity);
            isClear = true;
            window.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            window.SetActive(true);
            isOpen = true;
        }

        if (collision.gameObject.tag == "Ground")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.mass = 0;
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            window.SetActive(false);
            isOpen = false;
        }
    }
}
