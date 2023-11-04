using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAnim : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���Player�̏��(�J�v�Z�����ǂ���/���x�͈��ȏォ�ǂ���)�������ɉ�����
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Break");
        }
    }

    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }
}
