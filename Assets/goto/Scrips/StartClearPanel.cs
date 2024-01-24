
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U

public class StartClearPanel : MonoBehaviour
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) & Panel.SetActive(true))
        {
          
            Panel.SetActive(false);
            Panel1.SetActive(true);
            StartButton.Select();
        }
      

    }

}