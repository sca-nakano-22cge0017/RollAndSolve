using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrow : MonoBehaviour
{
    
    void Start()
    {
		// �͂������������Vector3�^�Œ�`
		// �����X������45�x�̌����Ɏˏo���邽�߁AX��Y��1:1�ɂ���
		Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

		// ��̌����ɉ����͂̑傫�����`
		float forceMagnitude = 10.0f;

		// �����Ƒ傫������Sphere�ɉ����͂��v�Z����
		Vector3 force = forceMagnitude * forceDirection;

		// Sphere�I�u�W�F�N�g��Rigidbody�R���|�[�l���g�ւ̎Q�Ƃ��擾
		Rigidbody rb = gameObject.GetComponent<Rigidbody>();

		// �͂������郁�\�b�h
		// ForceMode.Impulse�͌���
		rb.AddForce(force, ForceMode.Impulse);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
