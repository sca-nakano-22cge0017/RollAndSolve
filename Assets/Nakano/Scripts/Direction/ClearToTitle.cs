using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearToTitle : MonoBehaviour
{
    [SerializeField, Header("ƒ^ƒCƒgƒ‹‚É‘JˆÚ‚·‚é‚Ü‚Å‚ÌŽžŠÔ(•b)")] float sec = 3; 

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(sec);

        if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
        {
            SceneManager.LoadScene("BonusStage");
        }
        else SceneManager.LoadScene("Title");
    }
}
