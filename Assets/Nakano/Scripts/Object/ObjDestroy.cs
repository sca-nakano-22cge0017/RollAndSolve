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

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        tr = GetComponent<Transform>().position;
        boxesRecreate = GameObject.FindObjectOfType<BoxesRecreate>();

        seController = GameObject.FindObjectOfType<SEController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerController.ObjectBreak)
            {
                if(this.gameObject.tag == "Box")
                {
                    boxesRecreate.Recreate(tr);
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
