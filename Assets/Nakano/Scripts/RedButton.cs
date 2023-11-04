using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    ButtonCheck check;

    void Start()
    {
        check = gameObject.GetComponent<ButtonCheck>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Button_Base")
        {
            check.IsActive = true;
        }
    }
}
