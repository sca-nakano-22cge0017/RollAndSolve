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
        //‚±‚±‚ÅGameManager‚È‚Ç‚ÅŒvŽZ‚µ‚Ä‚¢‚éScore‚ðŽæ“¾‚·‚é

        score = 0; //GameManager‚È‚Ç‚ÅŒvŽZ‚µ‚Ä‚¢‚éScore‚ð“ü‚ê‚é
        lateScore = score;

        scoreText.text = score.ToString();

        isCount = false;
    }

    void Update()
    {
        //‚±‚±‚ÅGameManager‚È‚Ç‚ÅŒvŽZ‚µ‚Ä‚¢‚éScore‚ðŽæ“¾‚·‚é

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
