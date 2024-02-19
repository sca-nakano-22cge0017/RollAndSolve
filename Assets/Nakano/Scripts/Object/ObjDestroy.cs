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

    [SerializeField, Header("破壊後復活するかどうか")] bool isRecreate = true;
    bool rec = false;

    CinemachineImpulseSource impulse;

    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        tr = GetComponent<Transform>().position;
        boxesRecreate = GameObject.FindObjectOfType<BoxesRecreate>();

        seController = GameObject.FindObjectOfType<SEController>();

        if (this.gameObject.name == "Board") { isRecreate = false; }
    }

    private void Update()
    {
        if(rec)
        {
            rec = false;
            boxesRecreate.Recreate(tr);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Hole")
        {
            if (this.gameObject.tag == "Box")
            {
                if (isRecreate) { rec = true; } //木箱復活

                StartCoroutine(BoxFall());
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
