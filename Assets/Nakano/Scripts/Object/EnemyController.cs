using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Cinemachine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")] float speed;
    [SerializeField, Header("�ǐՔ͈�")] float dis;
    [SerializeField, Header("���ւ�Ray�̈ʒu")] GameObject leftRay;
    [SerializeField, Header("�E�ւ�Ray�̈ʒu")] GameObject rightRay;

    [SerializeField, Header("���ł������� (�x�N�g��)")] Vector3 flyDir;
    Vector3 dir;
    [SerializeField, Header("���ł������x")] float flySpeed;
    [SerializeField, Header("���ł����Ƃ��̓G�̊p�x")] float flyAngle;
    bool isFly = false;

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

    PlayerController playerController;

    CinemachineImpulseSource impulse;

    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(0, "blessing", true);
        isWalk = false;

        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        col.isTrigger = false;
        isChase = false;
        isMove = true;

        direction = Vector3.right;
    }

    private void Update()
    {
        if (leftHit)
        {
            if((transform.localScale.x == 0.05f && direction.x < 0) || (transform.localScale.x == -0.05f && direction.x > 0))
                isMove = false;
        }
        else { isMove = true; }

        if (rightHit)
        {
            if ((transform.localScale.x == 0.05f && direction.x > 0) || (transform.localScale.x == -0.05f && direction.x < 0))
                isMove = false;
        }
        else { isMove = true; }
    }

    void FixedUpdate()
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

        if (isChase)
        {
            if (isMove)
            {
                direction = (playerPos - thisPos).normalized;
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
                skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
                isWalk = true;
            }
        }

        else
        {
            if (isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "blessing", true);
                isWalk = false;
            }
        }

        if(isFly)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "damage", true);
            Transform myTrans = this.transform;
            Vector3 pos = myTrans.position;
            pos.x += dir.x * flySpeed * Time.deltaTime;
            pos.y += dir.y * flySpeed * Time.deltaTime;
            myTrans.position = pos;

            if(transform.position.y >= 10)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerController.ObjectBreak)
            {
                impulse.GenerateImpulse();

                Destroy(rb);
                Destroy(col);
                isFly = true;

                if (playerController.Speed < 0)
                {
                    transform.Rotate(0, 0, -flyAngle, Space.World);
                    dir = new Vector3(-flyDir.x, flyDir.y, flyDir.z).normalized;
                }
                if (playerController.Speed >= 0)
                {
                    transform.Rotate(0, 0, flyAngle, Space.World);
                    dir = flyDir.normalized;
                }
            }
            else
            {
                col.isTrigger = true;
            }
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

    private void OnTriggerStay2D(Collider2D other)
    {
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

        else if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
