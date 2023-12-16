using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCamera : MonoBehaviour
{
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.playerstate == PlayerController.PlayerState.Circle)
        {

        }
    }
}
