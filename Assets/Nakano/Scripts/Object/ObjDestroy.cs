using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDestroy : MonoBehaviour
{
    Animator anim;
    PlayerController playerController;
    Collider2D col;
    Vector3 tr;
    BoxesRecreate boxesRecreate;
    SEController seController;

    [SerializeField, Header("”j‰óŒã•œŠˆ‚·‚é‚©‚Ç‚¤‚©")] bool isRecreate = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        tr = GetComponent<Transform>().position;
        boxesRecreate = GameObject.FindObjectOfType<BoxesRecreate>();

        seController = GameObject.FindObjectOfType<SEController>();

        if (this.gameObject.name == "Board") { isRecreate = false; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Hole")
        {
            if (this.gameObject.tag == "Box")
            {
                if (isRecreate) { boxesRecreate.Recreate(tr); } //–Ø” •œŠˆ

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
                if(this.gameObject.tag == "Box")
                {
                    if (isRecreate) { boxesRecreate.Recreate(tr); } //–Ø” •œŠˆ
                    seController.BoxDestroy();
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
}
