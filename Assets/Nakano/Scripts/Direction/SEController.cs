using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEController : MonoBehaviour
{
    [SerializeField] AudioClip itemCatch;
    [SerializeField] AudioClip boxDestroy;
    [SerializeField] AudioClip boardDestroy;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ItemCatch()
    {
        audioSource.PlayOneShot(itemCatch);
    }

    public void BoxDestroy()
    {
        audioSource.PlayOneShot(boxDestroy);
    }

    public void BoardDestroy()
    {
        audioSource.PlayOneShot(boardDestroy);
    }
}
