
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartClearPanel : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Panel1;

    [SerializeField]
    Button StartButton;


    public void OnClick()
    {
        Time.timeScale = 0;
        var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
        Panel.SetActive(!isActive);

        var isActivee = Panel.activeInHierarchy; // Panelがアクティブか取得
        Panel1.SetActive(!isActivee);
        //EventSystem.current.SetSelectedGameObject(null);
        StartButton.Select();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && Panel.activeSelf)
        {
          
            Panel.SetActive(false);
            Panel1.SetActive(true);
            StartButton.Select();
        }
      

    }

}