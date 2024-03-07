using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 木箱が壁を貫通しないように判定する
/// 木箱の子オブジェクトにColliderと共に付属
/// </summary>
public class BoxStopCheck : MonoBehaviour
{
    Box box;
    public enum RL { left = 0, right }; //木箱の右側・左側を設定
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
                //右側のColliderが壁にぶつかったら右への移動停止
                case RL.right:
                    box.canMoveR = false;
                    break;

                //左側のColliderが壁にぶつかったら左への移動停止
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
