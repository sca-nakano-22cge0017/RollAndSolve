using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] Image[] heart;
    [SerializeField] Image[] emptyHeart;
    int hp;
    int lateHp;
    int hpLimit;
    int lateHpLimit;
    [SerializeField, Header("����HP")] int defaultHp;

    //�v���C���[������炤
    bool isDamage = false;
    bool isHeal = false;
    bool isLimitBreak = false;

    bool isDown = false;

    public bool IsDown
    {
        get { return isDown; }
    }

    public int Hp
    {
        get { return hp;}
        set { hp = value; }
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
        //��������
        if (isDamage)
        {
            isDamage = false;
            hp--;
            if(hp <= 0)
            {
                hp = 0;
            }
        }

        if (isHeal)
        {
            isHeal = false;
            hp++;
            if (hp >= hpLimit)
            {
                hp = hpLimit;
            }
        }

        if (isLimitBreak)
        {
            isLimitBreak = false;
            hpLimit++;
            if(hpLimit >= emptyHeart.Length)
            {
                hpLimit = emptyHeart.Length;
            }
        }

        //�\��
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

        //�Q�[���I�[�o�[����
        if(hp <= 0)
        {
            isDown = true;
        }
    }

    void Display(Image[] image, int num, bool isDisp)
    {
        for (int i = 0; i < num; i++)
        {
            image[i].enabled = isDisp;
        }
    }
}
