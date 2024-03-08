using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// HP管理、ゲームオーバー管理
/// </summary>
public class HPController : MonoBehaviour
{
    [SerializeField] Image[] heart;
    [SerializeField] Image[] emptyHeart;
    int hp; //HP
    int lateHp; //前フレームのHP
    int hpLimit; //HP上限
    int lateHpLimit; //前フレームのHP上限
    [SerializeField, Header("初期HP")] int defaultHp;
    [SerializeField, Header("ゲームオーバーに遷移するまでの時間")] float gameoverTime;

    //プレイヤーからもらう
    bool isDamage = false; //ダメージ
    bool isHeal = false; //回復
    bool isLimitBreak = false; //HP上限解放

    bool isDown = false; //HP0
    bool isFall = false; //落下

    PlayerController player;

    /// <summary>
    /// ダメージ
    /// </summary>
    public bool IsDamage
    {
        set { isDamage = value; }
    }

    /// <summary>
    /// HP0
    /// </summary>
    public bool IsDown
    {
        get { return isDown; }
        set { isDown = value; }
    }

    /// <summary>
    /// 落下
    /// </summary>
    public bool IsFall
    {
        get { return isFall; }
        set { isFall = value; }
    }

    /// <summary>
    /// 回復
    /// </summary>
    public bool IsHeal
    {
        set { isHeal = value; }
    }

    /// <summary>
    /// HP上限解放
    /// </summary>
    public bool IsLimitBreak
    {
        set { isLimitBreak = value; }
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

        player = GameObject.FindObjectOfType<PlayerController>();

        PlayingStage();
    }

    void Update()
    {
        //ダメージ
        if (isDamage)
        {
            isDamage = false;
            hp--;

            //ゲームオーバー判定
            if (hp <= 0)
            {
                hp = 0;
                isDown = true;
            }
        }

        //回復
        if (isHeal && hp < hpLimit)
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

            //解放上限以上には増えない
            if(hpLimit >= emptyHeart.Length)
            {
                hpLimit = emptyHeart.Length;
            }
        }

        //前フレームからHPに変更があれば表示をし直す
        if (lateHpLimit != hpLimit)
        {
            //空のハートをすべて消す
            Display(emptyHeart, emptyHeart.Length, false);

            //現在のHP上限の数だけ再表示
            Display(emptyHeart, hpLimit, true);

            lateHpLimit = hpLimit;
        }
        if (lateHp != hp)
        {
            //ハートをすべて消す
            Display(heart, heart.Length, false);

            //現在のHPの数だけ再表示
            Display(heart, hp, true);

            lateHp = hp;
        }

        //HPが0になったら
        if(isDown)
        {
            StartCoroutine(ToGameOverScene());
        }

        //画面外へ落下したら
        if(isFall)
        {
            SceneManager.LoadScene("GameOverScense");
        }
    }

    /// <summary>
    /// イラストの表示・非表示
    /// </summary>
    /// <param name="image">対象のイラスト</param>
    /// <param name="num">表示・非表示にする数</param>
    /// <param name="isDisp">trueのとき表示、falseのとき非表示</param>
    void Display(Image[] image, int num, bool isDisp)
    {
        for (int i = 0; i < num; i++)
        {
            image[i].enabled = isDisp;
        }
    }

    /// <summary>
    /// ゲームオーバー画面へ移行
    /// </summary>
    IEnumerator ToGameOverScene()
    {
        //倒れるアニメーションを再生するための待ち時間
        yield return new WaitForSeconds(gameoverTime);

        SceneManager.LoadScene("GameOverScense");
    }

    /// <summary>
    /// プレイ中のステージを保存 ゲームオーバー画面からリトライするため
    /// </summary>
    void PlayingStage()
    {
        //現在のシーン名を取得
        var sceneName = SceneManager.GetActiveScene().name;

        //保存
        PlayerPrefs.SetString("PlayingStage", sceneName);
    }
}
