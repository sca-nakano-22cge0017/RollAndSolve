using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureController : MonoBehaviour
{
    Animator anim;

    [SerializeField, Header("���_��")] GameObject medal;

    bool isOpen; //�󔠂��J���邩
    bool isClear; //�N���A����

    PlayerController playerController;

    /// <summary>
    /// �N���A����
    /// true�̂Ƃ��A�N���A
    /// </summary>
    public bool IsClear
    {
        get { return isClear; }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GameObject.FindObjectOfType<PlayerController>();

        isOpen = false;
        isClear = false;
    }

    void Update()
    {
        //�󔠂ɐG�ꂽ��N���A
        if (isOpen)
        {
            anim.SetTrigger("Open");

            //���_���l�����o
            Instantiate(medal, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.1f, 0), Quaternion.identity);
            isClear = true;
            isOpen = false;
            playerController.IsPause = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[������������w���E�B���h�E��\�����A�󔠂��J�����Ƃ��ł��邩�ǂ����̃t���O��true�ɂ���
        if(collision.gameObject.tag == "Player")
        {
            isOpen = true;
        }
        
        //�󔠂̗���
        if (collision.gameObject.tag == "Ground")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.mass = 0;
            rb.gravityScale = 0;
        }
    }
}
