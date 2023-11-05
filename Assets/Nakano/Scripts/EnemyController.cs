using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")] float speed;
    [SerializeField, Header("�ǐՔ͈�")] float dis;
    [SerializeField, Header("���ւ�Ray�̈ʒu")] GameObject leftRay;
    [SerializeField, Header("�E�ւ�Ray�̈ʒu")] GameObject rightRay;

    GameObject player;
    Vector3 playerPos;
    Vector3 thisPos;

    Collider2D col;
    Rigidbody2D rb;

    Vector3 direction; //�ړ�����
    bool isChase; //�v���C���[��ǂ������Ă��邩�ǂ���

    bool isMove;
    bool leftHit, rightHit;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        col.isTrigger = false;
        isChase = false;
        isMove = true;

        direction = Vector3.right;
    }

    void Update()
    {
        playerPos = player.transform.position;
        thisPos = this.transform.position;

        leftHit = leftRay.GetComponent<EnemyRay>().isHit;
        rightHit = rightRay.GetComponent<EnemyRay>().isHit;

        var distance = Vector3.Distance(playerPos, thisPos);

        if (distance <= dis)
        {
            isChase = true;
        }
        else if(distance > dis)
        {
            isChase = false;
        }

        if(isChase)
        {
            direction = (playerPos - thisPos).normalized;
            if(isMove)
            {
                transform.Translate(new Vector3(direction.x, 0, 0) * speed * Time.deltaTime);
            }
        }

        if(leftHit && direction.x < 0)
        {
            isMove = false;
        }
        else if(!leftHit && direction.x < 0) { isMove = true; }

        if(rightHit && direction.x >= 0)
        {
            isMove = false;
        }
        else if(!rightHit && direction.x >= 0) { isMove = true; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            col.isTrigger = true;
        }

        //�ڒn����IsTrigger��On�ɂȂ��Ă������Ă����Ȃ��悤��
        else if (collision.gameObject.tag == "Ground")
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
        //�������Ƀv���C���[���n�ʂɐG�ꂽ�Ƃ��p
        if (other.gameObject.tag == "Ground")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            col.isTrigger = false;
        }

        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 1;
        }
    }
}
