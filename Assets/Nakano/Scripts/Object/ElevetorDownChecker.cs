using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevetorDownChecker : MonoBehaviour
{
    [SerializeField] Elevetor elevetor;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        elevetor.IsMin = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        elevetor.IsMin = false;
    }
}