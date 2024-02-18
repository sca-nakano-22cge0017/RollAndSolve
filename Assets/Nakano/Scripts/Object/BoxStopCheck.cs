using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStopCheck : MonoBehaviour
{
    Box box;
    public enum RL { left = 0, right };
    public RL type = 0;


    void Start()
    {
        box = gameObject.transform.parent.GetComponent<Box>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            switch (type)
            {
                case RL.right:
                    box.canMoveR = false;
                    break;
                case RL.left:
                    box.canMoveL = false;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            switch (type)
            {
                case RL.right:
                    box.canMoveR = true;
                    break;
                case RL.left:
                    box.canMoveL = true;
                    break;
            }
        }
    }
}
