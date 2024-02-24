using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ObjDestroy : MonoBehaviour
{
    Animator anim;
    PlayerController playerController;
    Collider2D col;
    Vector3 tr;
    BoxesRecreate boxesRecreate;
    SEController seController;
    Rigidbody2D rb;

    [SerializeField, Header("�j��㕜�����邩�ǂ���")] bool isRecreate = true;
    bool rec = false;

    CinemachineImpulseSource impulse;

    bool isFall = false; //�]���藎����t���O
    bool isFallCr = false; //���̂Ƃ��̕����p�̃t���O

    [SerializeField, Header("���]���藎����Ƃ��̉�]���x")] float rotateSpeed = -180;

    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        tr = GetComponent<Transform>().position;
        boxesRecreate = GameObject.FindObjectOfType<BoxesRecreate>();
        rb = GetComponent<Rigidbody2D>();

        seController = GameObject.FindObjectOfType<SEController>();

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
        if(collision.gameObject.tag == "Hole" && this.gameObject.tag == "Box")
        {
            if (!isFall)
            {
                if (isRecreate) { rec = true; } //�ؔ�����

                StartCoroutine(BoxFall());
            }

            else { Destroy(this); }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerController.ObjectBreak)
            {
                impulse.GenerateImpulse();

                if(this.gameObject.tag == "Box")
                {
                    if (isRecreate) { rec = true; } //�ؔ�����
                    seController.BoxDestroy();

                    //�q�I�u�W�F�N�g�폜
                    foreach (Transform n in gameObject.transform)
                    {
                        Destroy(n.gameObject);
                    }
                }

                if(this.gameObject.name == "Board")
                {
                    seController.BoardDestroy();
                }
                
                anim.SetTrigger("Break");
                Destroy(col);
            }
        }

        if(collision.gameObject.CompareTag("Slope") && !isFall)
        {
            isFall = true;
            rb.mass = 0.001f;
            rb.angularDrag = 0;
            rb.gravityScale = 3;
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }

    IEnumerator BoxFall()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
