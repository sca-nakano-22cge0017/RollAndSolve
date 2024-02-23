using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public enum BUTTON { red = 0, blue };
    public BUTTON buttonType = 0;

    Rigidbody2D rb;
    Vector3 defaultPosition;

    bool isPush = false;

    [SerializeField, Header("Debug—p")] bool isActive = false;

    public bool IsPush
    {
        get { return isPush; }
        set { isPush = value; }
    }

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = this.transform.position;

        rb.isKinematic = true;
    }

    void Update()
    {
        if (transform.position.y >= defaultPosition.y)
        {
            transform.position = defaultPosition;
            rb.velocity = Vector3.zero;
        }

        if (isPush && !isActive)
        {
            transform.Translate(Vector3.down * 5 * Time.deltaTime);
        }

        if (!isPush)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            if(buttonType == ButtonObject.BUTTON.blue)
            {
                if (transform.position.y <= defaultPosition.y)
                {
                    transform.position = defaultPosition;
                    rb.velocity = Vector3.zero;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActive)
        {
            var p = collision.gameObject.GetComponent<PlayerController>();
            
            if (p.playerstate == PlayerController.PlayerState.Human)
            {
                isPush = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                isPush = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        if (collision.gameObject.CompareTag("Box"))
        {
            isPush = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        if (collision.gameObject.name == "Button_Base")
        {
            isActive = true;
            rb.velocity = Vector2.zero;
            //‰Ÿ‚³‚ê‚½‚çˆÊ’u‚ðŒÅ’è‚·‚é
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActive)
        {
            var p = collision.gameObject.GetComponent<PlayerController>();

            if (p.playerstate == PlayerController.PlayerState.Human)
            {
                isPush = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                isPush = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        if (collision.gameObject.CompareTag("Box"))
        {
            isPush = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        if (collision.gameObject.name == "Button_Base")
        {
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (buttonType == ButtonObject.BUTTON.blue)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
            {
                isPush = false;
            }
        }

        if (collision.gameObject.name == "Button_Base")
        {
            isActive = false;
        }
    }
}
