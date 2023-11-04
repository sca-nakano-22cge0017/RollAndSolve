using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCheck : MonoBehaviour
{
    bool isActive = false;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isActive)
        {
            Debug.Log("”­“®’†");
        }
    }
}
