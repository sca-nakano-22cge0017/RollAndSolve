using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 変形アニメーションの終了を確認する
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
