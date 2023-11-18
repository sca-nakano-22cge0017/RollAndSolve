using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverStart : MonoBehaviour
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
