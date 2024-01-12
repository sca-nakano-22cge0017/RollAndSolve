using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ActivateUIObj : MonoBehaviour
{

	//�@���UI
	[SerializeField]
	private GameObject statusWindow;
	//�@�{�^���̃C���^���N�e�B�u�Ɋւ��鏈����������Ă���X�N���v�g
	[SerializeField]
	private ActivateButton select1;
	[SerializeField]
	private ActivateButton select2;

    

    void Update()
	{
		//�@�X�y�[�X�L�[������������UI�̃I���E�I�t
		if (Input.GetKeyDown("space"))
		{
			statusWindow.SetActive(!statusWindow.activeSelf);

			//�@��ʂ��J��������Background1�̃{�^���̃C���^���N�e�B�u��true�ABackground2�̃{�^���̃C���^���N�e�B�u��false�ɂ���
			if (statusWindow.activeSelf)
			{

				select1.ActivateOrNotActivate(true);
				select2.ActivateOrNotActivate(false);

				//�@��ʂ������I��������
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}
	}
}