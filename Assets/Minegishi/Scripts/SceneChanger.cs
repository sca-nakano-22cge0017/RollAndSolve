using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    PlayerController player;

    void Start()
    {
        this.player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (player.IsDead)
        {
            SceneManager.LoadScene("GameOverScense");
        }
    }
}
