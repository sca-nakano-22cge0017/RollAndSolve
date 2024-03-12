using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! �S���ӏ������p�̃R�s�[
//! �u�S���ӏ�---�v�`�u---�S���ӏ��v�Ŋ����Ă��镔�����S����������
public class PlayerController_Copy : MonoBehaviour
{
    Box box;
    HPController HpController;

    public enum PlayerState { Human, Circle }
    public PlayerState playerstate;

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
        get { return objectBreak; }
        set { objectBreak = value; }
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
    bool LeftDeceleration = false;

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

    //!�S���ӏ�---
    //�A�j���[�V����
    Animator anim;
    [SerializeField] GameObject[] playerForms;
    [SerializeField] Animator[] playerAnims;
    [SerializeField] MeshRenderer[] playerMeshs;
    ChangeAnimEnd changeAnimEnd;

    //�|�[�Y��Ԃ̂Ƃ�true
    bool isPause = false;
    public bool IsPause { get { return isPause; } set { isPause = value; } }

    //�J�E���g�_�E���I���̃t���O
    bool countEnd = false;
    public bool CountEnd { set { countEnd = value; } }

    //�ؔ�������
    bool isPushing = false; //�ؔ��������Ă���Œ��Ȃ�true
    [SerializeField, Header("�ؔ��������Ƃ��̒������K�v����")] float pushTime = 0.2f;
    float pTime = 0;
    bool isPushCount = false;

    [SerializeField, Header("�ϐg�G�t�F�N�g")] ParticleSystem changeEffect;
    [SerializeField, Header("���G�t�F�N�g")] ParticleSystem windEffect;
    //!---�S���ӏ�

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

        //!�S���ӏ�---
        //Spine
        anim = playerAnims[0];
        //���݂̌`�ԈȊO��MeshRenderer���I�t�ɂ��Ĕ�\���ɂ���
        playerMeshs[0].enabled = true; //�J�v�Z��
        playerMeshs[1].enabled = false; //�E�����l�^
        playerMeshs[2].enabled = false; //�������l�^

        changeAnimEnd = playerForms[1].GetComponent<ChangeAnimEnd>();
        //!---�S���ӏ�
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

        if (HpController.IsDown) anim.SetBool("Dead", true); //HP0�ɂȂ�����_�E���A�j���[�V�����Đ�

        if (speedUp)
        {
            SpeedUp();
        }

        if (invincible) //���G���
        {
            invincibleTime -= Time.deltaTime;
            if (invincibleTime > 0)
            {
                Invincible();
            }
            else if (invincibleTime <= 0)
            {
                invincibleTime = 3.0f;
                invincible = false;
            }
        }

        //���̌`�Ԃōō����x�̂V���ȏ�̎��I�u�W�F�N�g��j��ł���
        if (Mathf.Abs(speed) >= CirclesMaxSpeed * 0.7 && playerstate == PlayerState.Circle)
        {
            objectBreak = true;

            //!�S���ӏ�---
            windEffect.Play(); //�ړ��G�t�F�N�g�Đ�
            //!---�S���ӏ�
        }
        else
        {
            objectBreak = false;

            //!�S���ӏ�---
            windEffect.Stop(); //�ړ��G�t�F�N�g��~
            //!---�S���ӏ�
        }

        //!�S���ӏ�---
        //�|�[�Y��Ԃ̂Ƃ��̓G�t�F�N�g��~
        if (isPause)
        {
            windEffect.Stop();
            changeEffect.Stop();
        }

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
        //!---�S���ӏ�
    }

    //�l�^
    void Human()
    {
        jumpForce = HumansJump;
    }

    //����
    void Circle()
    {
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
                    //!�S���ӏ�---
                    anim.SetBool("Change", true);
                    anim.SetBool("Dash", false);
                    anim.SetBool("Push", false);
                    //!---�S���ӏ�

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
        if (speed >= 1.0f || speed <= -1.0f)
        {
            run = true;
        }

        //!�S���ӏ�---
        // �E�����ֈړ�
        // �������u�ԂɈړ������㏑��
        if (Input.GetKeyDown(KeyCode.D))
        {
            isRight = true;
            isLeft = false;
            if (speed < 0)
                speed = 0;
        }
        //!---�S���ӏ�

        //D�L�[���͂��ړ��������E
        if (Input.GetKey(KeyCode.D) && isRight)
        {
            //!�S���ӏ�---
            //�G�t�F�N�g�̌����ύX
            windEffect.transform.localPosition = new Vector3(-7.0f, -6.5f, 0);
            windEffect.transform.rotation = Quaternion.Euler(angle, -90.0f, 90.0f);
            //!---�S���ӏ�

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
            else if (playerstate == PlayerState.Circle)
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
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            //����
            RightDeceleration = true;

            //!�S���ӏ�---
            isRight = false;
            isLeft = true;

            //�����]��
            if (Input.GetKey(KeyCode.A) && speed > 0) speed = 0;
            //!---�S���ӏ�
        }

        // �E��������
        if (RightDeceleration)
        {
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
            }
            else speed = 0; //���ߕ���߂�
        }

        //!�S���ӏ�---
        // �������ֈړ� 
        // �������u�ԂɈړ������㏑��
        if (Input.GetKeyDown(KeyCode.A))
        {
            isLeft = true;
            isRight = false;
            if (speed > 0)
                speed = 0;
        }
        //!---�S���ӏ�

        if (Input.GetKey(KeyCode.A) && isLeft)
        {
            //!�S���ӏ�---
            //�G�t�F�N�g�̌����ύX
            windEffect.transform.localPosition = new Vector3(7.0f, -6.5f, 0);
            windEffect.transform.rotation = Quaternion.Euler(-angle, 90.0f, -90.0f);
            //!---�S���ӏ�

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
            }
            else speed = 0;
        }

        transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);

        //!�S���ӏ�---
        Spine();
        //!---�S���ӏ�
    }

    //!�S���ӏ�---
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
                box.BoxRightMove(); //�E�ړ�
            }

            if (Input.GetKey(KeyCode.A) && box != null)
            {
                box.BoxLeftMove(); //���ړ�
            }

            //�ؔ����������SE
            if (soundSpan >= 0)
            {
                soundSpan -= Time.deltaTime;
            }

            if (soundSpan <= 0.0f)
            {
                audioSource.PlayOneShot(Box);
                float se_length = 1.752f;
                soundSpan = se_length;
            }
        }
        else
        {
            //�����Ă���or�����n�߂̃A�j���[�V�������Đ����ł����
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("push") || anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
                anim.SetBool("Push", false); //�A�j���[�V������~
        }

        //�ؔ��ɐG��Ă���Ƃ��@���@AD�L�[�������Ă���Ƃ�
        if (isPushCount)
        {
            //���Ԃ��v��
            pTime += Time.deltaTime;

            //��莞�ԃL�[��������������ؔ���������悤�ɂȂ�
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
            box = null; //�ؔ��������Ȃ��悤��null�ɂ���
            anim.SetBool("Push", false); //�A�j���[�V������~
        }
    }

    //�v���C���[�C���X�g�ʒu�����p�ϐ�
    Vector3 pushAjustR = new Vector3(5.0f, -3.0f, 0); //�E�ɖؔ��������Ă���Ƃ�
    Vector3 pushAjustL = new Vector3(-5.0f, -3.0f, 0); //���ɖؔ��������Ă���Ƃ�
    Vector3 dashAjust = new Vector3(0, -2.0f, 0); //�����Ă���Ƃ�
    Vector3 normalAjust = new Vector3(0, -1.0f, 0); //�ʏ���

    /// <summary>
    /// �ꕔSpine�̐���
    /// </summary>
    void Spine()
    {
        //�v���C���[�ʒu�����@Spine�̓s���㕂���Č�����̂� ���E�������ꂼ��̃C���X�g���ʒu��������
        for (int i = 1; i <= 2; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("push") || anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
            {
                if (speed >= 0)
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
        if (playerstate == PlayerState.Human)
        {
            //�L�[���������u�ԁA�t�����̃L�[��������Ă��Ȃ���Ԃ̂Ƃ��A�i�s�������������C���X�g�ɕς���
            if (Input.GetKeyDown(KeyCode.A) || (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                AnimFlipped("left");
            }
            if (Input.GetKeyDown(KeyCode.D) || (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)))
            {
                AnimFlipped("right");
            }
        }

        //AD�L�[�������Ă����
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //����A�j���[�V�������Đ����łȂ����
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
            {
                anim.SetBool("Dash", true); //����A�j���[�V�����Đ�
            }
        }

        //��葬�x�ȉ��ŃA�j���[�V�������~����
        float animMinSpeed = 5.0f;
        if (Mathf.Abs(speed) < animMinSpeed && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Dash", false);
        }

        //���x�ɍ��킹�đ��胂�[�V�����̑��x���㏸
        float coe = 0.1f;
        anim.SetFloat("Speed", Mathf.Abs(speed) * coe + 1);
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

        //�A�j���[�V������������Ԃɂ���@playerAnims[lastAnim]�ɑJ�ڂ������ɁA�A�C�h�����O��Ԃ���J�n����悤��
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

    //!---�S���ӏ�

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
        }
    }

    /// <summary>
    /// �ړ���
    /// </summary>
    private void MoveSound()
    {
        //�Đ����I��������܂��Đ�
        if (soundSpan >= 0)
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

        if (interval <= 0)
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
        //!�S���ӏ�---
        //���n�A�j���[�V����
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }
        //!---�S���ӏ�

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
            }
            if (Input.GetKey(KeyCode.A) && objectBreak)
            {
                speed -= speed * 0.2f;
            }

            //!�S���ӏ�---
            //���n�A�j���[�V���� �ؔ��̏�ł̒��n����
            foreach (ContactPoint2D contact in collision.contacts)
            {
                var hitPoint = contact.point;
                var sub = hitPoint.y - transform.position.y;

                //�Փˈʒu���m�F���A�v���C���[�̉������������ꍇ�͒��n�����Ƃ������Ƃɂ���
                if (sub < -0.7f)
                {
                    isGround = true;
                    anim.SetBool("Jump", false); //���n�A�j���[�V�����Đ�
                }
            }
            //!---�S���ӏ�
        }

        //!�S���ӏ�---
        //��ɂԂ�������
        if (collision.gameObject.tag == "Slope")
        {
            //�R���|�[�l���g�擾
            var slope = collision.gameObject.GetComponent<Slope>();

            //��̊p�x�擾
            angle = slope.Angle;

            //�������Z�𖳂���
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        //!---�S���ӏ�
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

        //!�S���ӏ�---
        if (collision.gameObject.tag == "Box")
        {
            foreach (ContactPoint2D contact in collision.contacts)
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
                            //���������ǂ����𔻒肷�邽�߂̎��Ԍv�����J�n
                            isPushCount = true;

                            //�������ؔ��̃R���|�[�l���g���擾
                            var obj = collision.gameObject;
                            box = obj.GetComponent<Box>();

                            //�ړ����x�Œ艻
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
        //!---�S���ӏ�
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = false;
        }

        //!�S���ӏ�---
        //�ؔ����痣�ꂽ��
        if (collision.gameObject.tag == "Box")
        {
            //�Փˈʒu���m�F���A�v���C���[�̉������������ꍇ�͖ؔ��̏ォ��W�����v�����Ƃ������Ƃɂ���
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

            //�ؔ������������A�j���[�V������������
            isPushing = false;
            isPushCount = false;
            anim.SetBool("Push", false);
        }

        //�₩�痣�ꂽ��
        if (collision.gameObject.tag == "Slope")
        {
            rb.gravityScale = 2;

            //�ړ��p�x��������Ԃɂ���
            angle = 0;
        }
        //!---�S���ӏ�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible) //���G��Ԃ���Ȃ��Ƃ�
        {
            //�Փˑ��肪�G���l�^�����x�V�������̂Ƃ��@�܂��͏Փˑ��肪�g�Q�̂Ƃ��A�_���[�W����
            if ((collision.gameObject.tag == "Enemy" && !objectBreak) || collision.gameObject.tag == "Thorn")
            {
                //!�S���ӏ�---
                //�J�v�Z����ԉ���
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                    anim = playerAnims[1];
                    playerMeshs[1].enabled = true;
                    playerMeshs[0].enabled = false;
                    playerMeshs[2].enabled = false;
                }
                //!---�S���ӏ�

                speed -= speed * 0.5f;
                invincible = true;
                HpController.IsDamage = true;

                //!�S���ӏ�---
                anim.SetTrigger("Damage");
                //!---�S���ӏ�
            }
        }

        //���ɗ������Ƃ�
        if (collision.gameObject.tag == "Hole")
        {
            HpController.IsFall = true;
        }

        //�X�s�[�h�A�b�v�A�C�e���擾
        if (collision.gameObject.tag == "SpeedUP")
        {
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
                //!�S���ӏ�---
                anim.SetTrigger("Damage");
                //�J�v�Z����ԉ���
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                    anim = playerAnims[1];
                    playerMeshs[1].enabled = true;
                    playerMeshs[0].enabled = false;
                    playerMeshs[2].enabled = false;
                }
                //!---�S���ӏ�

                speed -= speed * 0.5f;
                invincible = true;
                HpController.IsDamage = true;
            }
        }
    }

    IEnumerator ToBall()
    {
        //!�S���ӏ�---
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
        //!---�S���ӏ�

        playerstate = PlayerState.Circle;
    }

    IEnumerator ToHuman()
    {
        //!�S���ӏ�---
        //�t�H�[���`�F���W�A�j���[�V�����I���t���O��false�ɂ���
        //���ɐl�^->�J�v�Z���ό`���Atrue�̂܂܂��ƃA�j���[�V�������Đ�����Ȃ��̂�
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
        //!---�S���ӏ�

        playerstate = PlayerState.Human;
    }
}

