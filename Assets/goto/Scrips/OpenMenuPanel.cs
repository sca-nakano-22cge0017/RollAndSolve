using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenuPanel : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Panel1;

    [SerializeField]
    Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            {
            var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
            Panel.SetActive(!isActive);
            if (isActive == false)
            {
                StartButton.Select();
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
               
        }


    }

    public void OnClick()
    {


        var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
        Panel.SetActive(!isActive);

       
        StartButton.Select();

    }
}
