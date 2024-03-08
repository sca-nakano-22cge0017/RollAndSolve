using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �󔠂̐���
/// </summary>
public class TreasureController : MonoBehaviour
{
    Animator anim;

    [SerializeField, Header("���_��")] GameObject medal;

    bool isOpen; //�󔠂��J����
    bool isClear; //�N���A����

    /// <summary>
    /// �N���A����
    /// true�̂Ƃ��X�e�[�W�N���A
    /// </summary>
    public bool IsClear
    {
        get { return isClear; }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        
        isOpen = false;
        isClear = false;
    }

    void Update()
    {
        //�󔠂ɐG�ꂽ��N���A
        if (isOpen)
        {
            //�A�j���[�V����
            anim.SetTrigger("Open");

            //���_���l�����o
            Instantiate(medal, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.1f, 0), Quaternion.identity);

            isClear = true;
            isOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[������������A�󔠂��J�����ǂ����̃t���O��true�ɂ���
        if(collision.gameObject.tag == "Player")
        {
            isOpen = true;
        }
        
        //�����������痎�������Ƃ��n�ʂŎ~�܂�
        if (collision.gameObject.tag == "Ground")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.mass = 0;
            rb.gravityScale = 0;
        }
    }
}
