using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevetorDownChecker : MonoBehaviour
{
    [SerializeField] Elevetor elevetor;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Concrete" || collision.gameObject.CompareTag("Box"))
        {
            elevetor.IsMin = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Concrete" || collision.gameObject.CompareTag("Box"))
        {
            elevetor.IsMin = false;
        }
    }
}