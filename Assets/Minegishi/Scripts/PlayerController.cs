using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Box box;

    public enum PlayerState { Human, Circle}
    PlayerState playerstate;

    private Rigidbody2D rb;

    private float speed = 0;
    [Header("加速度")]
    [SerializeField] private float HumansAccelertion; //人形態の時の加速度
    [SerializeField] private float CirclesAccelertion; //球体形態の時の加速度

    private float HumansSpeed = 0.0f;
    private float CirclesSpeed = 0.0f;

    private float HumansSpeedUp = 0.0f;
    private float CirclesSpeedUp = 0.0f;

    [Header("減速度")]
    [SerializeField] private float deceleration;
    bool RightDeceleration = false;
    bool LeftDeceleration  = false;

    [Header("ジャンプ力")]
    [SerializeField] private float HumansJump = 400f; //人形態のときのジャンプ力
    [SerializeField] private float CirclesJump = 300f; //球体形態のときのジャンプ力
    private float jumpForce;

    bool speedUp = false;
    float speedUpCount = 7.0f; //スピードアップのアイテムを取った時の上昇する時間

    bool isGround = false;

    void Start()
    {
        box = GameObject.Find("Box").GetComponent<Box>();
        rb = GetComponent<Rigidbody2D>();
        playerstate = PlayerState.Circle;
        HumansSpeed = HumansAccelertion; //速度初期化
        CirclesSpeed = CirclesAccelertion; //速度初期化

        HumansSpeedUp = HumansAccelertion * 1.2f; //スピードアップした時の速度
        CirclesSpeedUp = CirclesAccelertion * 1.2f; //スピードアップした時の速度
    }


    void Update()
    {
        Debug.Log(CirclesSpeed);

        Run();
        if (speedUp)
        {
            SpeedUp();
        }

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
        Debug.Log(speed);
        if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A)))
        {
            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                speed += HumansSpeed * Time.deltaTime;
            }
            else if(playerstate == PlayerState.Circle)
            {
                speed += CirclesSpeed * Time.deltaTime;
            }
            transform.Translate(new Vector3(speed, 0,0) * Time.deltaTime) ;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            RightDeceleration = true;
        }
        else if (RightDeceleration)
        {
            Debug.Log("Dを離す");
            if (speed > 0)
            {
                speed -= deceleration * Time.deltaTime;
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
                if (speed <= 0.0f)
                {
                    //speed = 0.0f;
                }
            }
        }

        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D)))
        {
            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                speed -= HumansSpeed * Time.deltaTime;
            }
            else if (playerstate == PlayerState.Circle)
            {
                speed -= CirclesSpeed * Time.deltaTime;
            }
            transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            LeftDeceleration = true;
        }
        if (LeftDeceleration)
        {
            Debug.Log("Aを離す");
            if (speed < 0)
            {
                speed += deceleration * Time.deltaTime;
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
                if (speed >= 0.0f)
                {
                    //speed = 0.0f;
                }
            }
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            if(speed < 0)
            {
                speed += deceleration * Time.deltaTime;
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
            }
            if(speed > 0)
            {
                speed -= deceleration * Time.deltaTime;
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
            }
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            this.rb.AddForce(transform.up * jumpForce);
            Debug.Log(jumpForce);
        }
    }

    private void SpeedUp()
    {
        speedUpCount -= Time.deltaTime;
        if (speedUpCount >= 0)
        {
            //速度が1.2倍になる
            HumansSpeed = HumansSpeedUp;
            CirclesSpeed = CirclesSpeedUp;
        }
        else if (speedUpCount < 0)
        {
            //カウントが0になったら速度が元の戻る
            HumansSpeed = HumansAccelertion;
            CirclesSpeed = CirclesAccelertion;

            speedUpCount = 7.0f;
            speedUp = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }

        if(collision.gameObject.tag == "Enemy") //敵と接触
        {
            Debug.Log("敵と接触");
            //life--;
        }

        if (collision.gameObject.tag == "SpeedUP") //スピードアップ
        {
            Destroy(collision.gameObject);
            speedUpCount = 7.0f;
            speedUp = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Box")
        {
            speed = 0.0f; //壁に当たったら１度速度をリセット
        }

        //人形態の時に箱に接触している時
        if (playerstate == PlayerState.Human && collision.gameObject.tag == "Box")
        {
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.F))
            {
                box.BoxRightMove();
                speed = 1.0f;
            }
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.F))
            {
                box.BoxLeftMove();
                speed = -1.0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "SpeedUp")
        {
            Destroy(collision.gameObject);
        }
    }
}
