using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Human, Circle}
    PlayerState playerstate;

    [SerializeField] private float speed = 1.0f;

    void Start()
    {
        playerstate = PlayerState.Circle;
    }


    void Update()
    {
        Run();
        switch (playerstate)
        {
            case PlayerState.Human:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Circle;
                }
                    Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerstate = PlayerState.Human;
                }
                Circle();
                break;
        }


    }

    void Human()
    {
        speed = 1.0f;
        Debug.Log("êlÇ≈Ç∑");
    }

    void Circle()
    {
        speed = 3.0f;
        Debug.Log("ãÖëÃÇ≈Ç∑");
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed, 0,0) * Time.deltaTime) ;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }
    }
}
