using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsFall : MonoBehaviour
{
    [SerializeField] GameObject obj;

    Rigidbody2D rb;
    Vector3 defaultPos;
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
                rb.isKinematic = false;
            }
            else
            {
                obj.transform.position = defaultPos;
                rb.isKinematic = true;
            }
        }
    }
}
