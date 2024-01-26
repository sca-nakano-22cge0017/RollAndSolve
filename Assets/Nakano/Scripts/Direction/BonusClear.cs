using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusClear : MonoBehaviour
{
    [SerializeField] Animator thank;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Clear()
    {
        thank.SetBool("Clear", true);
        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Title");
    }
}
