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

    /// <summary>
    /// �N���A�A�j���[�V�����I��
    /// </summary>
    public void TextAnimEnd()
    {
        clear.TextAnimEnd = true;
    }
}
