using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �N���A���o��Animation����Ăяo���p
/// �A�j���[�V�������I��������Ƃ�`����
/// </summary>
public class ClearEffectEnd : MonoBehaviour
{
    [SerializeField] Clear clear;

    public void TextAnimEnd()
    {
        clear.TextAnimEnd = true;
    }
}
