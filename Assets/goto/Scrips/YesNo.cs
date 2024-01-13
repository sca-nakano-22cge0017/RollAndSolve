using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YesNo : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Panel1;

    [SerializeField]
    Button StartButton;

    
    public void OnClick()
    {
        var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
        Panel.SetActive(!isActive);

        var isActivee = Panel.activeInHierarchy; // Panelがアクティブか取得
        Panel1.SetActive(!isActivee);
        //EventSystem.current.SetSelectedGameObject(null);
       
        if (isActive == false)
        {
            StartButton.Select();
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

    }
}