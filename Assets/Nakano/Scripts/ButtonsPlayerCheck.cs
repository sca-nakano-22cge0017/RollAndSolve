using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPlayerCheck : MonoBehaviour
{
    ButtonObject button;
    Rigidbody2D rb;

    void Start()
    {
        button = this.gameObject.transform.parent.GetComponent<ButtonObject>();

        rb = this.gameObject.transform.parent.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !button.IsActive)
        {
            var p = collision.gameObject.GetComponent<PlayerController>();

            if (p.playerstate == PlayerController.PlayerState.Human)
            {
                button.IsPush = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                button.IsPush = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        if(collision.gameObject.CompareTag("Box"))
        {
            button.IsPush = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(button.buttonType == ButtonObject.BUTTON.blue)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
            {
                button.IsPush = false;
            }
        }
    }
}
