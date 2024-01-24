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
    [SerializeField, Header("çƒê∂ê¨Ç∑ÇÈÇ©Ç«Ç§Ç©")] bool isRecreate;
    Transform tr;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        tr = GetComponent<Transform>();

        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerController.ObjectBreak)
            {
                anim.SetTrigger("Break");
                audioSource.PlayOneShot(se);
                Destroy(col);

                if(isRecreate)
                {

                }
            }
        }
    }

    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }

    IEnumerator Recreate()
    {
        yield return new WaitForSeconds(2.0f);
        
    }
}
