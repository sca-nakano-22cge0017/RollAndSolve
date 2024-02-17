using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneBlockCheck : MonoBehaviour
{
    [SerializeField, Header("”»’è‚ð–³‚­‚µ‚½‚¢Collider")] Collider2D[] col;
    bool isThrough = false;

    PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if(isThrough)
        {
            foreach(Collider2D c in col)
            {
                c.enabled = false;
            }
        }
        else
        {
            foreach (Collider2D c in col)
            {
                if(c.enabled == false)
                c.enabled = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(playerController.playerstate == PlayerController.PlayerState.Human)
            {
                isThrough = true;
            }
            if(playerController.playerstate == PlayerController.PlayerState.Circle)
            {
                isThrough = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerController.playerstate == PlayerController.PlayerState.Human)
        {
            isThrough = false;
        }
    }
}
