using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン再読み込みしてスタートに戻る
/// </summary>
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
