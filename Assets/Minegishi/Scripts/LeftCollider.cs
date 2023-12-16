using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCollider : MonoBehaviour
{
    bool Collision = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" ||
           collision.gameObject.tag == "Box")
        {
            Debug.Log("collision");
            Collision = true;
        }
    }
}
