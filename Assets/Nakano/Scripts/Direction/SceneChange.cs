using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �{�^���ɂ��V�[���J��
/// </summary>
public class SceneChange : MonoBehaviour
{
    public void Select()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}