using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 defaultPosition;

    ButtonCheck check;

    bool isPush = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = this.transform.position;

        check = gameObject.GetComponent<ButtonCheck>();

        rb.isKinematic = true;
    }

    void Update()
    {
        //ã‚É”ò‚ñ‚Å‚¢‚©‚È‚¢‚æ‚¤‚Éˆê‰ž
        if (transform.position.y > defaultPosition.y)
        {
            transform.position = defaultPosition;
            rb.velocity = Vector3.zero;
        }

        if(rb.velocity.y < 0)
        {
            isPush = true;
        }

        if(isPush && !check.IsActive)
        {
            rb.AddForce(Vector3.down * 10 * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && !check.IsActive)
        {
            var p = collision.gameObject.GetComponent<PlayerController>();

            if(p.playerstate == PlayerController.PlayerState.Human) { rb.isKinematic = false; }
            else { rb.isKinematic = true; }
        }

        if(collision.gameObject.name == "Button_Base")
        {
            check.IsActive = true;

            //‰Ÿ‚³‚ê‚½‚çˆÊ’u‚ðŒÅ’è‚·‚é
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !check.IsActive)
        {
            var p = collision.gameObject.GetComponent<PlayerController>();

            if (p.playerstate == PlayerController.PlayerState.Human) { rb.isKinematic = false; }
            else { rb.isKinematic = true; }
        }
    }
}
