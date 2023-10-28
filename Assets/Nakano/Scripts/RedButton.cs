using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    bool isActive; //true�̂Ƃ��A�M�~�b�N����������

    public bool IsActive
    {
        get { return isActive; }
    }

    void Start()
    {
        isActive = false;
    }

    void Update()
    {
        if(isActive)
        {
            Debug.Log("������");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Button_Base")
        {
            isActive = true;
        }
    }
}
