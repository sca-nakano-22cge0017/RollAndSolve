using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueButton : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 defaultPosition;

    ButtonCheck check;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = this.transform.position;

        check = gameObject.GetComponent<ButtonCheck>();
    }

    void Update()
    {
        if (transform.position.y < defaultPosition.y)
        {
            rb.AddForce(new Vector3(0, 1, 0));
        }

        if (transform.position.y >= defaultPosition.y)
        {
            transform.position = defaultPosition;
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Button_Base")
        {
            check.IsActive = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Button_Base")
        {
            check.IsActive = false;
        }
    }
}
