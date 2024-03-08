using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �X�e�[�W���g���C
/// </summary>
public class Retry : MonoBehaviour
{
    string stageName = "";

    void Start()
    {
        //�v���C���Ă����X�e�[�W�̖��O���擾
        stageName = PlayerPrefs.GetString("PlayingStage", "Title");
    }

    public void OnClick()
    {
        Time.timeScale = 1; //�|�[�Y����
        StartCoroutine(Wait());
    }

    /// <summary>
    /// �V�[���J��
    /// </summary>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(stageName);
    }
}
