using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    [SerializeField, Header("�E����->1 ������->-1")] int num = 1;
    [SerializeField, Header("�c/���̃}�X�̐�")] Vector2 size = new Vector2(1, 1);
    float angle;

    /// <summary>
    /// �x���@�ŕԂ� 
    /// var slope = collision.gameObject.GetComponent<Slope>();
    /// angle = slope.Angle; 
    /// rb.gravtyScale = 0;
    /// </summary>
    public float Angle
    {
        get { return angle; }
    }

    private void Awake()
    {
        if(size.x <= 0) size.x = 1;
        if(size.y <= 0) size.y = 1;
        if(num != 1 && num != -1) num = 1;

        angle = Mathf.Atan2(size.y, size.x) * Mathf.Rad2Deg;
        if(num == -1) angle = (180.0f - angle) + 180.0f;
    }
}
