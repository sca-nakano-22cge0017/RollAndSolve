using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    public bool canMoveR = true;
    public bool canMoveL = true;

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
}