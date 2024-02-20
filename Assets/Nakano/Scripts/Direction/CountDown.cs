using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    Animator anim;
    PlayerController player;
    bool isPause = false;
    [HideInInspector] public bool gameStart = false;
    bool canStart = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();

        StartCoroutine(Countdown());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
        }

        if(isPause) anim.SetFloat("Speed", 0);
        else anim.SetFloat("Speed", 1);

        if (gameStart && canStart)
        {
            player.IsPause = false;
            Time.timeScale = 1;

            canStart = false;
            player.CountEnd = true;
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForEndOfFrame();

        anim.SetBool("Start", true);
        player.IsPause = true;
        Time.timeScale = 0;
    }

    public void CountEnd()
    {
        gameStart = true;
    }

    public void Stop()
    {
        isPause = true;
    }

    public void Restart()
    {
        isPause = false;
    }
}
