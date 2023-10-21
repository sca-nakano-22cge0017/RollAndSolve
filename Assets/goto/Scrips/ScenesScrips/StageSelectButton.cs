using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField]
    private EventSystem ev;

    // Start is called before the first frame update
    void Start()
    {
        ev = GetComponent<EventSystem>();

    }
    
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            
        } 
    }
    
    public void GetButtonDown()
    {
        SceneManager.LoadScene("StageSelect");
    }

   
}