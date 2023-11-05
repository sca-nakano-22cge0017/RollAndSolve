using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 defaultPosition;

    ButtonCheck check;

    bool isActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = this.transform.position;

        check = gameObject.GetComponent<ButtonCheck>();
    }

    void Update()
    {
        //ã‚É”ò‚ñ‚Å‚¢‚©‚È‚¢‚æ‚¤‚Éˆê‰ž
        if (transform.position.y > defaultPosition.y)
        {
            transform.position = defaultPosition;
            rb.velocity = Vector3.zero;
        }

        //‰Ÿ‚³‚ê‚½‚çˆÊ’u‚ðŒÅ’è‚·‚é
        if(isActive)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Button_Base")
        {
            check.IsActive = true;
            isActive = true;
        }
    }
}
