using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    HPController HpController;

    void Start()
    {
        this.HpController = FindObjectOfType<HPController>();
    }

    void Update()
    {
        //if (HpController.IsDown)
        //{
        //    SceneManager.LoadScene("GameOverScense");
        //}
    }
}
