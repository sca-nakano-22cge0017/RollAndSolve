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
        //���Player�̏��(�J�v�Z�����ǂ���/���x�͈��ȏォ�ǂ���)�������ɉ�����
        if(collision.gameObject.tag == "Player")
        {
            //if (playerController.playerstate == PlayerController.PlayerState.Circle && playerController.speed >= 7.0f)
            //{
            //    anim.SetTrigger("Break");
            //    Destroy(col);
            //}

            //�A�j���[�V�����m�F�p
            //anim.SetTrigger("Break");
            //Destroy(col);
        }
    }

    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }
}
