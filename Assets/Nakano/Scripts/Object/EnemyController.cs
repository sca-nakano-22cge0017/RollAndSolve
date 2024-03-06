using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Cinemachine;

/// <summary>
/// Enemy����
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")] float speed;
    [SerializeField, Header("�ǐՔ͈�")] float dis;
    [SerializeField, Header("���ւ�Ray�̈ʒu")] GameObject leftRay;
    [SerializeField, Header("�E�ւ�Ray�̈ʒu")] GameObject rightRay;

    [SerializeField, Header("���ł������� (�x�N�g��)")] Vector3 flyDir;
    Vector3 dir; //���ł�������������p�̕ϐ�
    [SerializeField, Header("���ł������x")] float flySpeed;
    [SerializeField, Header("���ł����Ƃ��̓G�̊p�x")] float flyAngle;
    bool isFly = false; //��΂���Ă���r����

    PlayerController playerController;
    GameObject player;
    Vector3 playerPos;

    Collider2D col;
    Rigidbody2D rb;

    //�C���X�g�̌���
    Vector3 facingRight = new Vector3(-0.05f, 0.05f, 1); //�E����
    Vector3 facingLeft = new Vector3(0.05f, 0.05f, 1); //������

    Vector3 moveDir; //�ړ�����
    bool isChase; //�v���C���[��ǂ������Ă��邩�ǂ���

    bool isMove; //�ړ��ł��邩
    bool leftHit, rightHit; //���E�����ɏՓ˕������邩

    //Spine
    SkeletonAnimation skeletonAnimation;
    bool isWalk = false;

    //Camera
    CinemachineImpulseSource impulse;

    void Start()
    {
        //�G���j���̉�ʗh��p
        impulse = GetComponent<CinemachineImpulseSource>();

        //animation
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(0, "blessing", true);

        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        col.isTrigger = false;

        isWalk = false;
        isChase = false;
        isMove = true;

        moveDir = Vector3.right;
    }

    private void Update()
    {
        //������Ray�ɂȂɂ��Ԃ������Ƃ�
        if (leftHit)
        {
            //�ړ����������̂Ƃ�
            if (moveDir.x < 0)
                isMove = false; //�����Ȃ�
        }
        else { isMove = true; }

        //�E����Ray�ɂȂɂ��Ԃ������Ƃ�
        if (rightHit)
        {
            //�ړ��������E�̂Ƃ�
            if (moveDir.x > 0)
                isMove = false;
        }
        else { isMove = true; }

        playerPos = player.transform.position;

        //���E�����̏Փ˂����m����Ray���q�I�u�W�F�N�g�ɂ��Ă��邽�߁AEnemy�{�̂����]�����Ray�̌��������]���Ă��܂��̂�
        //Enemy�������Ă�����ɉ�����Ray�����ւ���
        //�������̂Ƃ��͒ʏ�
        if (transform.localScale.x > 0)
        {
            leftHit = leftRay.GetComponent<EnemyRay>().isHit;
            rightHit = rightRay.GetComponent<EnemyRay>().isHit;
        }
        //�E�����̂Ƃ����]
        else
        {
            leftHit = rightRay.GetComponent<EnemyRay>().isHit;
            rightHit = leftRay.GetComponent<EnemyRay>().isHit;
        }

        //�v���C���[�Ƃ̒��������Z�o
        var distance = (playerPos - this.transform.position).sqrMagnitude;

        //�v���C���[�Ƃ̋��������ȉ����ƒǐՂ���
        if (distance <= dis * dis)
        {
            isChase = true;
        }
        else if (distance > dis * dis)
        {
            isChase = false;
        }

        //�v���C���[��ǂ��Ă���Ƃ�
        if (isChase)
        {
            //�ړ��\�ȂƂ�
            if (isMove)
            {
                //�ړ������Z�o�A�ړ�
                moveDir = (playerPos - this.transform.position).normalized;
                transform.Translate(new Vector3(moveDir.x, 0, 0) * speed * Time.deltaTime);

                //�ړ������ɉ�����Enemy�̌�����ς���
                if (moveDir.x > 0)
                {
                    this.transform.localScale = facingRight;
                }
                else if (moveDir.x <= 0)
                {
                    this.transform.localScale = facingLeft;
                }
            }

            //�������[�V�������Đ����Ă��Ȃ��ꍇ�͍Đ�
            if (!isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
                isWalk = true;
            }
        }

        //�v���C���[��ǂ��Ă��Ȃ��Ƃ�
        else
        {
            //�������[�V���������Ă���ꍇ�͑ҋ@���[�V�����ɐ؂�ւ�
            if (isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "blessing", true);
                isWalk = false;
            }
        }

        //��΂���Ă���Ƃ�
        if (isFly)
        {
            //�_���[�W�A�j���[�V����
            skeletonAnimation.AnimationState.SetAnimation(0, "damage", true);

            //�΂ߏ�����ɔ�΂�
            Transform myTrans = this.transform;
            Vector3 pos = myTrans.position;
            pos.x += dir.x * flySpeed * Time.deltaTime;
            pos.y += dir.y * flySpeed * Time.deltaTime;
            myTrans.position = pos;

            //������x��񂾂����
            if (transform.position.y >= 10)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�ɂԂ������Ƃ�
        if (collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[���I�u�W�F�N�g�j��\�ȂƂ�
            if (playerController.ObjectBreak)
            {
                //��ʗh��Đ�
                impulse.GenerateImpulse();

                //������΂�����ɑ��̂��̂ɂԂ���Ȃ��悤��Rigidbody2D�ACollider���폜
                Destroy(rb);
                Destroy(col);

                isFly = true;

                //�v���C���[���E�ֈړ����Ă���Ƃ� �� ��������Ԃ���ꂽ�Ƃ��A�E�֔��ł���
                if (playerController.Speed < 0)
                {
                    transform.Rotate(0, 0, -flyAngle, Space.World); //���ł��������Ɍ����ĉ�]
                    dir = new Vector3(-flyDir.x, flyDir.y, flyDir.z).normalized; //���E���������t�ɂ���
                }

                //�E����Ԃ���ꂽ�獶�֔��ł���
                else
                {
                    transform.Rotate(0, 0, flyAngle, Space.World);
                    dir = flyDir.normalized;
                }
            }
            else
            {
                //�v���C���[�����蔲������悤��
                col.isTrigger = true;
            }
        }

        else if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //�n�ʂ��痣�ꂽ���͗�����悤��
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //�n�ʏ�ɂ���Ƃ��͉��ɗ����Ȃ��悤�ɂ���
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            col.isTrigger = false;
        }

        //�n�ʂ��痣�ꂽ���͗�����悤��
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
