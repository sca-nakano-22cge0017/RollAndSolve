using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HPController : MonoBehaviour
{
    [SerializeField] Image[] heart;
    [SerializeField] Image[] emptyHeart;
    int hp;
    int lateHp;
    int hpLimit;
    int lateHpLimit;
    [SerializeField, Header("初期HP")] int defaultHp;
    [SerializeField, Header("ゲームオーバーに遷移するまでの時間")] float gameoverTime;

    //プレイヤーからもらう
    bool isDamage = false;
    bool isHeal = false;
    bool isLimitBreak = false;

    bool isDown = false;
    bool isFall = false;

    public bool IsDamage
    {
        set { isDamage = value; }
    }

    public bool IsDown
    {
        get { return isDown; }
        set { isDown = value; }
    }

    public bool IsFall
    {
        get { return isFall; }
        set { isFall = value; }
    }

    public bool IsHeal
    {
        set { isHeal = value; }
    }

    public int Hp
    {
        get { return hp;}
    }

    public int HpLimit
    {
        get { return hpLimit;}
        set { hpLimit = value; }
    }

    void Start()
    {
        hp = defaultHp;
        hpLimit = defaultHp;
        lateHp = hp;
        lateHpLimit = hpLimit;

        Display(emptyHeart, emptyHeart.Length, false);
        Display(emptyHeart, hpLimit, true);

        Display(heart, heart.Length, false);
        Display(heart, hp, true);
    }

    void Update()
    {
        //ダメージ
        if (isDamage)
        {
            isDamage = false;
            hp--;
            if(hp <= 0)
            {
                hp = 0;
            }
        }

        //回復
        if (isHeal)
        {
            isHeal = false;
            hp++;
            if (hp >= hpLimit)
            {
                hp = hpLimit;
            }
        }

        //上限解放
        if (isLimitBreak)
        {
            isLimitBreak = false;
            hpLimit++;
            if(hpLimit >= emptyHeart.Length)
            {
                hpLimit = emptyHeart.Length;
            }
        }

        //表示
        if (lateHpLimit != hpLimit)
        {
            Display(emptyHeart, emptyHeart.Length, false);
            Display(emptyHeart, hpLimit, true);

            lateHpLimit = hpLimit;
        }

        if (lateHp != hp)
        {
            Display(heart, heart.Length, false);
            Display(heart, hp, true);

            lateHp = hp;
        }

        //ゲームオーバー判定
        if(hp <= 0)
        {
            isDown = true;
        }

        if(isDown)
        {
            StartCoroutine(ToGameOverScene());
        }

        if(isFall)
        {
            SceneManager.LoadScene("GameOverScense");
        }
    }

    void Display(Image[] image, int num, bool isDisp)
    {
        for (int i = 0; i < num; i++)
        {
            image[i].enabled = isDisp;
        }
    }

    IEnumerator ToGameOverScene()
    {
        //倒れるアニメーションを再生するための待ち時間
        yield return new WaitForSeconds(gameoverTime);

        SceneManager.LoadScene("GameOverScense");
    }
}
