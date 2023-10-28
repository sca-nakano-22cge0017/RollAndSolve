using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p : MonoBehaviour
{
    public enum PlayerState { Human, Circle }
    PlayerState playerstate;

    private Rigidbody2D rb;

    private float speed = 0;

    [Header("加速度")]
    [SerializeField] private float HumansAccelertion = 1.0f; //人形態の時の加速度
    [SerializeField] private float CirclesAccelertion = 2.0f; //球体形態の時の加速度

    [Header("減速度")]
    [SerializeField] private float deceleration;

    [Header("ジャンプ力")]
    [SerializeField] private float HumansJump = 400f; //人形態のときのジャンプ力
    [SerializeField] private float CirclesJump = 300f; //球体形態のときのジャンプ力
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
                    Debug.Log("球体です");
                }
                Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Human;
                    Debug.Log("人です");
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
            Debug.Log("Dを離す");
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

        //ジャンプ
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
            Debug.Log("接地");
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
