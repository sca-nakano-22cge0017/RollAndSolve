using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クリア演出のAnimationから呼び出す用
/// アニメーションが終わったことを伝える
/// </summary>
public class ClearEffectEnd : MonoBehaviour
{
    [SerializeField] Clear clear;

    /// <summary>
    /// クリアアニメーション終了
    /// </summary>
    public void TextAnimEnd()
    {
        clear.TextAnimEnd = true;
    }
}
