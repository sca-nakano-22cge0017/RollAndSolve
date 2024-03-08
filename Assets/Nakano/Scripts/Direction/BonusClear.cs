using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BonusStage�̃A�j���[�V�����E��ʑJ��
/// </summary>
public class BonusClear : MonoBehaviour
{
    [SerializeField] Animator thank;
    [SerializeField] GameObject fade;

    PlayerController playerController;

    [SerializeField] float stopPos;

    bool isMove = false;

    PauseController pauseController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        pauseController = GameObject.FindObjectOfType<PauseController>();

        fade.SetActive(false);

        //�B���A�C�e���擾����������
        PlayerPrefs.SetInt("SecretCoin", 0);
    }

    void Update()
    {
        //��ʒ����ֈړ�
        if(isMove)
        {
            if(playerController.gameObject.transform.position.x <= stopPos)
            {
                playerController.gameObject.transform.Translate(new Vector3(10, 0, 0) * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// �{�[�i�X�X�e�[�W�N���A
    /// �{�[�i�X�X�e�[�W�Ō�̃R�C�����l��������Ăяo��
    /// </summary>
    public void Clear()
    {
        //�v���C���[����s�� Animation�Đ��ׂ̈�timeScale��1
        pauseController.Pause(true, 1);

        //�N���A���o�Đ�
        thank.SetBool("Clear", true);

        //��ʒ��S�ֈړ�
        isMove = true;

        StartCoroutine(SceneChange());
    }

    /// <summary>
    /// �V�[���J��
    /// </summary>
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(2);

        //�t�F�[�h�C���Đ�
        fade.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        //�J��
        SceneManager.LoadScene("Title");
    }
}
