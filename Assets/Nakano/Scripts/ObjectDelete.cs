using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDelete : MonoBehaviour
{
    [SerializeField] GameObject obj;

    ButtonObject button;

    void Start()
    {
        button = this.GetComponent<ButtonObject>();
    }

    void Update()
    {
        if(button.IsActive)
        {
            obj.gameObject.SetActive(false);
        }
        else { obj.gameObject.SetActive(true); }
    }
}
