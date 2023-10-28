using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueButton : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 defaultPosition;

    bool isActive; //trueのとき、ギミックが発動する
    
    public bool IsActive
    {
        get { return isActive; }
    }

    void Start()
    {
        isActive = false;
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = this.transform.position;
    }

    void Update()
    {
        //プレイヤーが離れたら元の位置に戻る
        if (transform.position.y < defaultPosition.y)
        {
            rb.AddForce(new Vector3(0, 1, 0));
        }

        if (transform.position.y >= defaultPosition.y)
        {
            transform.position = defaultPosition;
            rb.velocity = Vector3.zero;
        }

        if (isActive)
        {
            Debug.Log("発動中");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Button_Base")
        {
            isActive = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Button_Base")
        {
            isActive = false;
        }
    }
}
