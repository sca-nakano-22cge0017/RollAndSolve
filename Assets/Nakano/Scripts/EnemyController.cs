using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] float speed;
    [SerializeField, Header("追跡範囲")] float dis;
    [SerializeField, Header("下へのRayの位置")] Transform underRay;
    
    GameObject player;
    Vector3 playerPos;
    Vector3 thisPos;

    Collider2D col;
    Rigidbody2D rb;

    Vector3 direction; //移動方向
    bool isChase; //プレイヤーを追いかけているかどうか

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        col.isTrigger = false;
        isChase = false;
        direction = Vector3.right;
    }

    void Update()
    {
        playerPos = player.transform.position;
        thisPos = this.transform.position;

        var distance = Vector3.Distance(playerPos, thisPos);

        if (distance <= dis)
        {
            isChase = true;
        }
        else if(distance > dis)
        {
            isChase = false;
        }

        if(isChase)
        {
            direction = (playerPos - thisPos).normalized;
            transform.Translate(new Vector3(direction.x, 0, 0) * speed * Time.deltaTime);
        }

        Ray2D ray = new Ray2D(underRay.transform.position, Vector3.down);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 0.01f);

        if (hit.collider.gameObject.tag == "Ground")
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            col.isTrigger = false;
        }
    }
}
