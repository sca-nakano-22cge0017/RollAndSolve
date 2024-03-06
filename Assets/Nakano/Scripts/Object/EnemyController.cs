using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Cinemachine;

/// <summary>
/// Enemy制御
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("移動速度")] float speed;
    [SerializeField, Header("追跡範囲")] float dis;
    [SerializeField, Header("左へのRayの位置")] GameObject leftRay;
    [SerializeField, Header("右へのRayの位置")] GameObject rightRay;

    [SerializeField, Header("飛んでいく方向 (ベクトル)")] Vector3 flyDir;
    Vector3 dir; //飛んでいく処理をする用の変数
    [SerializeField, Header("飛んでいく速度")] float flySpeed;
    [SerializeField, Header("飛んでいくときの敵の角度")] float flyAngle;
    bool isFly = false; //飛ばされている途中か

    PlayerController playerController;
    GameObject player;
    Vector3 playerPos;

    Collider2D col;
    Rigidbody2D rb;

    //イラストの向き
    Vector3 facingRight = new Vector3(-0.05f, 0.05f, 1); //右向き
    Vector3 facingLeft = new Vector3(0.05f, 0.05f, 1); //左向き

    Vector3 moveDir; //移動方向
    bool isChase; //プレイヤーを追いかけているかどうか

    bool isMove; //移動できるか
    bool leftHit, rightHit; //左右方向に衝突物があるか

    //Spine
    SkeletonAnimation skeletonAnimation;
    bool isWalk = false;

    //Camera
    CinemachineImpulseSource impulse;

    void Start()
    {
        //敵撃破時の画面揺れ用
        impulse = GetComponent<CinemachineImpulseSource>();

        //animation
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(0, "blessing", true);

        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        col.isTrigger = false;

        isWalk = false;
        isChase = false;
        isMove = true;

        moveDir = Vector3.right;
    }

    private void Update()
    {
        //左側のRayになにかぶつかったとき
        if (leftHit)
        {
            //移動方向が左のとき
            if (moveDir.x < 0)
                isMove = false; //動けない
        }
        else { isMove = true; }

        //右側のRayになにかぶつかったとき
        if (rightHit)
        {
            //移動方向が右のとき
            if (moveDir.x > 0)
                isMove = false;
        }
        else { isMove = true; }

        playerPos = player.transform.position;

        //左右方向の衝突を検知するRayが子オブジェクトについているため、Enemy本体が反転するとRayの向きも反転してしまうので
        //Enemyが向いてる方向に応じてRayを入れ替える
        //左向きのときは通常
        if (transform.localScale.x > 0)
        {
            leftHit = leftRay.GetComponent<EnemyRay>().isHit;
            rightHit = rightRay.GetComponent<EnemyRay>().isHit;
        }
        //右向きのとき反転
        else
        {
            leftHit = rightRay.GetComponent<EnemyRay>().isHit;
            rightHit = leftRay.GetComponent<EnemyRay>().isHit;
        }

        //プレイヤーとの直線距離算出
        var distance = (playerPos - this.transform.position).sqrMagnitude;

        //プレイヤーとの距離が一定以下だと追跡する
        if (distance <= dis * dis)
        {
            isChase = true;
        }
        else if (distance > dis * dis)
        {
            isChase = false;
        }

        //プレイヤーを追っているとき
        if (isChase)
        {
            //移動可能なとき
            if (isMove)
            {
                //移動方向算出、移動
                moveDir = (playerPos - this.transform.position).normalized;
                transform.Translate(new Vector3(moveDir.x, 0, 0) * speed * Time.deltaTime);

                //移動方向に応じてEnemyの向きを変える
                if (moveDir.x > 0)
                {
                    this.transform.localScale = facingRight;
                }
                else if (moveDir.x <= 0)
                {
                    this.transform.localScale = facingLeft;
                }
            }

            //歩きモーションを再生していない場合は再生
            if (!isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
                isWalk = true;
            }
        }

        //プレイヤーを追っていないとき
        else
        {
            //歩きモーションをしている場合は待機モーションに切り替え
            if (isWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "blessing", true);
                isWalk = false;
            }
        }

        //飛ばされているとき
        if (isFly)
        {
            //ダメージアニメーション
            skeletonAnimation.AnimationState.SetAnimation(0, "damage", true);

            //斜め上方向に飛ばす
            Transform myTrans = this.transform;
            Vector3 pos = myTrans.position;
            pos.x += dir.x * flySpeed * Time.deltaTime;
            pos.y += dir.y * flySpeed * Time.deltaTime;
            myTrans.position = pos;

            //ある程度飛んだら消す
            if (transform.position.y >= 10)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーにぶつかったとき
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーがオブジェクト破壊可能なとき
            if (playerController.ObjectBreak)
            {
                //画面揺れ再生
                impulse.GenerateImpulse();

                //吹き飛ばした後に他のものにぶつからないようにRigidbody2D、Colliderを削除
                Destroy(rb);
                Destroy(col);

                isFly = true;

                //プレイヤーが右へ移動しているとき ＝ 左側からぶつかられたとき、右へ飛んでいく
                if (playerController.Speed < 0)
                {
                    transform.Rotate(0, 0, -flyAngle, Space.World); //飛んでいく方向に向けて回転
                    dir = new Vector3(-flyDir.x, flyDir.y, flyDir.z).normalized; //左右方向だけ逆にする
                }

                //右からぶつかられたら左へ飛んでいく
                else
                {
                    transform.Rotate(0, 0, flyAngle, Space.World);
                    dir = flyDir.normalized;
                }
            }
            else
            {
                //プレイヤーがすり抜けられるように
                col.isTrigger = true;
            }
        }

        else if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //地面から離れた時は落ちるように
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //地面上にいるときは下に落ちないようにする
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            col.isTrigger = false;
        }

        //地面から離れた時は落ちるように
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
