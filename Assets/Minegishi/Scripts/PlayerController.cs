using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Box box;
    HPController HpController;
    Animator anim;
    [SerializeField] GameObject[] playerForms;
    [SerializeField] Animator[] playerAnims;
    [SerializeField] MeshRenderer[] playerMeshs;

    public enum PlayerState { Human, Circle}
    public PlayerState playerstate;

    //�|�[�Y���
    bool isPause = false;
    public bool IsPause { get { return isPause;} set { isPause = value;} }

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

    [Header("�����x")]
    [SerializeField] private float HumansAccelertion; //�l�`�Ԃ̎��̉����x
    [SerializeField] private float CirclesAccelertion; //���̌`�Ԃ̎��̉����x

    private float HumansSpeed = 0.0f;
    private float CirclesSpeed = 0.0f;

    private float HumansSpeedUp = 0.0f;
    private float CirclesSpeedUp = 0.0f;

    [Header("�ō����x")]
    [SerializeField] private float HumansMaxSpeed;
    [SerializeField] private float CirclesMaxSpeed;

    [Header("�����x")]
    [SerializeField] private float HumansDeceleration;
    [SerializeField] private float CirclesDeceleration;
    bool RightDeceleration = false;
    bool LeftDeceleration  = false;

    [Header("�l�^�u���[�L")]
    [SerializeField] private float Brake;

    [Header("�W�����v��")]
    [SerializeField] private float HumansJump = 400f; //�l�`�Ԃ̂Ƃ��̃W�����v��
    [SerializeField] private float CirclesJump = 300f; //���̌`�Ԃ̂Ƃ��̃W�����v��
    private float jumpForce;

    bool speedUp = false;
    float speedUpCount = 7.0f; //�X�s�[�h�A�b�v�̃A�C�e������������̏㏸���鎞��
    [SerializeField, Header("�X�s�[�h�A�b�v�{��")] float speedUpNum = 1.2f; 

    bool isGround = false;
    
    [Header("�T�E���h")]
    [SerializeField] AudioClip Move;
    [SerializeField] AudioClip Jump;
    [SerializeField] AudioClip Box;
    AudioSource audioSource;
    float soundSpan = 0.0f;
    bool run = false;
    
    bool invincible = false; //���G���
    float invincibleTime = 3.0f; //���G����
    //int alpha = 255;
    float interval = 0.15f;

    //�ؔ�������
    bool isPushing = false; //�ؔ��������Ă���Œ��Ȃ�true
    [SerializeField, Header("�ؔ��������Ƃ��̒������K�v����")] float pushTime = 0.2f;
    float pTime = 0;
    bool isPushCount = false;

    void Start()
    {
        this.HpController = FindObjectOfType<HPController>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        playerstate = PlayerState.Circle;
        HumansSpeed = HumansAccelertion; //���x������
        CirclesSpeed = CirclesAccelertion; //���x������

        HumansSpeedUp = HumansAccelertion * speedUpNum; //�X�s�[�h�A�b�v�������̑��x
        CirclesSpeedUp = CirclesAccelertion * speedUpNum; //�X�s�[�h�A�b�v�������̑��x

        //Spine
        anim = playerAnims[0];
        //���݂̌`�ԈȊO��MeshRenderer���I�t�ɂ��Ĕ�\���ɂ���
        playerMeshs[0].enabled = true; //�J�v�Z��
        playerMeshs[1].enabled = false; //�E�����l�^
        playerMeshs[2].enabled = false; //�������l�^
    }

    void Update()
    {
        //Hp��0����Ȃ��Ƃ��@�|�[�Y��Ԃ���Ȃ��Ƃ�
        if (!HpController.IsDown && !isPause)
        {
            Run();
            FormChange();
            Push();
            PlayerJump();
            MoveSound();
        }
        else if(HpController.IsDown) anim.SetTrigger("Dead");
        
        if (speedUp)
        {
            SpeedUp();
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
            }
        }

        Vector2 position = transform.position;

        transform.position = position;

        //���̌`�Ԃōō����x�̂V���ȏ�̎��I�u�W�F�N�g��j��ł���
        if(Mathf.Abs(speed) >= CirclesMaxSpeed * 0.7 && playerstate == PlayerState.Circle)
        {
            objectBreak = true;
        }
        else
        {
            objectBreak = false;
        }
    }

    //�l�^
    void Human()
    {
        //speed = HumansSpeed;
        jumpForce = HumansJump;
    }

    //����
    void Circle()
    {
        //speed = CirclesSpeed;
        jumpForce = CirclesJump;
    }

    /// <summary>
    /// �ό` W�L�[�Ől�^<->����
    /// </summary>
    void FormChange()
    {
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
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    void Run()
    {
        if(speed >= 1.0f || speed <= -1.0f)
        {
            run = true;
        }

        if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A))) //D������
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
            //Debug.Log("D�𗣂�");
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

        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D))) //A������
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
            transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
            
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
            //Debug.Log("A�𗣂�");
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
                transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
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

        if(!isGround)
        {
            angle = 0;
        }

        Spine();
    }

    /// <summary>
    /// �ؔ��������A�j���[�V�����̍Đ��E��~�A�ؔ��̈ړ�
    /// </summary>
    void Push()
    {
        if (isPushing)
        {
            //�����Ă���or�����n�߂̃A�j���[�V�������Đ����łȂ����
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("push") && !anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
                anim.SetBool("Push", true); //�����n�߂̃A�j���[�V�����ɑJ��

            //�ؔ��ړ�
            if (Input.GetKey(KeyCode.D) && box != null)
            {
                box.BoxRightMove();
            }

            if(Input.GetKey(KeyCode.A) && box != null)
            {
                box.BoxLeftMove();
            }

            //�ؔ����������SE
            if (soundSpan >= 0)
            {
                soundSpan -= Time.deltaTime;
            }

            if (soundSpan <= 0.0f)
            {
                audioSource.PlayOneShot(Box);
                soundSpan = 1.752f;
            }
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("push") || anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
                anim.SetBool("Push", false);
        }

        if(isPushCount)
        {
            pTime += Time.deltaTime;

            if (pTime >= pushTime) isPushing = true;
            else isPushing = false;
        }
        else pTime = 0;

        //�L�[����肪����Ă���Ƃ� �܂���A��D�𗼕������Ă���Ƃ��ؔ��������Ȃ�
        if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            isPushCount = false;
            isPushing = false;
            box = null;
            anim.SetBool("Push",false);
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (playerstate == PlayerState.Human)
            {
                anim = playerAnims[2]; //�������̃A�j���[�V����
                playerMeshs[2].enabled = true;
                playerMeshs[0].enabled = false;
                playerMeshs[1].enabled = false;
            }
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            if (playerstate == PlayerState.Human)
            {
                anim = playerAnims[1]; //�E�����̃A�j���[�V����
                playerMeshs[1].enabled = true;
                playerMeshs[0].enabled = false;
                playerMeshs[2].enabled = false;
            }
        }
    }

    /// <summary>
    /// �ꕔSpine�̐���
    /// </summary>
    void Spine()
    {
        //�v���C���[�ʒu�����@Spine�̓s���㕂���Č�����̂�
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("push") || anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
        {
            playerForms[1].transform.localPosition = new Vector3(0, -3, 0);
            playerForms[2].transform.localPosition = new Vector3(0, -3, 0);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("dash") || anim.GetCurrentAnimatorStateInfo(0).IsName("dash_motion"))
        {
            playerForms[1].transform.localPosition = new Vector3(0, -2, 0);
            playerForms[2].transform.localPosition = new Vector3(0, -2, 0);
        }
        else
        {
            playerForms[1].transform.localPosition = new Vector3(0, -1, 0);
            playerForms[2].transform.localPosition = new Vector3(0, -1, 0);
        }

        
        //Spine �L�[�������ꂽ�u�ԃA�j���[�V�����E�\���C���X�g��؂�ւ���
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (playerstate == PlayerState.Human)
            {
                anim = playerAnims[2]; //�������̃A�j���[�V����
                playerMeshs[2].enabled = true;
                playerMeshs[0].enabled = false;
                playerMeshs[1].enabled = false;

                anim.SetBool("Change", false);
                anim.SetBool("Jump", false);

            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (playerstate == PlayerState.Human)
            {
                anim = playerAnims[1]; //�E�����̃A�j���[�V����
                playerMeshs[1].enabled = true;
                playerMeshs[0].enabled = false;
                playerMeshs[2].enabled = false;

                anim.SetBool("Change", false);
                anim.SetBool("Jump", false);
            }
        }

        //���胂�[�V����
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
            {
                anim.SetBool("Dash", true);
            }
        }
        if (speed <= 5f && speed >= -5f && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Dash", false);
        }

        //���x�ɍ��킹�đ��胂�[�V�����̑��x���㏸
        anim.SetFloat("Speed", Mathf.Abs(speed) * 0.1f + 1);
    }

    /// <summary>
    /// �W�����v
    /// </summary>
    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            anim.SetBool("Jump", true);
            this.rb.AddForce(transform.up * jumpForce);
            audioSource.PlayOneShot(Jump);
            //Debug.Log(jumpForce);
        }
    }

    /// <summary>
    /// �ړ���
    /// </summary>
    private void MoveSound()
    {
        //�Đ����I��������܂��Đ�
        if(soundSpan >= 0)
        {
            soundSpan -= Time.deltaTime;
        }

        //���̌`�Ԃ̎��Ɉړ����Đ�
        if (playerstate == PlayerState.Circle && speed != 0 && soundSpan <= 0)
        {
            audioSource.PlayOneShot(Move);
            soundSpan = 0.68f;
        }

        if (run)
        {
            if (speed <= 0.8f && speed >= -0.8f) //�X�s�[�h��-0.5�`0.5�ɂȂ�����X�g�b�v
            {
                audioSource.Stop();
                soundSpan = 0.0f;
            }
        }

    }

    /// <summary>
    /// ���x�㏸�A�C�e���擾���̏���
    /// </summary>
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

    /// <summary>
    /// ���G���
    /// </summary>
    private void Invincible()
    {
        interval -= Time.deltaTime;
        
        if(interval <= 0)
        {
            //if(alpha == 255)
            //{
            //    alpha = 0;
            //}else
            //    alpha = 255;
            interval = 0.15f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!invincible) //���G��Ԃ���Ȃ��Ƃ�
        {
            //�Փˑ��肪�G���l�^�����x�V�������@�܂��͏Փˑ��肪�g�Q�̂Ƃ��A�_���[�W����
            if((collision.gameObject.tag == "Enemy" && !objectBreak) || collision.gameObject.tag == "Thorn")
            {
                anim.SetTrigger("Damage");

                //�J�v�Z����ԉ���
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                    playerMeshs[1].enabled = true;
                    playerMeshs[0].enabled = false;
                    playerMeshs[2].enabled = false;
                }

                speed -= speed * 0.5f;
                invincible = true;
                HpController.IsDamage = true;
            }
        }

        //���n�A�j���[�V����
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }

        if(collision.gameObject.tag == "Ground")
        {
            angle = 0;
        }

        if (collision.gameObject.tag == "Box")
        {
            //����j��
            if (Input.GetKey(KeyCode.D) && objectBreak)
            {
                speed -= speed * 0.2f;
                //Debug.Log("����j��");
                //Destroy(collision.gameObject);
            }
            if (Input.GetKey(KeyCode.A) && objectBreak)
            {
                speed -= speed * 0.2f;
                //Debug.Log("����j��");
                //Destroy(collision.gameObject);
            }

            //���n�A�j���[�V���� �ؔ��̏�ł̒��n����
            foreach (ContactPoint2D contact in collision.contacts)
            {
                var hitPoint = contact.point;
                var sub = hitPoint.y - transform.position.y;

                if (sub < -0.7f)
                {
                    isGround = true;
                    anim.SetBool("Jump", false);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = true;
        }

        if (collision.gameObject.tag == "Wall")
        {
            speed = 0.0f; //�ǂɓ��������瑬�x�����Z�b�g
        }

        //��ɓ�������������邽�߂̊p�x���擾
        if (collision.gameObject.tag == "Slope")
        {
            var slope = collision.gameObject.GetComponent<Slope>();
            angle = slope.Angle;
            rb.gravityScale = 0;
        }

        if (collision.gameObject.tag == "Box")
        {
            foreach(ContactPoint2D contact in collision.contacts)
            {
                //�Փˈʒu���擾
                var hitPoint = contact.point;
                var sub = hitPoint.y - transform.position.y;

                //���E�ɖؔ�����������
                if (sub <= 0.7f && sub >= -0.7f)
                {
                    //���E�����łԂ�������~�܂�
                    speed = 0.0f;

                    //�l�`�Ԃ̎��ɔ��ɐڐG���Ă���Ƃ�
                    if (playerstate == PlayerState.Human)
                    {
                        //D�������ŉE�ɖؔ�������
                        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                        {
                            isPushCount = true;

                            var obj = collision.gameObject;
                            box = obj.GetComponent<Box>();
                            speed = 1.0f;
                        }

                        //A�������ō��ɖؔ�������
                        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                        {
                            isPushCount = true;

                            var obj = collision.gameObject;
                            box = obj.GetComponent<Box>();
                            speed = -1.0f;
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = false;
        }

        if(collision.gameObject.tag == "Box")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                var hitPoint = contact.point;
                var sub = hitPoint.y - transform.position.y;

                //�ؔ��̏�ɗ����Ă����ꍇ
                if (sub < -0.7f)
                {
                    isGround = false;
                }
            }

            isPushing = false;
            isPushCount = false;
            anim.SetBool("Push", false);
        }

        if(collision.gameObject.tag == "Slope")
        {
            rb.gravityScale = 2;
            //angle = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���ɗ�����
        if (collision.gameObject.tag == "Hole")
        {
            HpController.IsFall = true;
        }

        if (collision.gameObject.tag == "SpeedUP") //�X�s�[�h�A�b�v
        {
            //Destroy(collision.gameObject);
            speedUpCount = 7.0f;
            speedUp = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!invincible)
        {
            if (collision.gameObject.tag == "Poison")
            {
                anim.SetTrigger("Damage");
                //�J�v�Z����ԉ���
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                }

                speed -= speed * 0.5f;
                invincible = true;
                HpController.IsDamage = true;
            }
        }
    }

    IEnumerator ToBall()
    {
        isPause = true;

        yield return new WaitForSeconds(1f);

        anim = playerAnims[0];
        playerMeshs[0].enabled = true;
        playerMeshs[1].enabled = false;
        playerMeshs[2].enabled = false;

        isPause = false;

        playerstate = PlayerState.Circle;
        //sr.sprite = Circles;
        Debug.Log("���̂ł�");
    }

    IEnumerator ToHuman()
    {
        isPause = true;

        yield return new WaitForEndOfFrame();

        anim = playerAnims[1];
        playerMeshs[1].enabled = true;
        playerMeshs[0].enabled = false;
        playerMeshs[2].enabled = false;

        anim.SetBool("Change", false);

        isPause = false;

        playerstate = PlayerState.Human;
        //sr.sprite = Humans;
        Debug.Log("�l�ł�");
    }
}
