using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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

    SkeletonAnimation skeletonAnimation;
    bool isWalk = false;

    [SerializeField] int track;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(track, "blessing", true);
        isWalk = false;

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

        var distance = (playerPos - thisPos).sqrMagnitude;

        if (distance <= dis * dis)
        {
            isChase = true;
        }
        else if(distance > dis * dis)
        {
            isChase = false;
        }

        direction = (playerPos - thisPos).normalized;

        if (isChase)
        {
            if (isMove)
            {
                transform.Translate(new Vector3(direction.x, 0, 0) * speed * Time.deltaTime);

                if(direction.x > 0)
                {
                    this.transform.localScale = new Vector3(-0.05f, 0.05f, 1);
                }
                else if(direction.x <= 0)
                {
                    this.transform.localScale = new Vector3(0.05f, 0.05f, 1);
                }
            }

            if (!isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(track, "walk", true);
                isWalk = true;
            }
        }

        else
        {
            if (isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(track, "blessing", true);
                isWalk = false;
            }
        }

        if (leftHit && direction.x < 0)
        {
            isMove = false;
        }
        else { isMove = true; }

        if (rightHit && direction.x > 0)
        {
            isMove = false;
        }
        else { isMove = true; }
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
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //�i�����痎�������p
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�������Ƀv���C���[���n�ʂɐG�ꂽ�Ƃ��p
        if (other.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
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
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
