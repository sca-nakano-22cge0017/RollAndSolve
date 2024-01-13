using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage1Button : MonoBehaviour
{
    //[SerializeField]
    //Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        //StartButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        Time.timeScale =1;
        SceneManager.LoadScene("Stage1");
        PlayerPrefs.SetInt("PlayingStage", 1);
    }
}
