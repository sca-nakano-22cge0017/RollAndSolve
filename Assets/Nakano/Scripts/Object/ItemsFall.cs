using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタンを押したらアイテムが落ちる
/// </summary>
public class ItemsFall : MonoBehaviour
{
    [SerializeField] GameObject obj;

    Rigidbody2D rb;
    Vector3 defaultPos; //アイテム初期位置
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
                //落下
                rb.isKinematic = false;
            }
            else
            {
                //初期位置に戻る
                obj.transform.position = defaultPos;
                rb.isKinematic = true;
            }
        }
    }
}
