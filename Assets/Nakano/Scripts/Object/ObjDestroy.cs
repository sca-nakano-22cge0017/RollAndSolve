using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// �ؔ�/�؂̔̔j��
/// </summary>
public class ObjDestroy : MonoBehaviour
{
    PlayerController playerController;

    Collider2D col;
    Rigidbody2D rb;

    //�ؔ��Đ���
    Vector3 tr; //�Đ����ʒu
    BoxesRecreate boxesRecreate;
    [SerializeField, Header("�j��㕜�����邩�ǂ���")] bool isRecreate = true;
    bool rec = false;

    //���]����
    bool isFall = false; //�]���藎����t���O
    bool isFallCr = false; //�]���藎�����Ƃ��̕����p�̃t���O
    [SerializeField, Header("���]���藎���邩�ǂ���")] bool isFallBox = false;
    [SerializeField, Header("���]���藎����Ƃ��̉�]���x")] float rotateSpeed = -180;

    //���o�n
    Animator anim;
    CinemachineImpulseSource impulse;
    SEController seController;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        tr = GetComponent<Transform>().position;
        boxesRecreate = GameObject.FindObjectOfType<BoxesRecreate>();

        impulse = GetComponent<CinemachineImpulseSource>();
        anim = GetComponent<Animator>();
        seController = GameObject.FindObjectOfType<SEController>();

        //�؂̔͍Đ������Ȃ��̂Ō����isRecreate��true�ɂ���Ȃ��悤��
        if (this.gameObject.name == "Board") { isRecreate = false; }
    }

    private void Update()
    {
        //�ؔ��̕���
        if(rec)
        {
            rec = false;
            boxesRecreate.Recreate(tr);
        }

        //�₩��]���藎����Ƃ��̏���
        if(isFall)
        {
            //��񂾂��Đ���
            if (!isFallCr)
            {
                boxesRecreate.FallBox(tr, true);
                isFallCr = true;
            }

            //��]
            rb.angularVelocity = rotateSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ؔ��͌��ɗ����������
        if(collision.gameObject.tag == "Hole" && this.gameObject.tag == "Box")
        {
            //���]�����Ă��Ȃ��Ƃ�
            if (!isFall)
            {
                if (isRecreate) { rec = true; } //�ؔ�����

                StartCoroutine(BoxFall());
            }

            //���]�����Ă���Ƃ��͕ʂ̕��@�ōĐ�������̂ł��̂܂܏���
            else { Destroy(this); }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //�v���C���[���I�u�W�F�N�g�j��\�Ȏ�
            if (playerController.ObjectBreak)
            {
                //��ʗh��
                impulse.GenerateImpulse();

                if(this.gameObject.tag == "Box")
                {
                    if (isRecreate) { rec = true; } //�ؔ�����

                    seController.BoxDestroy(); //�j��SE

                    //�q�I�u�W�F�N�g�폜
                    foreach (Transform n in gameObject.transform)
                    {
                        Destroy(n.gameObject);
                    }
                }

                //�؂̔Ȃ畜���������ɂ��̂܂ܔj��
                if(this.gameObject.name == "Board")
                {
                    seController.BoardDestroy(); //�j��SE
                }
                
                anim.SetTrigger("Break"); //�j��A�j���[�V����

                Destroy(col); //�����蔻�������
            }
        }

        //��ɐG�ꂽ�炻�̂܂ܓ]���藎����
        if(collision.gameObject.CompareTag("Slope") && !isFall && isFallBox)
        {
            isFall = true; //��]������

            rb.mass = 0.001f;
            rb.angularDrag = 0;
            rb.gravityScale = 3;

            //������Ԃ��Ɖ�]���Ȃ��悤��constraints���ݒ肳��Ă���̂ŉ�]����悤�ɕύX
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    /// <summary>
    /// animation�̍Đ����I�������Ăяo���ăI�u�W�F�N�g�폜
    /// </summary>
    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// �ؔ��̉�ʊO�ւ̗���
    /// </summary>
    IEnumerator BoxFall()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
