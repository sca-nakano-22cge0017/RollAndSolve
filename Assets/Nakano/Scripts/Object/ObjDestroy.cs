using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// 木箱/木の板の破壊
/// </summary>
public class ObjDestroy : MonoBehaviour
{
    PlayerController playerController;

    Collider2D col;
    Rigidbody2D rb;

    //木箱再生成
    Vector3 tr; //再生成位置
    BoxesRecreate boxesRecreate;
    [SerializeField, Header("破壊後復活するかどうか")] bool isRecreate = true;
    bool rec = false;

    //坂を転がる
    bool isFall = false; //転がり落ちるフラグ
    bool isFallCr = false; //転がり落ちたときの復活用のフラグ
    [SerializeField, Header("坂を転がり落ちるかどうか")] bool isFallBox = false;
    [SerializeField, Header("坂を転がり落ちるときの回転速度")] float rotateSpeed = -180;

    //演出系
    Animator anim;
    CinemachineImpulseSource impulse;
    SEController seController;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        tr = GetComponent<Transform>().position;
        boxesRecreate = GameObject.FindObjectOfType<BoxesRecreate>();

        impulse = GetComponent<CinemachineImpulseSource>();
        anim = GetComponent<Animator>();
        seController = GameObject.FindObjectOfType<SEController>();

        //木の板は再生成しないので誤ってisRecreateがtrueにされないように
        if (this.gameObject.name == "Board") { isRecreate = false; }
    }

    private void Update()
    {
        //木箱の復活
        if(rec)
        {
            rec = false;
            boxesRecreate.Recreate(tr);
        }

        //坂から転がり落ちるときの処理
        if(isFall)
        {
            //一回だけ再生成
            if (!isFallCr)
            {
                boxesRecreate.FallBox(tr, true);
                isFallCr = true;
            }

            //回転
            rb.angularVelocity = rotateSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //木箱は穴に落ちたら消す
        if(collision.gameObject.tag == "Hole" && this.gameObject.tag == "Box")
        {
            //坂を転がっていないとき
            if (!isFall)
            {
                if (isRecreate) { rec = true; } //木箱復活

                StartCoroutine(BoxFall());
            }

            //坂を転がっているときは別の方法で再生成するのでそのまま消す
            else { Destroy(this); }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //プレイヤーがオブジェクト破壊可能な時
            if (playerController.ObjectBreak)
            {
                //画面揺れ
                impulse.GenerateImpulse();

                if(this.gameObject.tag == "Box")
                {
                    if (isRecreate) { rec = true; } //木箱復活

                    seController.BoxDestroy(); //破壊SE

                    //子オブジェクト削除
                    foreach (Transform n in gameObject.transform)
                    {
                        Destroy(n.gameObject);
                    }
                }

                //木の板なら復活させずにそのまま破壊
                if(this.gameObject.name == "Board")
                {
                    seController.BoardDestroy(); //破壊SE
                }
                
                anim.SetTrigger("Break"); //破壊アニメーション

                Destroy(col); //当たり判定を消す
            }
        }

        //坂に触れたらそのまま転がり落ちる
        if(collision.gameObject.CompareTag("Slope") && !isFall && isFallBox)
        {
            isFall = true; //回転させる

            rb.mass = 0.001f;
            rb.angularDrag = 0;
            rb.gravityScale = 3;

            //初期状態だと回転しないようにconstraintsが設定されているので回転するように変更
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    /// <summary>
    /// animationの再生が終わったら呼び出してオブジェクト削除
    /// </summary>
    public void ThisDestroy()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 木箱の画面外への落下
    /// </summary>
    IEnumerator BoxFall()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
