using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
    Animator anim;
    [SerializeField, Header("獲得確認ウィンドウ")] GameObject window;
    bool isOpen; //宝箱が開けるか

    void Start()
    {
        anim = GetComponent<Animator>();
        window.SetActive(false);
        isOpen = false;
    }

    void Update()
    {
        if(isOpen && Input.GetKeyDown(KeyCode.P))
        {
            anim.SetTrigger("Open");
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
