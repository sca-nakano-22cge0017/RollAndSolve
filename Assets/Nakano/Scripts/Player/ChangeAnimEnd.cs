using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ό`�A�j���[�V�����̏I�����m�F����
/// </summary>
public class ChangeAnimEnd : MonoBehaviour
{
    bool isEnd = false;
    public bool IsEnd { get { return isEnd; } set{ isEnd = value; } }
    
    public void AnimEnd()
    {
        isEnd = true;
    }
}
