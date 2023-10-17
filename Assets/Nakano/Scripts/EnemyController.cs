using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("ˆÚ“®‘¬“x")] float speed;
    
    GameObject player;
    Vector3 playerPos;
    Vector3 thisPos;

    Vector3 direction; //ˆÚ“®•ûŒü

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
