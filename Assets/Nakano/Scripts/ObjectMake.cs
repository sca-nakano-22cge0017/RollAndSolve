using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMake : MonoBehaviour
{
    GameObject parent;
    GameObject button;
    GameObject obj;

    ButtonCheck check;

    void Start()
    {
        parent = transform.parent.gameObject;
        obj = parent.transform.Find("Objects").gameObject;
        check = this.GetComponent<ButtonCheck>();
    }

    void Update()
    {
        if (check.IsActive)
        {
            obj.gameObject.SetActive(true);
        }
    }
}
