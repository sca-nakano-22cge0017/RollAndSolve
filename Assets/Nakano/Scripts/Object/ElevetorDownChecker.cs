using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G���x�[�^�[�̉��ɉ����������Ƃ��~�܂邽�߂̓����蔻��p
/// �����ɏՓ˂����炻�����G���x�[�^�[�ړ��̉����Ƃ��āA����ȏ㉺����Ȃ��悤�ɂ���
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