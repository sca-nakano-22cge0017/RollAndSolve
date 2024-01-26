using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBallFall : MonoBehaviour
{
    Rigidbody2D rb;

    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if(Mathf.Abs(this.transform.position.x - player.transform.position.x) <= 0.1f)
        {
            rb.isKinematic = false;
        }
    }
}
