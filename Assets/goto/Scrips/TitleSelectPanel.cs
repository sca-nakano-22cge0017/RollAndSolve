using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleSelectPanel : MonoBehaviour
{
    [SerializeField]
    GameObject Panel;
    [SerializeField] 
    Button StartButton;
    [SerializeField]
    private ActivateButton select1;

    public void Start()
    {

        Panel.SetActive(true);
        StartButton.Select();

    }
}