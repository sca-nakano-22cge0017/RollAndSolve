using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ؔ����ǂ��ђʂ��Ȃ��悤�ɔ��肷��
/// �ؔ��̎q�I�u�W�F�N�g��Collider�Ƌ��ɕt��
/// </summary>
public class BoxStopCheck : MonoBehaviour
{
    Box box;
    public enum RL { left = 0, right }; //�ؔ��̉E���E������ݒ�
    public RL type = 0;

    void Start()
    {
        box = gameObject.transform.parent.GetComponent<Box>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            switch (type)
            {
                //�E����Collider���ǂɂԂ�������E�ւ̈ړ���~
                case RL.right:
                    box.canMoveR = false;
                    break;

                //������Collider���ǂɂԂ������獶�ւ̈ړ���~
                case RL.left:
                    box.canMoveL = false;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            switch (type)
            {
                case RL.right:
                    box.canMoveR = true;
                    break;
                case RL.left:
                    box.canMoveL = true;
                    break;
            }
        }
    }
}
