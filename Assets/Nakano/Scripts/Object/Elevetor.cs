using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G���x�[�^�[
/// </summary>
public class Elevetor : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ�����I�u�W�F�N�g")] GameObject obj;
    [SerializeField, Tooltip("�ړ����x")] float speed;
    [SerializeField, Tooltip("��ԏ�/���ɒ����Ă���܂������n�߂�܂ł̃N�[���^�C��")] float coolTime;
    [SerializeField, Header("��ԏ��Y���W")] float topPos;
    [SerializeField, Header("��ԉ���Y���W")] float downPos;

    ButtonObject button;

    bool isMax = false; //��ԏ�̂Ƃ�true
    bool isMin = false; //��ԉ��̂Ƃ�true

    /// <summary>
    /// ��ԉ��܂ŗ�����true
    /// �G���x�[�^�[�̉��ӂ������ɂԂ�������u��ԉ��ɓ��B�����v�Ƃ������Ƃɂ��A�ړ����~�߂�
    /// </summary>
    public bool IsMin
    {
        get { return isMin; }
        set { isMin = value; }
    }

    enum STATE { down = 0, up }; //���~���/�㏸���
    STATE state = 0;

    void Start()
    {
        button = this.GetComponent<ButtonObject>();
        topPos = obj.transform.position.y;
    }

    void Update()
    {
        //�{�^����������Ă���Ԃ͓���
        if(button.IsActive)
        {
            Move();
        }
        else { DefaultMove(); }
    }

    /// <summary>
    /// �㉺�ړ�
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

        //��������܂ňړ�������state��ς���
        if (isMax) { StartCoroutine(StateChange(STATE.down)); }
        if (isMin) { StartCoroutine(StateChange(STATE.up)); }

        //state�ɂ���Ĉړ�������ς���
        switch (state)
        {
            //���~���
            case STATE.down:
                if(!isMin) //��������Ȃ��Ƃ����~
                    obj.transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
            //�㏸���
            case STATE.up:
                if (!isMax) //�������Ȃ��Ƃ��㏸
                    obj.transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;
        }
    }

    /// <summary>
    /// �����ʒu�ɖ߂�
    /// </summary>
    void DefaultMove()
    {
        //��ԏ�ɍs�������~
        if (obj.transform.position.y >= topPos)
        {
            isMax = true;
        }

        if(!isMax) obj.transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator StateChange(STATE s)
    {
        yield return new WaitForSeconds(coolTime);
        
        state = s; //state��ύX
        isMax = false;
        isMin = false;
    }
}