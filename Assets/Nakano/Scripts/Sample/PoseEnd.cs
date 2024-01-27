using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseEnd : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    public void OnClick()
    {
        var isActive = Panel.activeInHierarchy; // Panel���A�N�e�B�u���擾
        if (isActive == true)
        {
            Debug.Log("��\��");
            Time.timeScale = 1;
        }
        Panel.SetActive(!isActive);
    }
}
