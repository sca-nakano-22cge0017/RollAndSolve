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

    }
    
    public void GetButtonDown()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("StageSelect");
    }
}