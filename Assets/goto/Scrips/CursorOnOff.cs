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
        {//�J�[�\����\�����Ȃ�
            Cursor.visible = false;
        }
        else
        {
            //�J�[�\����\�����Ȃ�
            Cursor.visible = true;
        }
    }
}
