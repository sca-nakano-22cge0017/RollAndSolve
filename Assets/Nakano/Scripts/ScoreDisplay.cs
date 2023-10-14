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
        //‚±‚±‚ÅGameManager‚È‚Ç‚ÅŒvŽZ‚µ‚Ä‚¢‚éScore‚ðŽæ“¾‚·‚é

        score = 0; //GameManager‚È‚Ç‚ÅŒvŽZ‚µ‚Ä‚¢‚éScore‚ð“ü‚ê‚é
        lateScore = score;

        scoreText.text = score.ToString();
    }

    void Update()
    {
        //‚±‚±‚ÅGameManager‚È‚Ç‚ÅŒvŽZ‚µ‚Ä‚¢‚éScore‚ðŽæ“¾‚·‚é

        if(lateScore != score)
        {
            scoreText.text = score.ToString();
            lateScore = score;
        }
    }
}
