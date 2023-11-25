using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRay : MonoBehaviour
{
    [SerializeField, Header("Ray‚Ì•ûŒü")] Vector2 direction;
    [SerializeField, Header("Ray‚Ì’·‚³")] float maxRange = 0.01f;

    Ray2D ray;
    RaycastHit2D hit;

    public bool isHit;

    void Start()
    {
        isHit = false;
    }

    void Update()
    {
        ray = new Ray2D(transform.position, direction); //Ray‚Ìì¬
        hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxRange);
        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red); //Ray‚Ì•`‰æ

        if(hit.collider)
        {
            if(hit.collider.tag != "Enemy" && hit.collider.tag != "Player")
            {
                isHit = true;
            }
        }
        else { isHit = false; }
    }
}
