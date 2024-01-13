using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    [SerializeField] Text clearText;
    Animator textAnim;

    [SerializeField] Image effectLeft;
    [SerializeField] Image effectRight;
    Animator effectLeftAnim;
    Animator effectRightAnim;

    bool isClear;
    TreasureController treasureController;

    bool textAnimEnd = false;
    bool effectEnd = false;

    public bool TextAnimEnd
    {
        get { return textAnimEnd; }
        set { textAnimEnd = value; }
    }

    public bool EffectEnd
    {
        get { return effectEnd; }
        set { effectEnd = value; }
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
        }

        if(effectEnd)
        {
            effectLeft.enabled = false;
            effectRight.enabled = false;
            StartCoroutine(ToSelect());
        }

        //Debug用 削除必須
        if(Input.GetKey(KeyCode.C)) { StartCoroutine(ToSelect()); }
    }

    IEnumerator ToSelect()
    {
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("PlayingStage", 0);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                DataSave(1);
                SceneManager.LoadScene("StageSelect");
                break;

            case "Stage2":
                DataSave(2);
                SceneManager.LoadScene("StageSelect");
                break;

            case "Stage3":
                DataSave(3);
                SceneManager.LoadScene("ClearScene");
                break;
        }
    }

    void DataSave(int num)
    {
        //初クリアかどうかを保存
        if (PlayerPrefs.GetInt("Clear" + num.ToString(), 0) == 0)
        {
            PlayerPrefs.SetInt("FirstClear" + num.ToString(), 1);
        }

        //各ステージクリアしたかどうかを保存 boolが入れられないのでintで代用
        PlayerPrefs.SetInt("Clear" + num.ToString(), 1);

        PlayerPrefs.Save();
    }
}
