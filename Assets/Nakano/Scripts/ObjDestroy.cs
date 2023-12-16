using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDestroy : MonoBehaviour
{
    Animator anim;
    PlayerController playerController;
    Collider2D col;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerController.ObjectBreak)
            {
                anim.SetTrigger("Break");
                Destroy(col);
            }

            //アニメーション確認用
            //anim.SetTrigger("Break");
            //Destroy(col);
        }
    }

    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }
}
