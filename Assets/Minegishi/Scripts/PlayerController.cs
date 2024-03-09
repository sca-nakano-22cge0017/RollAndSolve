using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Box box;
    HPController HpController;

    //�A�j���[�V����
    Animator anim;
    [SerializeField] GameObject[] playerForms;
    [SerializeField] Animator[] playerAnims;
    [SerializeField] MeshRenderer[] playerMeshs;
    ChangeAnimEnd changeAnimEnd;

    public enum PlayerState { Human, Circle}
    public PlayerState playerstate;

    //�|�[�Y��Ԃ̂Ƃ�true
    bool isPause = false;
    public bool IsPause { get { return isPause;} set { isPause = value;} }

    //�J�E���g�_�E���I���̃t���O
    bool countEnd = false;
    public bool CountEnd { set { countEnd = value; } }

    private Rigidbody2D rb;

    //���o�鎞�Ɏg�p����@�ړ��p�x
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

    bool isRight = false;
    bool isLeft = false;

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

    [SerializeField, Header("�ϐg�G�t�F�N�g")] ParticleSystem changeEffect;
    [SerializeField, Header("���G�t�F�N�g")] ParticleSystem windEffect;

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

        changeAnimEnd = playerForms[1].GetComponent<ChangeAnimEnd>();
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

        if(HpController.IsDown) anim.SetBool("Dead", true); //HP0�ɂȂ�����_�E���A�j���[�V�����Đ�

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

        //���̌`�Ԃōō����x�̂V���ȏ�̎��I�u�W�F�N�g��j��ł���
        if(Mathf.Abs(speed) >= CirclesMaxSpeed * 0.7 && playerstate == PlayerState.Circle)
        {
            objectBreak = true;
            windEffect.Play(); //�ړ��G�t�F�N�g
        }
        else
        {
            objectBreak = false;
            windEffect.Stop();
        }

        if(isPause) windEffect.Stop();

        //�J�E���g�_�E������AD�L�[��������A���̂܂܃Q�[�����J�n����ƍŏ������Ȃ��̂ł���̉���
        //�J�E���g�_�E�����I����Ă��Ȃ��Ƃ�
        if (!countEnd)
        {
            //AD�L�[�������ꂽ�獶�E�����ֈړ��\�ɂ��Ă���
            if (Input.GetKeyDown(KeyCode.D))
            {
                isRight = true;
                isLeft = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                isRight = false;
                isLeft = true;
            }
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
                    anim.SetBool("Dash", false);
                    anim.SetBool("Push", false);

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

        // �E�����ֈړ�
        // �������u�ԂɈړ������㏑��
        if (Input.GetKeyDown(KeyCode.D))
        {
            isRight = true;
            isLeft = false;
            if(speed < 0)
                speed = 0;
        }

        //D�L�[���͂��ړ��������E
        if (Input.GetKey(KeyCode.D) && isRight)
        {
            //�G�t�F�N�g�̌����ύX
            windEffect.transform.localPosition = new Vector3(-7.0f, -6.5f, 0);
            windEffect.transform.rotation = Quaternion.Euler(angle, -90.0f, 90.0f);

            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                if (speed <= -3)
                {
                    speed = -Brake;
                }
                speed += HumansSpeed * Time.deltaTime;

                //���x���
                if (speed >= HumansMaxSpeed)
                {
                    speed = HumansMaxSpeed;
                }
            }
            else if(playerstate == PlayerState.Circle)
            {
                speed += CirclesSpeed * Time.deltaTime;

                //���x���
                if (speed > CirclesMaxSpeed)
                {
                    speed += CirclesDeceleration * Time.deltaTime;
                }
                if (speed >= CirclesMaxSpeed)
                {
                    speed = CirclesMaxSpeed;
                }

                playerForms[0].GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }
            //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0,0) * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            //����
            RightDeceleration = true;

            isRight = false;
            isLeft = true;

            //�����]��
            if (Input.GetKey(KeyCode.A) && speed > 0) speed = 0;
        }

        // �E��������
        if (RightDeceleration)
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

                //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
            }
            else speed = 0; //���ߕ���߂�
        }

        // �������ֈړ� 
        // �������u�ԂɈړ������㏑��
        if (Input.GetKeyDown(KeyCode.A))
        {
            isLeft = true;
            isRight = false;
            if(speed > 0)
                speed = 0;
        }

        if (Input.GetKey(KeyCode.A) && isLeft)
        {
            //�G�t�F�N�g�̌����ύX
            windEffect.transform.localPosition = new Vector3(7.0f, -6.5f, 0);
            windEffect.transform.rotation = Quaternion.Euler(-angle, 90.0f, -90.0f);

            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                if (speed >= 3)
                {
                    speed = Brake;
                }
                speed -= HumansSpeed * Time.deltaTime;

                //���x���
                if (speed <= -HumansMaxSpeed)
                {
                    speed = -HumansMaxSpeed;
                }
            }
            else if (playerstate == PlayerState.Circle)
            {
                speed -= CirclesSpeed * Time.deltaTime;

                //���x���
                if (speed < CirclesMaxSpeed)
                {
                    speed -= CirclesDeceleration * Time.deltaTime;
                }
                if (speed <= -CirclesMaxSpeed)
                {
                    speed = -CirclesMaxSpeed;
                }

                playerForms[0].GetComponent<Transform>().localScale = new Vector3(-1f, 1f, 1f);
            }
            //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            LeftDeceleration = true;
            isRight = true;
            isLeft = false;

            //�����]��
            if (Input.GetKey(KeyCode.D) && speed < 0) speed = 0;
        }

        // ����������
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

                //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
            }
            else speed = 0;
        }

        //�������ɏ�����Ă����̂��܂Ƃ߂�
        transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);

        //if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        //{
        //    //�������Ɉړ����Ă�����
        //    if (speed < 0)
        //    {
        //        //�t�����ɑ��x�ǉ�
        //        if (playerstate == PlayerState.Human)
        //        {
        //            speed += HumansSpeed * Time.deltaTime;
        //        }
        //        else if (playerstate == PlayerState.Circle)
        //        {
        //            speed += CirclesSpeed * Time.deltaTime;
        //        }
        //        transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
        //    }
        //    if (speed > 0)
        //    {
        //        if (playerstate == PlayerState.Human)
        //        {
        //            speed -= HumansSpeed * Time.deltaTime;
        //        }
        //        else if (playerstate == PlayerState.Circle)
        //        {
        //            speed -= CirclesSpeed * Time.deltaTime;
        //        }
        //        transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
        //    }
        //    if (playerstate == PlayerState.Human)
        //    {
        //        speed = 0;
        //    }
        //}

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
    }

    //�v���C���[�C���X�g�ʒu�����p�ϐ�
    Vector3 pushAjustR = new Vector3(5.0f, -3.0f, 0); //�ؔ��������Ă���Ƃ�
    Vector3 pushAjustL = new Vector3(-5.0f, -3.0f, 0);
    Vector3 dashAjust = new Vector3(0, -2.0f, 0); //�����Ă���Ƃ�
    Vector3 normalAjust = new Vector3(0, -1.0f, 0); //�ʏ���

    /// <summary>
    /// �ꕔSpine�̐���
    /// </summary>
    void Spine()
    {
        //�v���C���[�ʒu�����@Spine�̓s���㕂���Č�����̂� ���E�������ꂼ��̃C���X�g���ʒu��������
        for(int i = 1; i <= 2; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("push") || anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
            {
                if(speed >= 0)
                    playerForms[i].transform.localPosition = pushAjustR;
                else playerForms[i].transform.localPosition = pushAjustL;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("dash") || anim.GetCurrentAnimatorStateInfo(0).IsName("dash_motion"))
            {
                playerForms[i].transform.localPosition = dashAjust;
            }
            else
            {
                playerForms[i].transform.localPosition = normalAjust;
            }
        }

        //AD�L�[�������ꂽ�Ƃ��A�A�j���[�V�����E�\���C���X�g�����E�؂�ւ���
        if(playerstate == PlayerState.Human)
        {
            //�L�[���������u�ԁA�t�����̃L�[��������Ă��Ȃ���Ԃ̂Ƃ�
            if (Input.GetKeyDown(KeyCode.A) || (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                AnimFlipped("left");
            }
            if (Input.GetKeyDown(KeyCode.D) || (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)))
            {
                AnimFlipped("right");
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

        //��葬�x�ȉ��ŃA�j���[�V�������~����
        float animMinSpeed = 5.0f;
        if (speed <= animMinSpeed && speed >= -animMinSpeed && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Dash", false);
        }

        //���x�ɍ��킹�đ��胂�[�V�����̑��x���㏸
        anim.SetFloat("Speed", Mathf.Abs(speed) * 0.1f + 1);
    }

    /// <summary>
    /// �l�^�A�j���[�V�����̍��E���]
    /// </summary>
    /// <param name="key">���͂��ꂽ�L�[</param>
    void AnimFlipped(string leftOrRight)
    {
        int lastAnim = 0; //�O�̃A�j���[�V����
        int nextAnim = 0; //���̃A�j���[�V����

        switch (leftOrRight)
        {
            //���ړ�
            case "left":
                lastAnim = 1;
                nextAnim = 2;
                break;
            //�E�ړ�
            case "right":
                lastAnim = 2;
                nextAnim = 1;
                break;
        }

        //�A�j���[�V������������Ԃɂ���
        playerAnims[lastAnim].SetBool("Change", false);
        playerAnims[lastAnim].SetBool("Jump", false);
        playerAnims[lastAnim].SetBool("Dash", false);

        anim = playerAnims[nextAnim]; //�t�����̃A�j���[�V�����𑀍�ł���悤�ɕύX
        playerMeshs[nextAnim].enabled = true; //�t�����̃C���X�g�ɂ���
        changeAnimEnd = playerForms[nextAnim].GetComponent<ChangeAnimEnd>(); //�A�j���[�V�����I�������Ⴄ�X�N���v�g��ς���

        //���̃C���X�g�͔�\��
        playerMeshs[0].enabled = false;
        playerMeshs[lastAnim].enabled = false;
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
        //���n�A�j���[�V����
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }

        if (collision.gameObject.tag == "Ground")
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

        if (collision.gameObject.tag == "Slope")
        {
            var slope = collision.gameObject.GetComponent<Slope>();
            angle = slope.Angle;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
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

        if (collision.gameObject.tag == "Slope")
        {
            rb.gravityScale = 2;
            angle = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible) //���G��Ԃ���Ȃ��Ƃ�
        {
            //�Փˑ��肪�G���l�^�����x�V�������̂Ƃ��@�܂��͏Փˑ��肪�g�Q�̂Ƃ��A�_���[�W����
            if ((collision.gameObject.tag == "Enemy" && !objectBreak) || collision.gameObject.tag == "Thorn")
            {
                //�J�v�Z����ԉ���
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                    anim = playerAnims[1];
                    playerMeshs[1].enabled = true;
                    playerMeshs[0].enabled = false;
                    playerMeshs[2].enabled = false;
                }

                speed -= speed * 0.5f;
                invincible = true;
                HpController.IsDamage = true;

                anim.SetTrigger("Damage");
            }
        }

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
        //�A�j���[�V�������I���܂őҋ@
        yield return new WaitUntil(() => changeAnimEnd.IsEnd);

        //����->�l�^�ɂȂ����Ƃ��A�ό`���[�V�����̍Đ�����n�܂�Ȃ��悤��
        playerAnims[1].SetBool("Change", false);
        playerAnims[2].SetBool("Change", false);

        //���삷��A�j���[�V������ύX����
        anim = playerAnims[0];

        playerMeshs[0].enabled = true;
        playerMeshs[1].enabled = false;
        playerMeshs[2].enabled = false;

        playerstate = PlayerState.Circle;
        //sr.sprite = Circles;
        Debug.Log("���̂ł�");
    }

    IEnumerator ToHuman()
    {
        //�t�H�[���`�F���W�A�j���[�V�����I���t���O��false�ɂ���
        changeAnimEnd.IsEnd = false;

        //�G�t�F�N�g�Đ�
        changeEffect.Play();

        //�G�t�F�N�g�Đ����ҋ@
        yield return new WaitForSeconds(0.7f);

        //���삷��A�j���[�V������ύX����
        anim = playerAnims[1];

        playerMeshs[1].enabled = true;
        playerMeshs[0].enabled = false;
        playerMeshs[2].enabled = false;

        playerstate = PlayerState.Human;
        //sr.sprite = Humans;
        Debug.Log("�l�ł�");
    }
}
