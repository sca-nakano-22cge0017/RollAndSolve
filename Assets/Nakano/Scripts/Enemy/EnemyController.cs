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
    [SerializeField, Header("������Collider")] GameObject leftCol;
    [SerializeField, Header("�E����Collider")] GameObject rightCol;

    Vector3 toPlayer, moveDir; //�G����v���C���[�ւ̃x�N�g���A�ړ�����
    float distance; //�G�ƃv���C���[�̒�������
    bool isChase; //�v���C���[��ǂ������Ă��邩�ǂ���
    bool isMove; //�ړ��ł��邩
    bool leftHit, rightHit; //���E�����ɏՓ˕������邩

    [SerializeField, Header("���ł������� (�x�N�g��)")] Vector3 flyDir;
    Vector3 dir; //���ł�������������p�̕ϐ�
    [SerializeField, Header("���ł������x")] float flySpeed;
    [SerializeField, Header("���ł����Ƃ��̓G�̊p�x")] float flyAngle;
    bool isFly = false; //��΂���Ă���r����

    PlayerController playerController;
    GameObject player;

    Collider2D col;
    Rigidbody2D rb;

    //�C���X�g�̌���
    Vector3 facingRight = new Vector3(-0.05f, 0.05f, 1); //�E����
    Vector3 facingLeft = new Vector3(0.05f, 0.05f, 1); //������

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

        isWalk = false;
        isChase = false;
        isMove = true;

        moveDir = Vector3.right;
    }

    private void Update()
    {
        toPlayer = player.transform.position - this.transform.position;
        moveDir = toPlayer.normalized; //�ړ������Z�o
        distance = toPlayer.sqrMagnitude; //�v���C���[�Ƃ̒��������Z�o

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
                transform.Translate(new Vector3(moveDir.x, 0, 0) * speed * Time.deltaTime);
            }

            //�������[�V�������Đ����Ă��Ȃ��ꍇ�͍Đ�
            //�ǂ��Ă���Ԃ͍Đ���������
            if (!isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
                isWalk = true;
            }

            //�E�ֈړ�����Ƃ�
            if (moveDir.x > 0)
            {
                //�ړ������ɉ�����Enemy�̌�����ς���
                this.transform.localScale = facingRight;

                //���E�����̏Փ˂����m����Collider���q�I�u�W�F�N�g�ɂ��Ă��邽�߁AEnemy�{�̂����]����ƍ��E���]���Ă��܂��̂�
                //Enemy�������Ă�����ɉ�����Collider�����ւ���
                leftHit = rightCol.GetComponent<EnemyCol>().IsHit;
                rightHit = leftCol.GetComponent<EnemyCol>().IsHit;
            }
            //���ֈړ�����Ƃ�
            else
            {
                this.transform.localScale = facingLeft;

                leftHit = leftCol.GetComponent<EnemyCol>().IsHit;
                rightHit = rightCol.GetComponent<EnemyCol>().IsHit;
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

        //������Collider�ɂȂɂ��Ԃ������Ƃ��@���ړ����������̂Ƃ�
        //�E����Collider�ɂȂɂ��Ԃ������Ƃ��@���ړ��������E�̂Ƃ�
        if ((leftHit && moveDir.x < 0) || (rightHit && moveDir.x > 0))
        {
            isMove = false; //�����Ȃ��Ȃ�
        }
        else if ((!leftHit && moveDir.x < 0) || (!rightHit && moveDir.x > 0))
            { isMove = true; }

        //��΂���Ă���Ƃ�
        if (isFly)
        {
            //�_���[�W�A�j���[�V����
            skeletonAnimation.AnimationState.SetAnimation(0, "damage", true);

            //�΂ߏ�����ɔ�΂�
            transform.Translate(dir * flySpeed * Time.deltaTime, Space.World);

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
        }

        //�n�ʏ�ɂ���Ƃ��͉��ɗ����Ȃ��悤�ɂ���
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    col.isTrigger = false;
        //}

        //�n�ʂ��痣�ꂽ���͗�����悤��
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
