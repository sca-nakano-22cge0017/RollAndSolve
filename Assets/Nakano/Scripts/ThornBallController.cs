using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBallController : MonoBehaviour
{
    [SerializeField, Header("ˆÚ“®‘¬“x")] float speed;
    [SerializeField, Header("‰ŠúˆÚ“®•ûŒü”½“]"), Tooltip("false‚Å‰E‚ÖAtrue‚Å¶‚Ö")] bool isReverse;

    Vector3 direction;

    void Start()
    {
        if(!isReverse)
        {
            direction = Vector3.right;
        }
        if(isReverse)
        {
            direction = Vector3.left;
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            direction *= -1;
        }
    }
}
