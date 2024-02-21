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
    [SerializeField] Image clearText;
    Animator textAnim;

    [SerializeField] Image effectLeft;
    [SerializeField] Image effectRight;
    Animator effectLeftAnim;
    Animator effectRightAnim;

    bool isClear;
    TreasureController treasureController;

    bool textAnimEnd = false;

    [SerializeField] AudioClip clearSE;
    AudioSource audioSource;
    bool isSound = true;

    //�u�X�e�[�W�N���A�v�̕����̉��o���I���������ǂ�����Ⴄ
    public bool TextAnimEnd
    {
        set { textAnimEnd = value; }
    }

    void Start()
    {
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
            if(isSound)
            {
                audioSource.PlayOneShot(clearSE);
                isSound = false;
            }
            
            clearText.enabled = true;
            textAnim.SetTrigger("TextMove");
        }

        //�u�X�e�[�W�N���A�v�̕����̉��o���I�������
        if(textAnimEnd)
        {
            //�����L���L�����鉉�o���Đ�
            effectLeft.enabled = true;
            effectRight.enabled = true;
            effectLeftAnim.SetTrigger("Start");
            effectRightAnim.SetTrigger("Start");

            StartCoroutine(ToSelect());
        }

        //Debug�p �폜�K�{
        //if(Input.GetKey(KeyCode.C)) { StartCoroutine(ToSelect()); }
    }

    //�X�e�[�W�ɉ������V�[���J�ځE�X�e�[�W�N���A�ɂ��Ă̏��ۑ�
    IEnumerator ToSelect()
    {
        yield return new WaitForSeconds(4f);
        PlayerPrefs.SetInt("PlayingStage", 0);

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
                SceneManager.LoadScene("ClearScene");
                break;
        }
    }

    /// <summary>
    /// �X�e�[�W�N���A�ɂ��Ă̏��ۑ�
    /// </summary>
    /// <param name="num">�X�e�[�W�̔ԍ�</param>
    void DataSave(int num)
    {
        //���N���A���ǂ�����ۑ�
        if (PlayerPrefs.GetInt("Clear" + num.ToString(), 0) == 0)
        {
            PlayerPrefs.SetInt("FirstClear" + num.ToString(), 1);
        }

        //�e�X�e�[�W�N���A�������ǂ�����ۑ� bool��������Ȃ��̂�int�ő�p
        PlayerPrefs.SetInt("Clear" + num.ToString(), 1);

        PlayerPrefs.Save();

        //�X�e�[�W�P�A�Q�̂Ƃ��A�B���R�C����9�������Ă�����{�[�i�X�X�e�[�W�֔��
        //��������Ȃ���΃X�e�[�W�I����ʂ�
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
