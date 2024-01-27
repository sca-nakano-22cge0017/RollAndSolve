using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Box box;
    HPController HpController;
    Animator anim;
    //Slope slope;
    [SerializeField] GameObject[] playerForms;
    [SerializeField] Animator[] playerAnims;
    [SerializeField] MeshRenderer[] playerMeshs;

    public enum PlayerState { Human, Circle}
    public PlayerState playerstate;

    private Rigidbody2D rb;

    float angle = 0.0f;

    private float speed = 0;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private bool objectBreak = false;
    public bool ObjectBreak
    {
        get { return objectBreak;}
        set { objectBreak = value;}
    }

    [Header("加速度")]
    [SerializeField] private float HumansAccelertion; //人形態の時の加速度
    [SerializeField] private float CirclesAccelertion; //球体形態の時の加速度

    private float HumansSpeed = 0.0f;
    private float CirclesSpeed = 0.0f;

    private float HumansSpeedUp = 0.0f;
    private float CirclesSpeedUp = 0.0f;

    [Header("最高速度")]
    [SerializeField] private float HumansMaxSpeed;
    [SerializeField] private float CirclesMaxSpeed;

    [Header("減速度")]
    [SerializeField] private float HumansDeceleration;
    [SerializeField] private float CirclesDeceleration;
    bool RightDeceleration = false;
    bool LeftDeceleration  = false;

    [Header("人型ブレーキ")]
    [SerializeField] private float Brake;

    [Header("ジャンプ力")]
    [SerializeField] private float HumansJump = 400f; //人形態のときのジャンプ力
    [SerializeField] private float CirclesJump = 300f; //球体形態のときのジャンプ力
    private float jumpForce;

    bool speedUp = false;
    float speedUpCount = 7.0f; //スピードアップのアイテムを取った時の上昇する時間

    bool isGround = false;

    //[Header("プレイヤーの画像")]
    //[SerializeField] Sprite Humans;
    //[SerializeField] Sprite Circles;
    //SpriteRenderer sr;

    [Header("サウンド")]
    [SerializeField] AudioClip Move;
    [SerializeField] AudioClip Jump;
    [SerializeField] AudioClip Box;
    AudioSource audioSource;
    float soundSpan = 0.0f;
    bool run = false;
    

    bool invincible = false; //無敵状態
    float invincibleTime = 3.0f; //無敵時間
    int alpha = 255;
    float interval = 0.15f;

    void Start()
    {
        this.HpController = FindObjectOfType<HPController>();
        rb = GetComponent<Rigidbody2D>();
        //sr = gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerstate = PlayerState.Circle;
        HumansSpeed = HumansAccelertion; //速度初期化
        CirclesSpeed = CirclesAccelertion; //速度初期化

        HumansSpeedUp = HumansAccelertion * 1.2f; //スピードアップした時の速度
        CirclesSpeedUp = CirclesAccelertion * 1.2f; //スピードアップした時の速度

        //Spine
        anim = playerAnims[0];
        //現在の形態以外はMeshRendererをオフにして非表示にする
        playerMeshs[0].enabled = true; //カプセル
        playerMeshs[1].enabled = false; //右向き人型
        playerMeshs[2].enabled = false; //左向き人型
    }


    void Update()
    {
        if (!HpController.IsDown)
        {
            Run();
            PlayerJump();
            MoveSound();
        }
        else anim.SetTrigger("Dead");
        
        if (speedUp)
        {
            SpeedUp();
        }

        switch (playerstate)
        {
            case PlayerState.Human:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    anim.SetBool("Change", true);

                    StartCoroutine(ToBall());
                }
                    Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    StartCoroutine(ToHuman());
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
            HpController.IsDown = true;
        }

        transform.position = position;

        //球体形態で最高速度の７割以上の時オブジェクトを破壊できる
        if(speed >= CirclesMaxSpeed * 0.7 && playerstate == PlayerState.Circle)
        {
            objectBreak = true;
        }
        else
        {
            objectBreak = false;
        }

        anim.SetFloat("Speed", speed);
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
        if(speed >= 1.0f || speed <= -1.0f)
        {
            run = true;
        }

        if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A))) //Dを押す
        {
            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                if (speed <= -3)
                {
                    speed = -Brake;
                }
                speed += HumansSpeed * Time.deltaTime;
            }
            else if(playerstate == PlayerState.Circle)
            {
                speed += CirclesSpeed * Time.deltaTime;

                playerForms[0].GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }
            transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0,0) * Time.deltaTime);

            if(playerstate == PlayerState.Human)
            {
                if (speed >= HumansMaxSpeed)
                {
                    speed = HumansMaxSpeed;
                }
            }
            if(playerstate == PlayerState.Circle)
            {
                if(speed > CirclesMaxSpeed)
                {
                    speed += CirclesDeceleration * Time.deltaTime;
                }
                if(speed >= CirclesMaxSpeed)
                {
                    speed = CirclesMaxSpeed;
                }
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
                if (playerstate == PlayerState.Human)
                {
                    speed -= HumansSpeed * Time.deltaTime;
                }
                else if (playerstate == PlayerState.Circle)
                {
                    speed -= CirclesDeceleration * Time.deltaTime;
                }

                transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D))) //Aを押す
        {
            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                if (speed >= 3)
                {
                    speed = Brake;
                }
                speed -= HumansSpeed * Time.deltaTime;
            }
            else if (playerstate == PlayerState.Circle)
            {
                speed -= CirclesSpeed * Time.deltaTime;

                playerForms[0].GetComponent<Transform>().localScale = new Vector3(-1f, 1f, 1f);
            }
            transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);

            if(playerstate == PlayerState.Human)
            {
                if (speed <= -HumansMaxSpeed)
                {
                    speed = -HumansMaxSpeed;
                }
            }
            if (playerstate == PlayerState.Circle)
            {
                if (speed < CirclesMaxSpeed)
                {
                    speed -= CirclesDeceleration * Time.deltaTime;
                }
                if (speed <= -CirclesMaxSpeed)
                {
                    speed = -CirclesMaxSpeed;
                }
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
                if (playerstate == PlayerState.Human)
                {
                    speed += HumansSpeed * Time.deltaTime;
                }
                else if (playerstate == PlayerState.Circle)
                {
                    speed += CirclesDeceleration * Time.deltaTime;
                }
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            if (speed < 0)
            {
                if (playerstate == PlayerState.Human)
                {
                    speed += HumansSpeed * Time.deltaTime;
                }
                else if (playerstate == PlayerState.Circle)
                {
                    speed += CirclesSpeed * Time.deltaTime;
                }
                transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
            }
            if(speed > 0)
            {
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
            if(playerstate == PlayerState.Human)
            {
                speed = 0;
            }
        }

        //Spine
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(playerstate == PlayerState.Human)
            {
                anim = playerAnims[2]; //左向き
                playerMeshs[2].enabled = true;
                playerMeshs[0].enabled = false;
                playerMeshs[1].enabled = false;
                anim.SetBool("Change", false);
            }
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (playerstate == PlayerState.Human)
            {
                anim = playerAnims[1]; //右向き
                playerMeshs[1].enabled = true;
                playerMeshs[0].enabled = false;
                playerMeshs[2].enabled = false;
                anim.SetBool("Change", false);
            }
        }

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            if ((!Input.GetKey(KeyCode.F) && playerstate == PlayerState.Human) || playerstate == PlayerState.Circle)
            {
                anim.SetBool("Dash", true);
            }
        }
        if (speed <= 1f && speed >= -1f && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Dash", false);
        }
    }

    private void PlayerJump()
    {
        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            anim.SetBool("Jump", true);
            this.rb.AddForce(transform.up * jumpForce);
            audioSource.PlayOneShot(Jump);
            Debug.Log(jumpForce);
        }
    }

    private void MoveSound()
    {
        if(soundSpan >= 0)
        {
            soundSpan -= Time.deltaTime;
        }

        //球体形態の時に移動中再生
        if (playerstate == PlayerState.Circle && speed != 0 && soundSpan <= 0)
        {
            audioSource.PlayOneShot(Move);
            soundSpan = 0.68f;
        }

        if (run)
        {
            if (speed <= 0.8f && speed >= -0.8f) //スピードが-0.5〜0.5になったらストップ
            {
                audioSource.Stop();
                soundSpan = 0.0f;
            }
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
        Debug.Log("Damage");
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
        if(!invincible) //無敵状態じゃないとき
        {
            if(collision.gameObject.tag == "Enemy")
            {
                //人型のときか、速度が7割以下のとき
                if (!objectBreak)
                {
                    Debug.Log("敵と接触");

                    anim.SetTrigger("Damage");
                    //カプセル状態解除
                    if(playerstate == PlayerState.Circle)
                    {
                        playerstate = PlayerState.Human;
                        playerMeshs[1].enabled = true;
                        playerMeshs[0].enabled = false;
                        playerMeshs[2].enabled = false;
                    }

                    speed -= speed * 0.5f;
                    invincible = true;
                    HpController.Hp--;
                }
            }

            if(collision.gameObject.tag == "Thorn")
            {
                Debug.Log("敵と接触");

                anim.SetTrigger("Damage");
                //カプセル状態解除
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                    playerMeshs[1].enabled = true;
                    playerMeshs[0].enabled = false;
                    playerMeshs[2].enabled = false;
                }

                speed -= speed * 0.5f;
                invincible = true;
                HpController.Hp--;
            }
        }

        if(collision.gameObject.tag == "Box") //箱を破壊
        {
            if (Input.GetKey(KeyCode.D) && objectBreak)
            {
                speed -= speed * 0.2f;
                Debug.Log("箱を破壊");
                //Destroy(collision.gameObject);
            }
            if (Input.GetKey(KeyCode.A) && objectBreak)
            {
                speed -= speed * 0.2f;
                Debug.Log("箱を破壊");
                //Destroy(collision.gameObject);
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
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }

        if (collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Box")
        {
            speed = 0.0f; //壁に当たったら速度をリセット
        }

        //坂に当たったら坂を上るための角度を取得
        if(collision.gameObject.tag == "Slope")
        {
            var slope = collision.gameObject.GetComponent<Slope>();
            angle = slope.Angle;
            rb.gravityScale = 0;
        }

        //人形態の時に箱に接触しているとき箱を押す
        if (playerstate == PlayerState.Human && collision.gameObject.tag == "Box")
        {
            isGround = true;
            Debug.Log("衝突");

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.F))
            {
                if(!anim.GetCurrentAnimatorStateInfo(0).IsName("push") && !anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
                    anim.SetBool("Push", true);

                var obj = collision.gameObject; 
                box = obj.GetComponent<Box>();
                box.BoxRightMove();
                speed = 1.0f;
                audioSource.PlayOneShot(Box);
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.F))
            {
                Debug.Log("test");
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("push") && !anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
                    anim.SetBool("Push", true);

                var obj = collision.gameObject;
                box = obj.GetComponent<Box>();
                box.BoxLeftMove();
                speed = -1.0f;
                audioSource.PlayOneShot(Box);
            }

            if(Input.GetKeyUp(KeyCode.F) || !Input.GetKey(KeyCode.F)) anim.SetBool("Push", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope" ||
            collision.gameObject.tag == "Box")
        {
            isGround = false;
        }

        if(collision.gameObject.tag == "Slope")
        {
            rb.gravityScale = 2;
            angle = 0;
        }

        if(collision.gameObject.tag == "Box")
        {
            anim.SetBool("Push", false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!invincible)
        {
            if (collision.gameObject.tag == "Thorn" ||
                collision.gameObject.tag == "Poison")
            {
                anim.SetTrigger("Damage");
                //カプセル状態解除
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                }

                speed -= speed * 0.5f;
                invincible = true;
                HpController.Hp--;
            }
        }
    }

    IEnumerator ToBall()
    {
        yield return new WaitForSeconds(1f);

        anim = playerAnims[0];
        playerMeshs[0].enabled = true;
        playerMeshs[1].enabled = false;
        playerMeshs[2].enabled = false;

        playerstate = PlayerState.Circle;
        //sr.sprite = Circles;
        Debug.Log("球体です");
    }

    IEnumerator ToHuman()
    {
        yield return new WaitForEndOfFrame();

        anim = playerAnims[1];
        playerMeshs[1].enabled = true;
        playerMeshs[0].enabled = false;
        playerMeshs[2].enabled = false;

        anim.SetBool("Change", false);
        

        playerstate = PlayerState.Human;
        //sr.sprite = Humans;
        Debug.Log("人です");
    }
}
