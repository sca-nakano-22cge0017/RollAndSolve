using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    [SerializeField] Text clearText;
    Animator textAnim;

    bool isClear;
    TreasureController treasureController;

    void Start()
    {
        treasureController = GameObject.FindWithTag("Treasure").GetComponent<TreasureController>();
        textAnim = clearText.GetComponent<Animator>();
        clearText.enabled = false;
    }

    void Update()
    {
        isClear = treasureController.IsClear;
        if(isClear)
        {
            clearText.enabled = true;
            textAnim.SetTrigger("TextMove");
        }
    }

    IEnumerator ToSelect()
    {
        yield return new WaitForSeconds(2);
        //SceneManager.LoadScene("Select");
    }
}
