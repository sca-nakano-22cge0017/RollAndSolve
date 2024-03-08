using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���ēǂݍ��݂��ăX�^�[�g�ɖ߂�
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
