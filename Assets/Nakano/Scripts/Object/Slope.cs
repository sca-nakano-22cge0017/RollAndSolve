using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    [SerializeField, Header("右向き->1 左向き->-1")] int num = 1;
    [SerializeField, Header("縦/横のマスの数")] Vector2 size = new Vector2(1, 1);
    float angle;

    /// <summary>
    /// 度数で返す
    /// </summary>
    public float Angle
    {
        get { return angle; }
    }

    /// <summary>
    /// 右向きか左向きかの変数 1なら右向き -1なら左向き
    /// </summary>
    public int Num
    {
        get { return num; }
    }

    private void Awake()
    {
        if(size.x <= 0) size.x = 1;
        if(size.y <= 0) size.y = 1;
        if(num != 1 && num != -1) num = 1;

        angle = Mathf.Atan2(size.y, size.x);
        angle = Mathf.Rad2Deg * angle;
    }
}
