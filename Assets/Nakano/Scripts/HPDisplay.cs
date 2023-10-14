using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    [SerializeField] Image[] heart;
    int hp;
    int lateHp;

    //GameObject player;
    //PlayerController playerController;

    void Start()
    {
        //player = GameObject.FindWithTag("Player");
        //playerController = player.GetComponent<PlayerController>();
        hp = 5; //‚ ‚Æ‚ÅplayerController‚Ìhp‚ğ“ü‚ê‚é‚æ‚¤‚É•ÏX
        lateHp = hp;
    }

    void Update()
    {
        //Player‚ÌHP‚ğæ“¾

        if(lateHp != hp)
        {
            for(int i = 0; i < heart.Length; i++)
            {
                heart[i].enabled = false;
            }

            for (int i = 0; i < hp; i++)
            {
                heart[i].enabled = true;
            }

            lateHp = hp;
        }
    }
}
