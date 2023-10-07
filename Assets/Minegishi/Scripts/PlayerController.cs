using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Human, Circle}
    PlayerState playerstate;

    private Rigidbody2D rb;

    private float speed = 1.0f;
    [SerializeField] private float HumansSpeed = 1.0f; //�l�`�Ԃ̂Ƃ��̃X�s�[�h
    [SerializeField] private float CirclesSpeed = 3.0f; //���̌`�Ԃ̎��̃X�s�[�h

     private float jumpForce = 3f;
    [SerializeField] private float HumansJump = 400f; //�l�`�Ԃ̂Ƃ��̃W�����v��
    [SerializeField] private float CirclesJump = 300f; //���̌`�Ԃ̂Ƃ��̃W�����v��

    [SerializeField] private float accelertion = 1f; 

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
                    speed = CirclesSpeed;
                }
                    Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Human;
                    speed = HumansSpeed;
                }
                Circle();
                break;
        }


    }

    void Human()
    {
        //speed = HumansSpeed;
        jumpForce = HumansJump;
        //Debug.Log("�l�ł�");
    }

    void Circle()
    {
        //speed = CirclesSpeed;
        jumpForce = CirclesJump;
        //Debug.Log("���̂ł�");
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.D))
        {
            speed += accelertion * Time.deltaTime;
            Debug.Log(speed);
            transform.Translate(new Vector3(speed, 0,0) * Time.deltaTime) ;
        }
        else if (Input.GetKeyUp(KeyCode.D))//�{�^���𗣂����Ƃ��ɉ����x��������
        {
            if(playerstate == PlayerState.Human)
            {
                speed = HumansSpeed;
            }
            else if(playerstate == PlayerState.Circle)
            {
                speed = CirclesSpeed;
            }
        }


        if (Input.GetKey(KeyCode.A))
        {
            speed += accelertion * Time.deltaTime;
            transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.A)) //�{�^���𗣂����Ƃ��ɉ����x��������
        {
            if (playerstate == PlayerState.Human)
            {
                speed = HumansSpeed;
            }
            else if (playerstate == PlayerState.Circle)
            {
                speed = CirclesSpeed;
            }
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
