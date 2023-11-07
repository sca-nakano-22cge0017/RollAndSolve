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

    [Header("�����x")]
    [SerializeField] private float HumansAccelertion; //�l�`�Ԃ̎��̉����x
    [SerializeField] private float CirclesAccelertion; //���̌`�Ԃ̎��̉����x

    private float HumansSpeed = 0.0f;
    private float CirclesSpeed = 0.0f;

    private float HumansSpeedUp = 0.0f;
    private float CirclesSpeedUp = 0.0f;

    [Header("�ō����x")]
    [SerializeField] private float MaxSpeed;

    [Header("�����x")]
    [SerializeField] private float deceleration;
    bool RightDeceleration = false;
    bool LeftDeceleration  = false;

    [Header("�W�����v��")]
    [SerializeField] private float HumansJump = 400f; //�l�`�Ԃ̂Ƃ��̃W�����v��
    [SerializeField] private float CirclesJump = 300f; //���̌`�Ԃ̂Ƃ��̃W�����v��
    private float jumpForce;

    bool speedUp = false;
    float speedUpCount = 7.0f; //�X�s�[�h�A�b�v�̃A�C�e������������̏㏸���鎞��

    bool isGround = false;

    [Header("�v���C���[�̉摜")]
    [SerializeField] Sprite Humans;
    [SerializeField] Sprite Circles;
    SpriteRenderer sr;

    bool invincible = false; //���G���
    float invincibleTime = 3.0f; //���G����
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
        HumansSpeed = HumansAccelertion; //���x������
        CirclesSpeed = CirclesAccelertion; //���x������

        HumansSpeedUp = HumansAccelertion * 1.2f; //�X�s�[�h�A�b�v�������̑��x
        CirclesSpeedUp = CirclesAccelertion * 1.2f; //�X�s�[�h�A�b�v�������̑��x
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
                    Debug.Log("���̂ł�");
                }
                    Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Human;
                    sr.sprite = Humans;
                    Debug.Log("�l�ł�");
                }
                Circle();
                break;
        }

        if (invincible) //���G���
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

        if(position.y < -5.5) //���ɗ�������
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
            Debug.Log("D�𗣂�");
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
            Debug.Log("A�𗣂�");
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

        //�W�����v
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
            //���x��1.2�{�ɂȂ�
            HumansSpeed = HumansSpeedUp;
            CirclesSpeed = CirclesSpeedUp;
        }
        else if (speedUpCount < 0)
        {
            //�J�E���g��0�ɂȂ����瑬�x�����̖߂�
            HumansSpeed = HumansAccelertion;
            CirclesSpeed = CirclesAccelertion;

            speedUpCount = 7.0f;
            speedUp = false;
        }
    }

    private void Invincible() //���G���
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

        if(collision.gameObject.tag == "Enemy" && !invincible) //���G��Ԃ���Ȃ��Ƃ��ɓG�ƐڐG
        {
            Debug.Log("�G�ƐڐG");
            speed -= speed * 0.5f;
            invincible = true;
            //HpController.Hp--;
        }

        if(collision.gameObject.tag == "Box" && playerstate == PlayerState.Circle) //����j��
        {
            if (Input.GetKey(KeyCode.D) && speed >= 7.0f)
            {
                speed -= speed * 0.2f;
                Debug.Log("����j��");
                //Destroy(collision.gameObject);
            }
            if (Input.GetKey(KeyCode.A) && speed <= -7.0f)
            {
                speed -= speed * 0.2f;
                Debug.Log("����j��");
               // Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "SpeedUP") //�X�s�[�h�A�b�v
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
            speed = 0.0f; //�ǂɓ��������瑬�x�����Z�b�g
        }

        //�l�`�Ԃ̎��ɔ��ɐڐG���Ă���Ƃ���������
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
