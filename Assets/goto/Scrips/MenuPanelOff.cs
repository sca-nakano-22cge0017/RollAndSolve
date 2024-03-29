using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelOff : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Panel1;

    [SerializeField]
    Button StartButton;

    private void Start()
    {
       
    }
  
    public void OnClick()
    {
        var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
        Panel.SetActive(!isActive);

        var isActivee = Panel.activeInHierarchy; // Panelがアクティブか取得
        Panel1.SetActive(!isActivee);
        //EventSystem.current.SetSelectedGameObject(null);

        StartButton.Select();
        if (isActive == false)
        {
            Debug.Log("非表示");
            Time.timeScale = 1;
        }
    }
}