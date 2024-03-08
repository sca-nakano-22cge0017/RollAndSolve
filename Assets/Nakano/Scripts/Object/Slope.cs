using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 坂の角度を算出する
/// </summary>
public class Slope : MonoBehaviour
{
    [SerializeField, Header("右向き->1 左向き->-1")] int num = 1;
    [SerializeField, Header("縦/横のマスの数")] Vector2 size = new Vector2(1, 1);
    float angle;

    /// <summary>
    /// 坂の角度を度数法で返す 
    /// </summary>
    public float Angle
    {
        get { return angle; }
    }

    //角度計算
    private void Awake()
    {
        //想定外の値のとき修正
        if(size.x <= 0) size.x = 1;
        if(size.y <= 0) size.y = 1;
        if(num != 1 && num != -1) num = 1;

        //角度算出
        angle = Mathf.Atan2(size.y, size.x) * Mathf.Rad2Deg;

        //左向きの場合はy軸中心に角を線対称移動
        if(num == -1) angle = 360.0f - angle;
    }
}
