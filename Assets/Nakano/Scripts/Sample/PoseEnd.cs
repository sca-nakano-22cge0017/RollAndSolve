using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseEnd : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    public void OnClick()
    {
        var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
        if (isActive == true)
        {
            Debug.Log("非表示");
            Time.timeScale = 1;
        }
        Panel.SetActive(!isActive);
    }
}
