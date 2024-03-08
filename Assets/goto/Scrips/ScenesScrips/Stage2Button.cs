using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage2Button : MonoBehaviour
{
    [SerializeField] Animator chain;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        Time.timeScale = 1;
        if(PlayerPrefs.GetInt("Clear1", 0) == 0)
        {
            chain.SetTrigger("CantPlay");
        }
        else
        {
            SceneManager.LoadScene("Stage2");
        }
    }
}