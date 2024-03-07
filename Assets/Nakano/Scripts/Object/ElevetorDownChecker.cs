using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エレベーターの下に何かあったとき止まるための当たり判定用
/// 何かに衝突したらそこをエレベーター移動の下限として、それ以上下がらないようにする
/// </summary>
public class ElevetorDownChecker : MonoBehaviour
{
    [SerializeField] Elevetor elevetor;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        elevetor.IsMin = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        elevetor.IsMin = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        elevetor.IsMin = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        elevetor.IsMin = false;
    }
}