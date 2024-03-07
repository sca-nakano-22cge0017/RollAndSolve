using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ؔ��̍Đ�������
/// </summary>
public class BoxesRecreate : MonoBehaviour
{
    [SerializeField, Header("�e�I�u�W�F�N�g")] GameObject parent;
    [SerializeField, Header("��������ؔ���Prefab")] GameObject prefab;

    float waitTime = 5.0f; //�j�󂩂�Đ����܂ł̃N�[���^�C��

    /// <summary>
    /// �ؔ������������u�ԂɍĐ�������
    /// </summary>
    /// <param name="tr">����������W</param>
    /// <param name="create">�Đ������邩�ǂ���</param>
    public void FallBox(Vector3 tr, bool create)
    {
        //��x�����Đ���
        if(create)
        {
            Instantiate(prefab, tr, Quaternion.identity, parent.transform);
            create = false;
        }
    }

    /// <summary>
    /// �ؔ��̍Đ����@�j�󂳂ꂽ�^�C�~���O�ŌĂяo��
    /// ���ۂɐ�������̂̓R���[�`��
    /// </summary>
    /// <param name="tr">����������W</param>
    public void Recreate(Vector3 tr)
    {
        StartCoroutine(recreate(tr));
    }

    /// <summary>
    /// �ؔ��̍Đ���
    /// </summary>
    /// <param name="tr">����������W</param>
    /// <returns></returns>
    IEnumerator recreate(Vector3 tr)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(prefab, tr, Quaternion.identity, parent.transform);
    }
}
