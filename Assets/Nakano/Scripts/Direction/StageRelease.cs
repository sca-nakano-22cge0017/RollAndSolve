using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class SecretCoins
{
    public Image[] coin;
}

public class StageRelease : MonoBehaviour
{
    [SerializeField] Animator[] release;
    [SerializeField] Animator[] bg;

    [SerializeField] SecretCoins[] stage;
    [SerializeField] Sprite[] coinImages;

    private void Awake()
    {
        Time.timeScale = 1;
        FirstClearCheck();

        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                var num = PlayerPrefs.GetInt("SecretCoin:Stage" + (i + 1).ToString() + "-SecretCoin" + (j + 1).ToString(), 0);
                stage[i].coin[j].sprite = coinImages[num];
            }
        }
    }

    void FirstClearCheck()
    {
        for (int i = 1; i <= 3; i++) //3はステージ数
        {
            int firstClear = PlayerPrefs.GetInt("FirstClear" + i.ToString(), 0);
            int clear = PlayerPrefs.GetInt("Clear" + i.ToString(), 0);

            if (clear == 1 && firstClear == 0 && i < 3) //初クリアじゃないなら
            {
                for (int j = i; j > 0; j--)
                {
                    release[j - 1].SetTrigger("Released");
                    bg[j - 1].SetTrigger("Released");
                }
            }

            if (firstClear == 1) //初クリアなら
            {
                if(i < 3)
                {
                    release[i - 1].SetTrigger("Release"); //解放演出再生
                    bg[i - 1].SetTrigger("Release");
                }
                
                PlayerPrefs.SetInt("FirstClear" + i.ToString(), 0);
            }
        }
    }
}
