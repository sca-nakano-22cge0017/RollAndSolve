using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCamera : MonoBehaviour
{
    bool isClear = false;
    public bool IsClear { set { isClear = value; } }

    [SerializeField] PlayerController playerController;
    [SerializeField] Vector3 pos;

    void Start()
    {
        
    }

    void Update()
    {
        //未クリア時カメラ追従
        if(!isClear)
        {
            transform.position = playerController.gameObject.transform.position + pos;
        }
    }
}
