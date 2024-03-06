using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseEnd : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    PlayerController playerController;

    bool gameStart = false;
    public bool GameStart { set { gameStart = value; } }

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public void OnClick()
    {
        var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
        if (isActive == true)
        {
            if (gameStart)
            {
                playerController.IsPause = false;
                Time.timeScale = 1;
            }
        }
        Panel.SetActive(!isActive);
    }
}
