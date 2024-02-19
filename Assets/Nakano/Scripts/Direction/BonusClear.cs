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

    [SerializeField] float stopPos;

    bool isMove = false;
    bool isStart = false;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.IsPause = true;

        fade.SetActive(false);

        //�B���A�C�e���擾����������
        PlayerPrefs.SetInt("SecretCoin", 0);

        Time.timeScale = 0;
        StartCoroutine(BonusStart());
    }

    void Update()
    {
        //BonusStage�A�j���[�V�����I����A�v���C���[�𓮂�����悤�ɂ���
        if (start[0].GetCurrentAnimatorStateInfo(0).IsName("BonusStart") && !isStart)
        {
            Time.timeScale = 1;
            playerController.IsPause = false;
            isStart = true;
        }

        if(isMove)
        {
            if(playerController.gameObject.transform.position.x <= stopPos)
            {
                playerController.gameObject.transform.Translate(new Vector3(10, 0, 0) * Time.deltaTime);
            }
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
        playerController.IsPause = true;
        
        thank.SetBool("Clear", true);
        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        isMove = true;
        yield return new WaitForSeconds(2);
        fade.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Title");
    }
}
