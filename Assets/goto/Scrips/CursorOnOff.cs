using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOnOff : MonoBehaviour
{
    [SerializeField]
    bool isOn = false;
    public bool IsOn { get { return isOn; } }

    public void InitFlag()
    {
        isOn = false;
    }

    public void SetFlagStatus(bool value = true)
    {
        isOn = value;
    }

    void Update()
    {
        if (isOn)
        {//カーソルを表示しない
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //カーソルを表示しない
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
