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
        //������GameManager�ȂǂŌv�Z���Ă���Score���擾����

        score = 0; //GameManager�ȂǂŌv�Z���Ă���Score������
        lateScore = score;

        scoreText.text = score.ToString();
    }

    void Update()
    {
        //������GameManager�ȂǂŌv�Z���Ă���Score���擾����

        if(lateScore != score)
        {
            scoreText.text = score.ToString();
            lateScore = score;
        }
    }
}
