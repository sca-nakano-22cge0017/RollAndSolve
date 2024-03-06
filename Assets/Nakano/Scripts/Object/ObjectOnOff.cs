using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g��Ԑ{�^���ŕ\��/��\������
/// </summary>
public class ObjectOnOff : MonoBehaviour
{
    public enum ONOFF { on = 0, off };
    [Tooltip("�{�^���������ăI�u�W�F�N�g��\������Ƃ��Fon ��\���ɂ���Ƃ��Foff")] public ONOFF onOff = 0;

    [SerializeField, Header("�\��/��\���ɂ���I�u�W�F�N�g")] GameObject obj;

    ButtonObject button;

    void Start()
    {
        button = this.GetComponent<ButtonObject>();
    }

    void Update()
    {
        switch (onOff)
        {
            //�{�^���������ĕ\������Ƃ�
            case ONOFF.on:
                obj.gameObject.SetActive(button.IsActive);
                break;

            //�{�^���������Ĕ�\���ɂ���Ƃ�
            case ONOFF.off:
                obj.gameObject.SetActive(!button.IsActive);
                break;
        }

        //�{�^�����������Ƃ��Atrue��button.IsActive�ɓ���̂ŕ\������Ƃ��͂��̂܂�SetActive�֑���A��\���ɂ���Ƃ���false�ɔ��]
        //�{�^���𗣂����Ƃ��͋t�ɂȂ�
    }
}
