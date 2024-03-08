using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// HP�Ǘ��A�Q�[���I�[�o�[�Ǘ�
/// </summary>
public class HPController : MonoBehaviour
{
    [SerializeField] Image[] heart;
    [SerializeField] Image[] emptyHeart;
    int hp; //HP
    int lateHp; //�O�t���[����HP
    int hpLimit; //HP���
    int lateHpLimit; //�O�t���[����HP���
    [SerializeField, Header("����HP")] int defaultHp;
    [SerializeField, Header("�Q�[���I�[�o�[�ɑJ�ڂ���܂ł̎���")] float gameoverTime;

    //�v���C���[������炤
    bool isDamage = false; //�_���[�W
    bool isHeal = false; //��
    bool isLimitBreak = false; //HP������

    bool isDown = false; //HP0
    bool isFall = false; //����

    PlayerController player;

    /// <summary>
    /// �_���[�W
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
    /// ����
    /// </summary>
    public bool IsFall
    {
        get { return isFall; }
        set { isFall = value; }
    }

    /// <summary>
    /// ��
    /// </summary>
    public bool IsHeal
    {
        set { isHeal = value; }
    }

    /// <summary>
    /// HP������
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
        //�_���[�W
        if (isDamage)
        {
            isDamage = false;
            hp--;

            //�Q�[���I�[�o�[����
            if (hp <= 0)
            {
                hp = 0;
                isDown = true;
            }
        }

        //��
        if (isHeal && hp < hpLimit)
        {
            isHeal = false;
            hp++;
            if (hp >= hpLimit)
            {
                hp = hpLimit;
            }
        }

        //������
        if (isLimitBreak)
        {
            isLimitBreak = false;
            hpLimit++;

            //�������ȏ�ɂ͑����Ȃ�
            if(hpLimit >= emptyHeart.Length)
            {
                hpLimit = emptyHeart.Length;
            }
        }

        //�O�t���[������HP�ɕύX������Ε\����������
        if (lateHpLimit != hpLimit)
        {
            //��̃n�[�g�����ׂď���
            Display(emptyHeart, emptyHeart.Length, false);

            //���݂�HP����̐������ĕ\��
            Display(emptyHeart, hpLimit, true);

            lateHpLimit = hpLimit;
        }
        if (lateHp != hp)
        {
            //�n�[�g�����ׂď���
            Display(heart, heart.Length, false);

            //���݂�HP�̐������ĕ\��
            Display(heart, hp, true);

            lateHp = hp;
        }

        //HP��0�ɂȂ�����
        if(isDown)
        {
            StartCoroutine(ToGameOverScene());
        }

        //��ʊO�֗���������
        if(isFall)
        {
            SceneManager.LoadScene("GameOverScense");
        }
    }

    /// <summary>
    /// �C���X�g�̕\���E��\��
    /// </summary>
    /// <param name="image">�Ώۂ̃C���X�g</param>
    /// <param name="num">�\���E��\���ɂ��鐔</param>
    /// <param name="isDisp">true�̂Ƃ��\���Afalse�̂Ƃ���\��</param>
    void Display(Image[] image, int num, bool isDisp)
    {
        for (int i = 0; i < num; i++)
        {
            image[i].enabled = isDisp;
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[��ʂֈڍs
    /// </summary>
    IEnumerator ToGameOverScene()
    {
        //�|���A�j���[�V�������Đ����邽�߂̑҂�����
        yield return new WaitForSeconds(gameoverTime);

        SceneManager.LoadScene("GameOverScense");
    }

    /// <summary>
    /// �v���C���̃X�e�[�W��ۑ� �Q�[���I�[�o�[��ʂ��烊�g���C���邽��
    /// </summary>
    void PlayingStage()
    {
        //���݂̃V�[�������擾
        var sceneName = SceneManager.GetActiveScene().name;

        //�ۑ�
        PlayerPrefs.SetString("PlayingStage", sceneName);
    }
}
