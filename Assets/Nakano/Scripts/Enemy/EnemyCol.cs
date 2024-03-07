using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の左右方向の衝突判定をとる
/// </summary>
public class EnemyCol : MonoBehaviour
{
    bool isHit = false;

    /// <summary>
    /// 障害物にぶつかったかどうかの判定
    /// </summary>
    public bool IsHit
    {
        get { return isHit; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemy本体、カメラ制御用のColliderは無視する
        if(!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Camera"))
            isHit = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Camera"))
            isHit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Camera"))
            isHit = false;
    }
}
