using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Panel1;

    [SerializeField]
    Button StartButton;


    public void OnClick()
    {
        Time.timeScale = 0;
        var isActive = Panel.activeInHierarchy; // Panel���A�N�e�B�u���擾
        Panel.SetActive(!isActive);

        var isActivee = Panel.activeInHierarchy; // Panel���A�N�e�B�u���擾
        Panel1.SetActive(!isActivee);
        //EventSystem.current.SetSelectedGameObject(null);
        StartButton.Select();

    }
}