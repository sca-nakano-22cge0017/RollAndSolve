using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Human, Circle}
    PlayerState playerstate;

    private Rigidbody2D rb;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float jumpForce = 3f;

    bool isGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerstate = PlayerState.Circle;
    }


    void Update()
    {
        Run();
        switch (playerstate)
        {
            case PlayerState.Human:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Circle;
                }
                    Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Human;
                }
                Circle();
                break;
        }


    }

    void Human()
    {
        speed = 1.0f;
        jumpForce = 400f;
        //Debug.Log("êlÇ≈Ç∑");
    }

    void Circle()
    {
        speed = 3.0f;
        jumpForce = 300f;
        //Debug.Log("ãÖëÃÇ≈Ç∑");
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed, 0,0) * Time.deltaTime) ;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            this.rb.AddForce(transform.up * jumpForce);
            Debug.Log(jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }
}
