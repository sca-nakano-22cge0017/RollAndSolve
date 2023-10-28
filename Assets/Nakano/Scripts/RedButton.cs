using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    bool isActive; //trueのとき、ギミックが発動する

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
            Debug.Log("発動中");
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
