using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDelete : MonoBehaviour
{
    GameObject parent;
    GameObject button;

    ButtonCheck check;

    void Start()
    {
        parent = transform.parent.gameObject;
        button = parent.transform.Find("Button").gameObject;
        check = button.GetComponent<ButtonCheck>();
    }

    void Update()
    {
        if(check.IsActive)
        {
            this.gameObject.SetActive(false);
        }
    }
}
