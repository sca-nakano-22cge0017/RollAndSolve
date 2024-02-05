using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureController : MonoBehaviour
{
    Animator anim;
    [SerializeField, Header("�l���m�F�E�B���h�E")] GameObject window;

    [SerializeField, Header("���_��")] GameObject medal;

    bool isOpen; //�󔠂��J���邩
    bool isClear; //�N���A����

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

        window.SetActive(false);
        isOpen = false;
        isClear = false;
    }

    void Update()
    {
        //�󔠂ɐG��āAP�L�[����������N���A
        if (Input.GetKeyDown(KeyCode.P) && isOpen)
        {
            anim.SetTrigger("Open");

            //���_���l�����o
            Instantiate(medal, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.1f, 0), Quaternion.identity);
            isClear = true;
            window.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[������������w���E�B���h�E��\�����A�󔠂��J�����Ƃ��ł��邩�ǂ����̃t���O��true�ɂ���
        if(collision.gameObject.tag == "Player")
        {
            window.SetActive(true);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            window.SetActive(false);
            isOpen = false;
        }
    }
}
