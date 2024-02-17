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
        //–¢ƒNƒŠƒAƒJƒƒ‰’Ç]
        if(!isClear)
        {
            transform.position = playerController.gameObject.transform.position + pos;
        }
    }
}
