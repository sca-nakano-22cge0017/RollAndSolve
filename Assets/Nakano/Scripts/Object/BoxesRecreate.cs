using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 木箱の再生成制御
/// </summary>
public class BoxesRecreate : MonoBehaviour
{
    [SerializeField, Header("親オブジェクト")] GameObject parent;
    [SerializeField, Header("生成する木箱のPrefab")] GameObject prefab;

    float waitTime = 5.0f; //破壊から再生成までのクールタイム

    /// <summary>
    /// 木箱が落下した瞬間に再生成する
    /// </summary>
    /// <param name="tr">生成する座標</param>
    /// <param name="create">再生成するかどうか</param>
    public void FallBox(Vector3 tr, bool create)
    {
        //一度だけ再生成
        if(create)
        {
            Instantiate(prefab, tr, Quaternion.identity, parent.transform);
            create = false;
        }
    }

    /// <summary>
    /// 木箱の再生成　破壊されたタイミングで呼び出し
    /// 実際に生成するのはコルーチン
    /// </summary>
    /// <param name="tr">生成する座標</param>
    public void Recreate(Vector3 tr)
    {
        StartCoroutine(recreate(tr));
    }

    /// <summary>
    /// 木箱の再生成
    /// </summary>
    /// <param name="tr">生成する座標</param>
    /// <returns></returns>
    IEnumerator recreate(Vector3 tr)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(prefab, tr, Quaternion.identity, parent.transform);
    }
}
