using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SE管理
/// </summary>
public class SEController : MonoBehaviour
{
    [SerializeField, Header("アイテム取得")] AudioClip itemCatch;
    [SerializeField, Header("木箱破壊")] AudioClip boxDestroy;
    [SerializeField, Header("木の板破壊")] AudioClip boardDestroy;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// アイテム取得SE
    /// </summary>
    public void ItemCatch()
    {
        audioSource.PlayOneShot(itemCatch);
    }

    /// <summary>
    /// 木箱破壊SE
    /// </summary>
    public void BoxDestroy()
    {
        audioSource.PlayOneShot(boxDestroy);
    }

    /// <summary>
    /// 木の板破壊SE
    /// </summary>
    public void BoardDestroy()
    {
        audioSource.PlayOneShot(boardDestroy);
    }
}