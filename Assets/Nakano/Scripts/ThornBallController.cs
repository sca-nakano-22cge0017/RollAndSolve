using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBallController : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")] float speed;
    [SerializeField, Header("�����ړ��������]"), Tooltip("false�ŉE�ցAtrue�ō���")] bool isReverse;
    [SerializeField, Header("�I�u�W�F�N�g�̔��a")] float radius;

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

        //�ڒn����IsTrigger��On�ɂȂ��Ă������Ă����Ȃ��悤��
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //�i�����痎�������p
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

        //�������Ƀv���C���[���n�ʂɐG�ꂽ�Ƃ��p
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
