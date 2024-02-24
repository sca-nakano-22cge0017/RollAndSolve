using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesRecreate : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject prefab;

    public void FallBox(Vector3 tr, bool create)
    {
        if(create)
        {
            Instantiate(prefab, tr, Quaternion.identity, parent.transform);
            create = false;
        }
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
