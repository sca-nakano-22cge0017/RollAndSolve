using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearToTitle : MonoBehaviour
{
    [SerializeField, Header("タイトルに遷移するまでの時間(秒)")] float sec = 3; 

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(sec);

        if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
        {
            SceneManager.LoadScene("BonusScene");
        }
        else SceneManager.LoadScene("Title");
    }
}
