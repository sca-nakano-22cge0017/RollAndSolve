using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public bool TextAnimEnd
    {
        get { return textAnimEnd; }
        set { textAnimEnd = value; }
    }

    void Start()
    {
        treasureController = GameObject.FindWithTag("Treasure").GetComponent<TreasureController>();
        textAnim = clearText.GetComponent<Animator>();
        effectLeftAnim = effectLeft.GetComponent<Animator>();
        effectRightAnim = effectRight.GetComponent<Animator>();
        effectRightAnim = effectRight.GetComponent<Animator>();
        clearText.enabled = false;
    }

    void Update()
    {
        isClear = treasureController.IsClear;
        if(isClear)
        {
            clearText.enabled = true;
            textAnim.SetTrigger("TextMove");
        }

        if(textAnimEnd)
        {
            effectLeft.enabled = true;
            effectRight.enabled = true;
            effectLeftAnim.SetTrigger("Start");
            effectRightAnim.SetTrigger("Start");
            StartCoroutine(ToSelect());
        }

        //Debug�p �폜�K�{
        if(Input.GetKey(KeyCode.C)) { StartCoroutine(ToSelect()); }
    }

    IEnumerator ToSelect()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("PlayingStage", 0);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                DataSave(1);
                if(PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
                {
                    SceneManager.LoadScene("BonusScene");
                }
                else SceneManager.LoadScene("StageSelect");
                break;

            case "Stage2":
                DataSave(2);
                if (PlayerPrefs.GetInt("SecretCoin", 0) >= 9)
                {
                    SceneManager.LoadScene("BonusScene");
                }
                else SceneManager.LoadScene("StageSelect");
                break;

            case "Stage3":
                DataSave(3);
                SceneManager.LoadScene("ClearScene");
                break;
        }
    }

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
    }
}