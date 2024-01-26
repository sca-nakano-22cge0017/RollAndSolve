using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    int stageNum = 0;
    string stageName = "";

    void Start()
    {
        stageNum = PlayerPrefs.GetInt("PlayingStage", 0);
        stageName = Stage(stageNum);
    }

    public void OnClick()
    {
        Time.timeScale = 1;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(stageName);
    }

    string Stage(int num)
    {
        string sceneName = "";

        switch(num)
        {
            case 1:
                sceneName = "Stage1";
                break;
            case 2:
                sceneName = "Stage2";
                break;
            case 3:
                sceneName = "Stage3";
                break;
            default:
                sceneName = "Title";
                break;
        }

        return sceneName;
    }
}
