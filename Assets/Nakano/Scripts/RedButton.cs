using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    public bool isActive; //true�̂Ƃ��A�M�~�b�N����������

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
