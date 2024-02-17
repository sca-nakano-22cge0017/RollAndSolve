using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    bool canMoveR = true;
    bool canMoveL = true;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void BoxRightMove()
    {
        //Debug.Log("MoveBox");
        if(canMoveR)
        {
            transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
        }
    }

    public void BoxLeftMove()
    {
        //Debug.Log("MoveBox");
        if(canMoveL)
        {
            transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                //è’ìÀà íuÇéÊìæ
                var hitPoint = contact.point;
                var subY = hitPoint.y - transform.position.y;
                var subX = hitPoint.x - transform.position.x;

                //âEÇ…âΩÇ©Ç†Ç¡ÇΩÇÁ
                if (subY <= 0.7f && subX <= 0.9f && subX >= -0.9f)
                {
                    canMoveR = false;
                }

                //ç∂Ç…âΩÇ©Ç†Ç¡ÇΩÇÁ
                if (subY >= -0.7f && subX <= 0.9f && subX >= -0.9f)
                {
                    canMoveL = false;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                //è’ìÀà íuÇéÊìæ
                var hitPoint = contact.point;
                var subY = hitPoint.y - transform.position.y;
                var subX = hitPoint.x - transform.position.x;

                if (subY <= 0.7f && subX <= 0.9f && subX >= -0.9f)
                {
                    canMoveR = true;
                }

                if (subY >= -0.7f && subX <= 0.9f && subX >= -0.9f)
                {
                    canMoveL = true;
                }
            }
        }
    }
}