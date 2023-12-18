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

    bool isActive = false;

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
            transform.Translate(Vector3.down * 10 * Time.deltaTime);
        }

        if (!isPush)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Button_Base")
        {
            isActive = true;
            rb.velocity = Vector2.zero;
            //‰Ÿ‚³‚ê‚½‚çˆÊ’u‚ðŒÅ’è‚·‚é
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Button_Base")
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
