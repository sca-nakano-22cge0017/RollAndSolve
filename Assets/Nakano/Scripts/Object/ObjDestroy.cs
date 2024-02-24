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

    [SerializeField, Header("破壊後復活するかどうか")] bool isRecreate = true;
    bool rec = false;

    CinemachineImpulseSource impulse;

    bool isFall = false; //転がり落ちるフラグ
    bool isFallCr = false; //↑のときの復活用のフラグ

    [SerializeField, Header("坂を転がり落ちるときの回転速度")] float rotateSpeed = -180;

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
        //木箱の復活
        if(rec)
        {
            rec = false;
            boxesRecreate.Recreate(tr);
        }

        //坂から転がり落ちるときの処理
        if(isFall)
        {
            if (!isFallCr)
            {
                boxesRecreate.FallBox(tr, true);
                isFallCr = true;
            }

            //回転
            rb.angularVelocity = rotateSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Hole" && this.gameObject.tag == "Box")
        {
            if (!isFall)
            {
                if (isRecreate) { rec = true; } //木箱復活

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
                    if (isRecreate) { rec = true; } //木箱復活
                    seController.BoxDestroy();

                    //子オブジェクト削除
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
