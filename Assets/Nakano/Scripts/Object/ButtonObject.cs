using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタン本体の制御
/// </summary>
public class ButtonObject : MonoBehaviour
{
    public enum BUTTON { red = 0, blue };
    public BUTTON buttonType = 0;

    Vector3 defaultPosition;

    [SerializeField, Header("木箱との判定をなしにする")] bool isBox = false;
    [SerializeField, Header("Debug用"), Tooltip("チェックを入れるとボタンが作動")] bool isActive = false;

    Vector3 pushPos = new Vector3(0, -0.1f, 0); //押したとき、ボタン部分がどのくらい下に移動するか

    PlayerController playerController;

    /// <summary>
    /// ボタンが作動しているときはtrue
    /// </summary>
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();

        //元々のボタン部分の座標を保存
        defaultPosition = this.transform.position;
    }

    void Update()
    {
        //押されてるときはボタン部分が下に移動
        if(isActive)
        {
            transform.position = defaultPosition + pushPos;
        }

        //押されていない
        else
        {
            //青ボタンの場合、元の位置に戻る
            if (buttonType == ButtonObject.BUTTON.blue)
            {
                if (transform.position.y <= defaultPosition.y)
                {
                    transform.position = defaultPosition;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //判定相手がプレイヤー
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーが人型のとき
            if (playerController.playerstate == PlayerController.PlayerState.Human)
            {
                //押す
                isActive = true;
            }
            else
            {
                //カプセル状態なら押さない
                isActive = false;
            }
        }

        //判定相手が木箱かつ、木箱と判定を取る場合
        if (collision.gameObject.CompareTag("Box") && !isBox)
        {
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //青ボタンのときはプレイヤーや木箱が離れたらoff
        if (buttonType == ButtonObject.BUTTON.blue)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
            {
                isActive = false;
            }
        }
    }
}
