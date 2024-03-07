using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�^������������A�C�e����������
/// </summary>
public class ItemsFall : MonoBehaviour
{
    [SerializeField] GameObject obj;

    Rigidbody2D rb;
    Vector3 defaultPos; //�A�C�e�������ʒu
    ButtonObject button;

    void Start()
    {
        rb = obj.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        defaultPos = obj.GetComponent<Transform>().position;
        button = this.GetComponent<ButtonObject>();
    }

    
    void Update()
    {
        if(obj)
        {
            if (button.IsActive)
            {
                //����
                rb.isKinematic = false;
            }
            else
            {
                //�����ʒu�ɖ߂�
                obj.transform.position = defaultPos;
                rb.isKinematic = true;
            }
        }
    }
}
