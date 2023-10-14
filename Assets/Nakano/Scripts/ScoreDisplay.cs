using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] Text scoreText;
    int score;
    int lateScore;

    void Start()
    {
        //ここでGameManagerなどで計算しているScoreを取得する

        score = 0; //GameManagerなどで計算しているScoreを入れる
        lateScore = score;

        scoreText.text = score.ToString();
    }

    void Update()
    {
        //ここでGameManagerなどで計算しているScoreを取得する

        if(lateScore != score)
        {
            scoreText.text = score.ToString();
            lateScore = score;
        }
    }
}
