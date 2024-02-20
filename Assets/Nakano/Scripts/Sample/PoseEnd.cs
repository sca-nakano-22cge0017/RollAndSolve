using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseEnd : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    PlayerController playerController;

    CountDown countDown;

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();

        countDown = GameObject.FindObjectOfType<CountDown>();
    }

    public void OnClick()
    {
        var isActive = Panel.activeInHierarchy; // Panelがアクティブか取得
        if (isActive == true)
        {
            if(countDown.gameStart)
            {
                playerController.IsPause = false;
                Time.timeScale = 1;
            }
        }
        Panel.SetActive(!isActive);
    }
}
