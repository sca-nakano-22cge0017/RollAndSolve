using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectPanel : MonoBehaviour
{
    [SerializeField] 
    GameObject Panel;
    [SerializeField]
    private ActivateButton select1;

    public void Start()
    {
       
        Panel.SetActive(true);
        select1.ActivateOrNotActivate(true);

    }
}