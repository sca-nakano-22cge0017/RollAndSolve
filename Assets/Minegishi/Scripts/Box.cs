using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void BoxRightMove()
    {
        Debug.Log("MoveBox");
        transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
    }

    public void BoxLeftMove()
    {
        Debug.Log("MoveBox");
        transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
    }
}