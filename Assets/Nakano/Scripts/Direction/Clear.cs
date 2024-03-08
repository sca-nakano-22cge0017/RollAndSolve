using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �N���A�����o�E�V�[���J��
/// </summary>
public class Clear : MonoBehaviour
{
    //Animation
    [SerializeField] Image clearText;
    Animator textAnim;
    bool textAnimEnd = false;

    //�u�X�e�[�W�N���A�v�̕����̉��o���I���������ǂ���
    public bool TextAnimEnd
    {
        set { textAnimEnd = value; }
    }

    [SerializeField] Image effectLeft;
    [SerializeField] Image effectRight;
    Animator effectLeftAnim;
    Animator effectRightAnim;

    //�N���A����
    bool isClear;
    TreasureController treasureController;

    float waitTime = 4.0f; //�A�j���[�V�����I������V�[���J�ڂ܂ł̑҂�����

    //SE
    [SerializeField] AudioClip clearSE;
    AudioSource audioSource;
    bool isSound = true;

    PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        treasureController = GameObject.FindWithTag("Treasure").GetComponent<TreasureController>();

        audioSource = GetComponent<AudioSource>();
        textAnim = clearText.GetComponent<Animator>();
        effectLeftAnim = effectLeft.GetComponent<Animator>();
        effectRightAnim = effectRight.GetComponent<Animator>();

        clearText.enabled = false;
    }

    void Update()
    {
        //�󔠂���N���A�����Ⴄ
        isClear = treasureController.IsClear;

        //�N���A������
        if(isClear)
        {
            //��x����SE�Đ�
            if(isSound)
            {
                audioSource.PlayOneShot(clearSE);
                isSound = false;
            }
            
            //�N���A�e�L�X�g�\��
            clearText.enabled = true;
            textAnim.SetTrigger("TextMove");

            //�v���C���[����s��
            playerController.IsPause = true;
        }

        //�u�X�e�[�W�N���A�v�̕����̉��o���I�������
        if(textAnimEnd)
        {
            //�����L���L�����鉉�o���Đ�
            effectLeft.enabled = true;
            effectRight.enabled = true;
            effectLeftAnim.SetTrigger("Start");
            effectRightAnim.SetTrigger("Start");

            //�V�[���J��
            StartCoroutine(ToSelect());
        }
    }

    /// <summary>
    /// �X�e�[�W�ɉ������V�[���J�ځE�X�e�[�W�N���A�ɂ��Ă̏��ۑ�
    /// </summary>
    IEnumerator ToSelect()
    {
        yield return new WaitForSeconds(waitTime);

        //�v���C���X�e�[�W�̏���������
        PlayerPrefs.SetString("PlayingStage", "Title");

        //�V�[�����ɂ���ď�������
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                DataSave(1);
                break;

            case "Stage2":
                DataSave(2);
                break;

            case "Stage3":
                DataSave(3);
                SceneManager.LoadScene("ClearScene"); //�Q�[���N���A��ʂ�
                break;
        }
    }

    /// <summary>
    /// �X�e�[�W�N���A�ɂ��Ă̏��ۑ�
    /// </summary>
    /// <param name="num">�X�e�[�W�̔ԍ�</param>
    void DataSave(int num)
    {
        //���N���A���ǂ�����ۑ� bool�̑����int���g�p 0�̂Ƃ�false 1�̂Ƃ�true
        //Clear, FirstClear + �X�e�[�W�̔ԍ��ŕۑ�
        if (PlayerPrefs.GetInt("Clear" + num.ToString(), 0) == 0)
        {
            PlayerPrefs.SetInt("FirstClear" + num.ToString(), 1);
        }

        //�e�X�e�[�W�N���A�������ǂ�����ۑ�
        PlayerPrefs.SetInt("Clear" + num.ToString(), 1);
        PlayerPrefs.Save();

        //�X�e�[�W1�A2�̂Ƃ��A�B���R�C����9�������Ă�����{�[�i�X�X�e�[�W�֔��
        //��������Ȃ���΃X�e�[�W�I����ʂ�
        //�X�e�[�W3�̓Q�[���N���A��ʂ����ނ̂ŏ��O����
        if(num < 3)
        {
            if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
            {
                SceneManager.LoadScene("BonusStage");
            }
            else SceneManager.LoadScene("StageSelect");
        }
    }
}
