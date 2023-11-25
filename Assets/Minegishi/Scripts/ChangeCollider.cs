using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollider : MonoBehaviour
{
    PlayerController playerController;
    CircleCollider2D circleCol;
    BoxCollider2D boxCol;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        circleCol = GetComponent<CircleCollider2D>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(playerController.playerstate == PlayerController.PlayerState.Circle)
        {
            boxCol.offset = new Vector2(0.2f, -5.8f);
            boxCol.size = new Vector2(27f, 27.2f);
        }
        if(playerController.playerstate == PlayerController.PlayerState.Human)
        {
            //Vector2 objectSize = gameObject.GetComponent<RectTransform>().sizeDelta;
            boxCol.offset = new Vector2(0.2f, -0.28f);
            boxCol.size = new Vector2(26f, 38.1f);
        }
    }
}
