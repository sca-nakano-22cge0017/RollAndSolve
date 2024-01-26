using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    HPController hpController;

    bool isDown = false;

    void Start()
    {
        
    }

    void Update()
    {
        isDown = hpController.IsDown;

        if(isDown)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
