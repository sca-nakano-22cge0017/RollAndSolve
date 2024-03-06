using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを赤青ボタンで表示/非表示する
/// </summary>
public class ObjectOnOff : MonoBehaviour
{
    public enum ONOFF { on = 0, off };
    [Tooltip("ボタンを押してオブジェクトを表示するとき：on 非表示にするとき：off")] public ONOFF onOff = 0;

    [SerializeField, Header("表示/非表示にするオブジェクト")] GameObject obj;

    ButtonObject button;

    void Start()
    {
        button = this.GetComponent<ButtonObject>();
    }

    void Update()
    {
        switch (onOff)
        {
            //ボタンを押して表示するとき
            case ONOFF.on:
                obj.gameObject.SetActive(button.IsActive);
                break;

            //ボタンを押して非表示にするとき
            case ONOFF.off:
                obj.gameObject.SetActive(!button.IsActive);
                break;
        }

        //ボタンを押したとき、trueがbutton.IsActiveに入るので表示するときはそのままSetActiveへ代入、非表示にするときはfalseに反転
        //ボタンを離したときは逆になる
    }
}
