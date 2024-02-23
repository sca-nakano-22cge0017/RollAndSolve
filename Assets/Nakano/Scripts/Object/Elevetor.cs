using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevetor : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] float speed;
    [SerializeField, Tooltip("一番上/下に着いてからまた動き始めるまでのクールタイム")] float coolTime;
    [SerializeField, Header("一番上のY座標")] float topPos;
    [SerializeField, Header("一番下のY座標")] float downPos;

    ButtonObject button;

    bool isMax = false;
    bool isMin = false;

    public bool IsMin
    {
        get { return isMin; }
        set { isMin = value; }
    }

    enum STATE { down = 0, up };
    STATE state = 0;

    void Start()
    {
        button = this.GetComponent<ButtonObject>();
        topPos = obj.transform.position.y;
    }

    void Update()
    {
        if(button.IsActive)
        {
            Move();
        }
        else { DefaultMove(); }
    }

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

        if (isMax) { StartCoroutine(StateChange(STATE.down)); }
        if (isMin) { StartCoroutine(StateChange(STATE.up)); }

        switch (state)
        {
            case STATE.down:
                if(!isMin)
                obj.transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
            case STATE.up:
                if (!isMax)
                    obj.transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;
        }
    }

    /// <summary>
    /// 初期位置に戻る
    /// </summary>
    void DefaultMove()
    {
        if (obj.transform.position.y >= topPos)
        {
            isMax = true;
        }

        if(!isMax) obj.transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator StateChange(STATE s)
    {
        yield return new WaitForSeconds(coolTime);

        state = s;
        isMax = false;
        isMin = false;
    }
}