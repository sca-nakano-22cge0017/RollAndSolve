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

    void Start()
    {
        //�B���A�C�e���擾����������
        PlayerPrefs.SetInt("SecretCoin", 0);

        Time.timeScale = 0;
        //Player��State�ɑ�����󂯕t���Ȃ����[�h������āA����ɕύX��������ǂ�����
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
    }

    public void Clear()
    {
        Time.timeScale = 0;

        thank.SetBool("Clear", true);
        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        //ToDo �~�`�t�F�[�h�A�E�g
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Title");
    }
}
