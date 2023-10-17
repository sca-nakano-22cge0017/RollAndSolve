using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] float countSpeed;

    int score;
    int lateScore;

    bool isCount;

    void Start()
    {
        //ここでGameManagerなどで計算しているScoreを取得する

        score = 0; //GameManagerなどで計算しているScoreを入れる
        lateScore = score;

        scoreText.text = score.ToString();

        isCount = false;
    }

    void Update()
    {
        //ここでGameManagerなどで計算しているScoreを取得する

        if(lateScore != score && !isCount)
        {
            isCount = true;
            StartCoroutine(countUp());
        }

        if (Input.GetKeyDown(KeyCode.A)) //Debug
        {
            score += 100;
        }
    }

    IEnumerator countUp()
    {
        while (score > lateScore)
        {
            lateScore++;
            scoreText.text = lateScore.ToString();
            yield return new WaitForSeconds(countSpeed);
        }
        isCount = false;
    }
}
