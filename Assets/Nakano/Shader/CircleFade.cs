using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFade : MonoBehaviour
{
    float power;
    [SerializeField] float fadeSpeed;
    bool fadeEnd = false;

    bool fadeStart = false;

    PlayerController playerController;

    void OnEnable()
    {
        power = 1.5f;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        transform.position = playerController.transform.position;
        fadeStart = true;
    }

    void Update()
    {
        if(fadeStart)
        {
            if (power > 0)
            {
                power -= fadeSpeed * Time.deltaTime;
            }
            else
            {
                power = 0;
                fadeEnd = true;
            }

            GetComponent<Renderer>().material.SetFloat("_Power", power);
        }
    }
}
