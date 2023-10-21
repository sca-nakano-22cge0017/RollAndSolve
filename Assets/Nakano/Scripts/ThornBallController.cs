using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBallController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] float speed;
    [SerializeField, Header("初期移動方向反転"), Tooltip("falseで右へ、trueで左へ")] bool isReverse;
    [SerializeField, Header("オブジェクトの半径")] float radius;

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
        //一時的にタグがWallのものにぶつかったときのみ移動方向を反転する
        if (collision.gameObject.tag == "Wall")
        {
            direction *= -1;
        }
    }
}
