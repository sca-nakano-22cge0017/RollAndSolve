using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRay : MonoBehaviour
{
    [SerializeField, Header("Ray‚Ì•ûŒü")] Vector2 direction;
    [SerializeField, Header("Ray‚Ì’·‚³")] float maxRange = 0.01f;

    GameObject enemy;

    Vector2 dir;
    Ray2D ray;
    RaycastHit2D hit;

    public bool isHit;

    void Start()
    {
        enemy = transform.parent.gameObject;
        isHit = false;
    }

    void Update()
    {
        if(enemy.transform.localScale.x == -0.05f)
        {
            dir = direction * -1;
        }
        else { dir = direction; }

        ray = new Ray2D(transform.position, dir); //Ray‚Ìì¬
        hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxRange);
        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red);

        if (hit.collider)
        {
            isHit = true;
        }
        else
        {
            isHit = false;
        }
    }
}
