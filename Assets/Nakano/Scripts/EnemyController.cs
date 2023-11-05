using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] float speed;
    [SerializeField, Header("追跡範囲")] float dis;
    [SerializeField, Header("左へのRayの位置")] GameObject leftRay;
    [SerializeField, Header("右へのRayの位置")] GameObject rightRay;

    GameObject player;
    Vector3 playerPos;
    Vector3 thisPos;

    Collider2D col;
    Rigidbody2D rb;

    Vector3 direction; //移動方向
    bool isChase; //プレイヤーを追いかけているかどうか

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

        //接地中はIsTriggerがOnになっても落ちていかないように
        else if (collision.gameObject.tag == "Ground")
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
        //落下中にプレイヤー→地面に触れたとき用
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
