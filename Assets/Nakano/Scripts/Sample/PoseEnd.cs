using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseEnd : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public void OnClick()
    {
        var isActive = Panel.activeInHierarchy; // Panel���A�N�e�B�u���擾
        if (isActive == true)
        {
            playerController.IsPause = false;
            Time.timeScale = 1;
        }
        Panel.SetActive(!isActive);
    }
}
