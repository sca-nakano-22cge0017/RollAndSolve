using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderCollider : MonoBehaviour
{
    bool Collision = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("collision");
            Collision = true;
        }
    }
}
