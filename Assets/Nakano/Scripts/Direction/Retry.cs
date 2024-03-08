using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージリトライ
/// </summary>
public class Retry : MonoBehaviour
{
    string stageName = "";

    void Start()
    {
        //プレイしていたステージの名前を取得
        stageName = PlayerPrefs.GetString("PlayingStage", "Title");
    }

    public void OnClick()
    {
        Time.timeScale = 1; //ポーズ解除
        StartCoroutine(Wait());
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(stageName);
    }
}
