using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��̊p�x���Z�o����
/// </summary>
public class Slope : MonoBehaviour
{
    [SerializeField, Header("�E����->1 ������->-1")] int num = 1;
    [SerializeField, Header("�c/���̃}�X�̐�")] Vector2 size = new Vector2(1, 1);
    float angle;

    /// <summary>
    /// ��̊p�x��x���@�ŕԂ� 
    /// </summary>
    public float Angle
    {
        get { return angle; }
    }

    //�p�x�v�Z
    private void Awake()
    {
        //�z��O�̒l�̂Ƃ��C��
        if(size.x <= 0) size.x = 1;
        if(size.y <= 0) size.y = 1;
        if(num != 1 && num != -1) num = 1;

        //�p�x�Z�o
        angle = Mathf.Atan2(size.y, size.x) * Mathf.Rad2Deg;

        //�������̏ꍇ��y�����S�Ɋp����Ώ̈ړ�
        if(num == -1) angle = 360.0f - angle;
    }
}
