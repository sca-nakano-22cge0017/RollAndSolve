using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDestroy : MonoBehaviour
{
    Animator anim;
    PlayerController playerController;
    Collider2D col;
    [SerializeField] AudioClip se;
    AudioSource audioSource;
    Vector3 tr;
    BoxesRecreate boxesRecreate;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        tr = GetComponent<Transform>().position;
        boxesRecreate = GameObject.FindObjectOfType<BoxesRecreate>();

        audioSource = GetComponent<AudioSource>();
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
                }
                
                anim.SetTrigger("Break");
                audioSource.PlayOneShot(se);
                Destroy(col);
            }
        }
    }

    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }
}
