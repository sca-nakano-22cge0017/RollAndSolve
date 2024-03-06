using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�^���{�̂̐���
/// </summary>
public class ButtonObject : MonoBehaviour
{
    public enum BUTTON { red = 0, blue };
    public BUTTON buttonType = 0;

    Vector3 defaultPosition;

    [SerializeField, Header("�ؔ��Ƃ̔�����Ȃ��ɂ���")] bool isBox = false;
    [SerializeField, Header("Debug�p"), Tooltip("�`�F�b�N������ƃ{�^�����쓮")] bool isActive = false;

    Vector3 pushPos = new Vector3(0, -0.1f, 0); //�������Ƃ��A�{�^���������ǂ̂��炢���Ɉړ����邩

    PlayerController playerController;

    /// <summary>
    /// �{�^�����쓮���Ă���Ƃ���true
    /// </summary>
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();

        //���X�̃{�^�������̍��W��ۑ�
        defaultPosition = this.transform.position;
    }

    void Update()
    {
        //������Ă�Ƃ��̓{�^�����������Ɉړ�
        if(isActive)
        {
            transform.position = defaultPosition + pushPos;
        }

        //������Ă��Ȃ�
        else
        {
            //�{�^���̏ꍇ�A���̈ʒu�ɖ߂�
            if (buttonType == ButtonObject.BUTTON.blue)
            {
                if (transform.position.y <= defaultPosition.y)
                {
                    transform.position = defaultPosition;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���葊�肪�v���C���[
        if (collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[���l�^�̂Ƃ�
            if (playerController.playerstate == PlayerController.PlayerState.Human)
            {
                //����
                isActive = true;
            }
            else
            {
                //�J�v�Z����ԂȂ牟���Ȃ�
                isActive = false;
            }
        }

        //���葊�肪�ؔ����A�ؔ��Ɣ�������ꍇ
        if (collision.gameObject.CompareTag("Box") && !isBox)
        {
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�{�^���̂Ƃ��̓v���C���[��ؔ������ꂽ��off
        if (buttonType == ButtonObject.BUTTON.blue)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
            {
                isActive = false;
            }
        }
    }
}
