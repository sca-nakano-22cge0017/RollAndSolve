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
        hp = 3; //あとでplayerControllerのhpを入れるように変更
        lateHp = hp;
    }

    void Update()
    {
        //PlayerのHPを取得

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

        if(Input.GetKeyDown(KeyCode.A)) //Debug
        {
            hp--;
        }
    }
}
