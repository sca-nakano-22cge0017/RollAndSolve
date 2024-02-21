using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            var name = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(name);
        }
    }
}
