using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BonusStage�̃A�j���[�V�����E��ʑJ��
/// </summary>
public class BonusClear : MonoBehaviour
{
    [SerializeField] Animator[] start;
    [SerializeField] Animator thank;

    [SerializeField] GameObject fade;

    PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.IsPause = true;

        //�B���A�C�e���擾����������
        PlayerPrefs.SetInt("SecretCoin", 0);

        Time.timeScale = 0;
        StartCoroutine(BonusStart());
    }

    void Update()
    {
        //BonusStage�A�j���[�V�����I����A�v���C���[�𓮂�����悤�ɂ���
        if (start[0].GetCurrentAnimatorStateInfo(0).IsName("BonusStart"))
        {
            Time.timeScale = 1;
        }
    }

    IEnumerator BonusStart()
    {
        yield return new WaitForEndOfFrame();
        //Animation��UpdateMode��UnscaledTime�ɕς�����ԂŁA
        //Start()�ōĐ�����Ɨ\�����Ȃ������ɂȂ�̂Ń����N�b�V��������

        start[0].SetBool("Start", true);
        start[1].SetBool("Start", true);

        yield return new WaitForSeconds(1.54f);

        playerController.IsPause = false;
    }

    public void Clear()
    {
        Time.timeScale = 0;

        thank.SetBool("Clear", true);
        playerController.IsPause = true;

        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(2);

        //�~�`�t�F�[�h�A�E�g
        fade.SetActive(true);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Title");
    }
}
