using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SEÇ
/// </summary>
public class SEController : MonoBehaviour
{
    [SerializeField, Header("ACeæ¾")] AudioClip itemCatch;
    [SerializeField, Header("Ø jó")] AudioClip boxDestroy;
    [SerializeField, Header("ØÌÂjó")] AudioClip boardDestroy;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ACeæ¾SE
    /// </summary>
    public void ItemCatch()
    {
        audioSource.PlayOneShot(itemCatch);
    }

    /// <summary>
    /// Ø jóSE
    /// </summary>
    public void BoxDestroy()
    {
        audioSource.PlayOneShot(boxDestroy);
    }

    /// <summary>
    /// ØÌÂjóSE
    /// </summary>
    public void BoardDestroy()
    {
        audioSource.PlayOneShot(boardDestroy);
    }
}