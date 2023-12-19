using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevetorDownChecker : MonoBehaviour
{
    [SerializeField] Elevetor elevetor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Concrete" || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Enemy"))
        {
            elevetor.IsMin = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Concrete" || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Enemy"))
        {
            elevetor.IsMin = false;
        }
    }
}