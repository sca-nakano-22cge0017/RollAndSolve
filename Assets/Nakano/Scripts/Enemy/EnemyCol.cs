using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̍��E�����̏Փ˔�����Ƃ�
/// </summary>
public class EnemyCol : MonoBehaviour
{
    bool isHit = false;

    /// <summary>
    /// ��Q���ɂԂ��������ǂ����̔���
    /// </summary>
    public bool IsHit
    {
        get { return isHit; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemy�{�́A�J��������p��Collider�͖�������
        if(!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Camera"))
            isHit = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Camera"))
            isHit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Camera"))
            isHit = false;
    }
}
