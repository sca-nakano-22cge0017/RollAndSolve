using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffectEnd : MonoBehaviour
{
    [SerializeField] Clear clear;

    public void EffectAnimEnd()
    {
        clear.EffectEnd = true;
    }

    public void TextAnimEnd()
    {
        clear.TextAnimEnd = true;
    }
}
