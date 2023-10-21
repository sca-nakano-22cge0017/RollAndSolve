using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //後でPlayerの状態(カプセルかどうか/速度は一定以上かどうか)も条件に加える
        if(collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Break");
        }
    }

    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }
}
