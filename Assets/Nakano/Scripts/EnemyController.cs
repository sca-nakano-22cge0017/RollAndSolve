using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] float speed;
    
    GameObject player;
    Vector3 playerPos;
    Vector3 thisPos;

    Vector3 direction; //移動方向

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        playerPos = player.transform.position;
        thisPos = this.transform.position;

        direction = (playerPos - thisPos).normalized;

        transform.Translate(new Vector3(direction.x, 0, 0) * speed * Time.deltaTime);
    }
}
