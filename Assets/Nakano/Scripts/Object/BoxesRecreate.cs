using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesRecreate : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject prefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Recreate(Vector3 tr)
    {
        StartCoroutine(recreate(tr));
    }

    IEnumerator recreate(Vector3 tr)
    {
        yield return new WaitForSeconds(5.0f);
        Instantiate(prefab, tr, Quaternion.identity, parent.transform);
    }
}
