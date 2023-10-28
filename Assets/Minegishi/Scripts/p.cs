using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p : MonoBehaviour
{
    public enum PlayerState { Human, Circle }
    PlayerState playerstate;

    private Rigidbody2D rb;

    private float speed = 0;

    [Header("�����x")]
    [SerializeField] private float HumansAccelertion = 1.0f; //�l�`�Ԃ̎��̉����x
    [SerializeField] private float CirclesAccelertion = 2.0f; //���̌`�Ԃ̎��̉����x

    [Header("�����x")]
    [SerializeField] private float deceleration;

    [Header("�W�����v��")]
    [SerializeField] private float HumansJump = 400f; //�l�`�Ԃ̂Ƃ��̃W�����v��
    [SerializeField] private float CirclesJump = 300f; //���̌`�Ԃ̂Ƃ��̃W�����v��
    private float jumpForce;

    bool isGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerstate = PlayerState.Circle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerstate)
        {
            case PlayerState.Human:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Circle;
                    Debug.Log("���̂ł�");
                }
                Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Human;
                    Debug.Log("�l�ł�");
                }
                Circle();
                break;
        }
    }

    void Human()
    {
        //speed = HumansSpeed;
        jumpForce = HumansJump;
    }

    void Circle()
    {
        //speed = CirclesSpeed;
        jumpForce = CirclesJump;
    }

    void Run()
    {
        Vector2 position = transform.position; 
        if (Input.GetKey(KeyCode.D))
        {
            if (playerstate == PlayerState.Human)
            {
                speed += HumansAccelertion * Time.deltaTime;
            }
            else if (playerstate == PlayerState.Circle)
            {
                speed += CirclesAccelertion * Time.deltaTime;
            }
            position.x += speed * Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("D�𗣂�");
            if(speed > 0)
            {
                speed -= deceleration * Time.deltaTime;
                position.x += speed * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (playerstate == PlayerState.Human)
            {
                speed += HumansAccelertion * Time.deltaTime;
            }
            else if (playerstate == PlayerState.Circle)
            {
                speed += CirclesAccelertion * Time.deltaTime;
            }
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            if (speed < 0)
            {
                speed += deceleration * Time.deltaTime;
                position.x -= speed * Time.deltaTime;
            }
        }

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            this.rb.AddForce(transform.up * jumpForce);
            Debug.Log(jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("�ڒn");
            isGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            speed = 0.0f;
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
