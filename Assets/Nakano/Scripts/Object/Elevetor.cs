using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エレベーター
/// </summary>
public class Elevetor : MonoBehaviour
{
    [SerializeField, Tooltip("移動するオブジェクト")] GameObject obj;
    [SerializeField, Tooltip("移動速度")] float speed;
    [SerializeField, Tooltip("一番上/下に着いてからまた動き始めるまでのクールタイム")] float coolTime;
    [SerializeField, Header("一番上のY座標")] float topPos;
    [SerializeField, Header("一番下のY座標")] float downPos;

    ButtonObject button;

    bool isMax = false; //一番上のときtrue
    bool isMin = false; //一番下のときtrue

    /// <summary>
    /// 一番下まで来たらtrue
    /// エレベーターの下辺が何かにぶつかったら「一番下に到達した」ということにし、移動を止める
    /// </summary>
    public bool IsMin
    {
        get { return isMin; }
        set { isMin = value; }
    }

    enum STATE { down = 0, up }; //下降状態/上昇状態
    STATE state = 0;

    void Start()
    {
        button = this.GetComponent<ButtonObject>();
        topPos = obj.transform.position.y;
    }

    void Update()
    {
        //ボタンが押されている間は動く
        if(button.IsActive)
        {
            Move();
        }
        else { DefaultMove(); }
    }

    /// <summary>
    /// 上下移動
    /// </summary>
    private void Move()
    {
        if(obj.transform.position.y >= topPos)
        {
            isMax = true;
        }

        if (obj.transform.position.y <= downPos)
        {
            isMin = true;
        }

        //上限下限まで移動したらstateを変える
        if (isMax) { StartCoroutine(StateChange(STATE.down)); }
        if (isMin) { StartCoroutine(StateChange(STATE.up)); }

        //stateによって移動方向を変える
        switch (state)
        {
            //下降状態
            case STATE.down:
                if(!isMin) //下限じゃないとき下降
                    obj.transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
            //上昇状態
            case STATE.up:
                if (!isMax) //上限じゃないとき上昇
                    obj.transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;
        }
    }

    /// <summary>
    /// 初期位置に戻る
    /// </summary>
    void DefaultMove()
    {
        //一番上に行ったら停止
        if (obj.transform.position.y >= topPos)
        {
            isMax = true;
        }

        if(!isMax) obj.transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator StateChange(STATE s)
    {
        yield return new WaitForSeconds(coolTime);
        
        state = s; //stateを変更
        isMax = false;
        isMin = false;
    }
}