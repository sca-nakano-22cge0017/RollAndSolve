using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBallController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] float speed;
    [SerializeField, Header("初期移動方向反転"), Tooltip("falseで右へ、trueで左へ")] bool isReverse;
    [SerializeField, Header("オブジェクトの半径")] float radius;

    Vector3 direction;

    Collider2D col;
    Rigidbody2D rb;

    void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        col.isTrigger = false;

        if (!isReverse)
        {
            direction = Vector3.right;
        }
        if(isReverse)
        {
            direction = Vector3.left;
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            direction *= -1;
        }

        if (collision.gameObject.tag == "Player")
        {
            col.isTrigger = true;
        }

        //接地中はIsTriggerがOnになっても落ちていかないように
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //段差から落ちた時用
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            direction *= -1;
        }

        //落下中にプレイヤー→地面に触れたとき用
        if (other.gameObject.tag == "Ground")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            col.isTrigger = false;
        }

        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 1;
        }
    }
}
