using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SE�Ǘ�
/// </summary>
public class SEController : MonoBehaviour
{
    [SerializeField, Header("�A�C�e���擾")] AudioClip itemCatch;
    [SerializeField, Header("�ؔ��j��")] AudioClip boxDestroy;
    [SerializeField, Header("�؂̔j��")] AudioClip boardDestroy;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// �A�C�e���擾SE
    /// </summary>
    public void ItemCatch()
    {
        audioSource.PlayOneShot(itemCatch);
    }

    /// <summary>
    /// �ؔ��j��SE
    /// </summary>
    public void BoxDestroy()
    {
        audioSource.PlayOneShot(boxDestroy);
    }

    /// <summary>
    /// �؂̔j��SE
    /// </summary>
    public void BoardDestroy()
    {
        audioSource.PlayOneShot(boardDestroy);
    }
}