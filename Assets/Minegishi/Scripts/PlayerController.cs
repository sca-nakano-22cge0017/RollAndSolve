using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Box box;
    HPController HpController;

    public enum PlayerState { Human, Circle}
    public PlayerState playerstate;

    private Rigidbody2D rb;

    private float speed = 0;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [Header("加速度")]
    [SerializeField] private float HumansAccelertion; //人形態の時の加速度
    [SerializeField] private float CirclesAccelertion; //球体形態の時の加速度

    private float HumansSpeed = 0.0f;
    private float CirclesSpeed = 0.0f;

    private float HumansSpeedUp = 0.0f;
    private float CirclesSpeedUp = 0.0f;

    [Header("最高速度")]
    [SerializeField] private float MaxSpeed;

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

    [Header("プレイヤーの画像")]
    [SerializeField] Sprite Humans;
    [SerializeField] Sprite Circles;
    SpriteRenderer sr;

    bool invincible = false; //無敵状態
    float invincibleTime = 3.0f; //無敵時間
    int alpha = 255;
    float interval = 0.15f;

    bool isDead = false;

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value;}
    }

    void Start()
    {
        box = GameObject.Find("Box").GetComponent<Box>();
        this.HpController = FindObjectOfType<HPController>();
        rb = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        playerstate = PlayerState.Circle;
        HumansSpeed = HumansAccelertion; //速度初期化
        CirclesSpeed = CirclesAccelertion; //速度初期化

        HumansSpeedUp = HumansAccelertion * 1.2f; //スピードアップした時の速度
        CirclesSpeedUp = CirclesAccelertion * 1.2f; //スピードアップした時の速度
    }


    void Update()
    {
        //Debug.Log(CirclesSpeed);

        if (!isDead) 
        {
            Run();
        }
        
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
                    sr.sprite = Circles;
                    Debug.Log("球体です");
                }
                    Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Human;
                    sr.sprite = Humans;
                    Debug.Log("人です");
                }
                Circle();
                break;
        }

        if (invincible) //無敵状態
        {
            invincibleTime -= Time.deltaTime;
            if(invincibleTime > 0)
            {
                Invincible();
            }
            else if(invincibleTime <= 0)
            {
                invincibleTime = 3.0f;
                invincible = false;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 255);
            }
        }

        Vector2 position = transform.position;

        if(position.y < -5.5) //穴に落ちたら
        {
            isDead = true;
        }

        transform.position = position;
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
            if(speed >= MaxSpeed)
            {
               speed = MaxSpeed;
            }

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

            if(speed <= -MaxSpeed)
            {
                speed = -MaxSpeed;
            }
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

    private void Invincible() //無敵状態
    {
        interval -= Time.deltaTime;
        
        if(interval <= 0)
        {
            if(alpha == 255)
            {
                alpha = 0;
            }else
                alpha = 255;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            interval = 0.15f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }

        if(collision.gameObject.tag == "Enemy" && !invincible) //無敵状態じゃないときに敵と接触
        {
            Debug.Log("敵と接触");
            speed -= speed * 0.5f;
            invincible = true;
            //HpController.Hp--;
        }

        if(collision.gameObject.tag == "Box" && playerstate == PlayerState.Circle) //箱を破壊
        {
            if (Input.GetKey(KeyCode.D) && speed >= 7.0f)
            {
                speed -= speed * 0.2f;
                Debug.Log("箱を破壊");
                //Destroy(collision.gameObject);
            }
            if (Input.GetKey(KeyCode.A) && speed <= -7.0f)
            {
                speed -= speed * 0.2f;
                Debug.Log("箱を破壊");
               // Destroy(collision.gameObject);
            }
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
            speed = 0.0f; //壁に当たったら速度をリセット
        }

        //人形態の時に箱に接触しているとき箱を押す
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
}
