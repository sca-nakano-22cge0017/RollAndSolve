using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Box box;
    HPController HpController;

    //アニメーション
    Animator anim;
    [SerializeField] GameObject[] playerForms;
    [SerializeField] Animator[] playerAnims;
    [SerializeField] MeshRenderer[] playerMeshs;
    ChangeAnimEnd changeAnimEnd;

    public enum PlayerState { Human, Circle}
    public PlayerState playerstate;

    //ポーズ状態のときtrue
    bool isPause = false;
    public bool IsPause { get { return isPause;} set { isPause = value;} }

    //カウントダウン終了のフラグ
    bool countEnd = false;
    public bool CountEnd { set { countEnd = value; } }

    private Rigidbody2D rb;

    //坂を登る時に使用する　移動角度
    float angle = 0.0f;

    private float speed = 0;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private bool objectBreak = false;
    public bool ObjectBreak
    {
        get { return objectBreak;}
        set { objectBreak = value;}
    }

    bool isRight = false;
    bool isLeft = false;

    [Header("加速度")]
    [SerializeField] private float HumansAccelertion; //人形態の時の加速度
    [SerializeField] private float CirclesAccelertion; //球体形態の時の加速度

    private float HumansSpeed = 0.0f;
    private float CirclesSpeed = 0.0f;

    private float HumansSpeedUp = 0.0f;
    private float CirclesSpeedUp = 0.0f;

    [Header("最高速度")]
    [SerializeField] private float HumansMaxSpeed;
    [SerializeField] private float CirclesMaxSpeed;

    [Header("減速度")]
    [SerializeField] private float HumansDeceleration;
    [SerializeField] private float CirclesDeceleration;
    bool RightDeceleration = false;
    bool LeftDeceleration  = false;

    [Header("人型ブレーキ")]
    [SerializeField] private float Brake;

    [Header("ジャンプ力")]
    [SerializeField] private float HumansJump = 400f; //人形態のときのジャンプ力
    [SerializeField] private float CirclesJump = 300f; //球体形態のときのジャンプ力
    private float jumpForce;

    bool speedUp = false;
    float speedUpCount = 7.0f; //スピードアップのアイテムを取った時の上昇する時間
    [SerializeField, Header("スピードアップ倍率")] float speedUpNum = 1.2f; 

    bool isGround = false;
    
    [Header("サウンド")]
    [SerializeField] AudioClip Move;
    [SerializeField] AudioClip Jump;
    [SerializeField] AudioClip Box;
    AudioSource audioSource;
    float soundSpan = 0.0f;
    bool run = false;
    
    bool invincible = false; //無敵状態
    float invincibleTime = 3.0f; //無敵時間
    //int alpha = 255;
    float interval = 0.15f;

    //木箱を押す
    bool isPushing = false; //木箱を押している最中ならtrue
    [SerializeField, Header("木箱を押すときの長押し必要時間")] float pushTime = 0.2f;
    float pTime = 0;
    bool isPushCount = false;

    [SerializeField, Header("変身エフェクト")] ParticleSystem changeEffect;
    [SerializeField, Header("風エフェクト")] ParticleSystem windEffect;

    void Start()
    {
        this.HpController = FindObjectOfType<HPController>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        playerstate = PlayerState.Circle;
        HumansSpeed = HumansAccelertion; //速度初期化
        CirclesSpeed = CirclesAccelertion; //速度初期化

        HumansSpeedUp = HumansAccelertion * speedUpNum; //スピードアップした時の速度
        CirclesSpeedUp = CirclesAccelertion * speedUpNum; //スピードアップした時の速度

        //Spine
        anim = playerAnims[0];
        //現在の形態以外はMeshRendererをオフにして非表示にする
        playerMeshs[0].enabled = true; //カプセル
        playerMeshs[1].enabled = false; //右向き人型
        playerMeshs[2].enabled = false; //左向き人型

        changeAnimEnd = playerForms[1].GetComponent<ChangeAnimEnd>();
    }

    void Update()
    {
        //Hpが0じゃないとき　ポーズ状態じゃないとき
        if (!HpController.IsDown && !isPause)
        {
            Run();
            FormChange();
            Push();
            PlayerJump();
            MoveSound();
        }

        if(HpController.IsDown) anim.SetBool("Dead", true); //HP0になったらダウンアニメーション再生

        if (speedUp)
        {
            SpeedUp();
        }

        if (invincible) //無敵状態
        {
            invincibleTime -= Time.deltaTime;
            if(invincibleTime > 0)
            {
                Invincible();
            }
            else if(invincibleTime <= 0)
            {
                invincibleTime = 3.0f;
                invincible = false;
            }
        }

        //球体形態で最高速度の７割以上の時オブジェクトを破壊できる
        if(Mathf.Abs(speed) >= CirclesMaxSpeed * 0.7 && playerstate == PlayerState.Circle)
        {
            objectBreak = true;
            windEffect.Play(); //移動エフェクト
        }
        else
        {
            objectBreak = false;
            windEffect.Stop();
        }

        if (isPause)
        {
            windEffect.Stop();
            changeEffect.Stop();
        }

        //カウントダウン中にADキーが押され、そのままゲームが開始すると最初動かないのでそれの解決
        //カウントダウンが終わっていないとき
        if (!countEnd)
        {
            //ADキーが押されたら左右方向へ移動可能にしておく
            if (Input.GetKeyDown(KeyCode.D))
            {
                isRight = true;
                isLeft = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                isRight = false;
                isLeft = true;
            }
        }
    }

    //人型
    void Human()
    {
        //speed = HumansSpeed;
        jumpForce = HumansJump;
    }

    //球体
    void Circle()
    {
        //speed = CirclesSpeed;
        jumpForce = CirclesJump;
    }

    /// <summary>
    /// 変形 Wキーで人型<->球体
    /// </summary>
    void FormChange()
    {
        switch (playerstate)
        {
            case PlayerState.Human:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    anim.SetBool("Change", true);
                    anim.SetBool("Dash", false);
                    anim.SetBool("Push", false);

                    StartCoroutine(ToBall());
                }
                Human();
                break;
            case PlayerState.Circle:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    StartCoroutine(ToHuman());
                }
                Circle();
                break;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Run()
    {
        if(speed >= 1.0f || speed <= -1.0f)
        {
            run = true;
        }

        // 右方向へ移動
        // 押した瞬間に移動方向上書き
        if (Input.GetKeyDown(KeyCode.D))
        {
            isRight = true;
            isLeft = false;
            if(speed < 0)
                speed = 0;
        }

        //Dキー入力かつ移動方向が右
        if (Input.GetKey(KeyCode.D) && isRight)
        {
            //エフェクトの向き変更
            windEffect.transform.localPosition = new Vector3(-7.0f, -6.5f, 0);
            windEffect.transform.rotation = Quaternion.Euler(angle, -90.0f, 90.0f);

            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                if (speed <= -3)
                {
                    speed = -Brake;
                }
                speed += HumansSpeed * Time.deltaTime;

                //速度上限
                if (speed >= HumansMaxSpeed)
                {
                    speed = HumansMaxSpeed;
                }
            }
            else if(playerstate == PlayerState.Circle)
            {
                speed += CirclesSpeed * Time.deltaTime;

                //速度上限
                if (speed > CirclesMaxSpeed)
                {
                    speed += CirclesDeceleration * Time.deltaTime;
                }
                if (speed >= CirclesMaxSpeed)
                {
                    speed = CirclesMaxSpeed;
                }

                playerForms[0].GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }
            //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0,0) * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            //減速
            RightDeceleration = true;

            isRight = false;
            isLeft = true;

            //方向転換
            if (Input.GetKey(KeyCode.A) && speed > 0) speed = 0;
        }

        // 右方向減速
        if (RightDeceleration)
        {
            //Debug.Log("Dを離す");
            if (speed > 0)
            {
                if (playerstate == PlayerState.Human)
                {
                    speed -= HumansSpeed * Time.deltaTime;
                }
                else if (playerstate == PlayerState.Circle)
                {
                    speed -= CirclesDeceleration * Time.deltaTime;
                }

                //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
            }
            else speed = 0; //超過分を戻す
        }

        // 左方向へ移動 
        // 押した瞬間に移動方向上書き
        if (Input.GetKeyDown(KeyCode.A))
        {
            isLeft = true;
            isRight = false;
            if(speed > 0)
                speed = 0;
        }

        if (Input.GetKey(KeyCode.A) && isLeft)
        {
            //エフェクトの向き変更
            windEffect.transform.localPosition = new Vector3(7.0f, -6.5f, 0);
            windEffect.transform.rotation = Quaternion.Euler(-angle, 90.0f, -90.0f);

            RightDeceleration = false;
            LeftDeceleration = false;

            if (playerstate == PlayerState.Human)
            {
                if (speed >= 3)
                {
                    speed = Brake;
                }
                speed -= HumansSpeed * Time.deltaTime;

                //速度上限
                if (speed <= -HumansMaxSpeed)
                {
                    speed = -HumansMaxSpeed;
                }
            }
            else if (playerstate == PlayerState.Circle)
            {
                speed -= CirclesSpeed * Time.deltaTime;

                //速度上限
                if (speed < CirclesMaxSpeed)
                {
                    speed -= CirclesDeceleration * Time.deltaTime;
                }
                if (speed <= -CirclesMaxSpeed)
                {
                    speed = -CirclesMaxSpeed;
                }

                playerForms[0].GetComponent<Transform>().localScale = new Vector3(-1f, 1f, 1f);
            }
            //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            LeftDeceleration = true;
            isRight = true;
            isLeft = false;

            //方向転換
            if (Input.GetKey(KeyCode.D) && speed < 0) speed = 0;
        }

        // 左方向減速
        if (LeftDeceleration)
        {
            //Debug.Log("Aを離す");
            if (speed < 0)
            {
                if (playerstate == PlayerState.Human)
                {
                    speed += HumansSpeed * Time.deltaTime;
                }
                else if (playerstate == PlayerState.Circle)
                {
                    speed += CirclesDeceleration * Time.deltaTime;
                }

                //transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
            }
            else speed = 0;
        }

        //複数個所に書かれていたのをまとめた
        transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);

        //if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        //{
        //    //左方向に移動していたら
        //    if (speed < 0)
        //    {
        //        //逆方向に速度追加
        //        if (playerstate == PlayerState.Human)
        //        {
        //            speed += HumansSpeed * Time.deltaTime;
        //        }
        //        else if (playerstate == PlayerState.Circle)
        //        {
        //            speed += CirclesSpeed * Time.deltaTime;
        //        }
        //        transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
        //    }
        //    if (speed > 0)
        //    {
        //        if (playerstate == PlayerState.Human)
        //        {
        //            speed -= HumansSpeed * Time.deltaTime;
        //        }
        //        else if (playerstate == PlayerState.Circle)
        //        {
        //            speed -= CirclesSpeed * Time.deltaTime;
        //        }
        //        transform.Translate(Quaternion.Euler(0, 0, angle) * new Vector3(speed, 0, 0) * Time.deltaTime);
        //    }
        //    if (playerstate == PlayerState.Human)
        //    {
        //        speed = 0;
        //    }
        //}

        Spine();
    }

    /// <summary>
    /// 木箱を押すアニメーションの再生・停止、木箱の移動
    /// </summary>
    void Push()
    {
        if (isPushing)
        {
            //押しているor押し始めのアニメーションが再生中でなければ
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("push") && !anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
                anim.SetBool("Push", true); //押し始めのアニメーションに遷移

            //木箱移動
            if (Input.GetKey(KeyCode.D) && box != null)
            {
                box.BoxRightMove();
            }

            if(Input.GetKey(KeyCode.A) && box != null)
            {
                box.BoxLeftMove();
            }

            //木箱引きずりのSE
            if (soundSpan >= 0)
            {
                soundSpan -= Time.deltaTime;
            }

            if (soundSpan <= 0.0f)
            {
                audioSource.PlayOneShot(Box);
                soundSpan = 1.752f;
            }
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("push") || anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
                anim.SetBool("Push", false);
        }

        if(isPushCount)
        {
            pTime += Time.deltaTime;

            if (pTime >= pushTime) isPushing = true;
            else isPushing = false;
        }
        else pTime = 0;

        //キーから手が離れているとき またはAとDを両方押しているとき木箱を押せない
        if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            isPushCount = false;
            isPushing = false;
            box = null;
            anim.SetBool("Push",false);
        }
    }

    //プレイヤーイラスト位置調整用変数
    Vector3 pushAjustR = new Vector3(5.0f, -3.0f, 0); //木箱を押しているとき
    Vector3 pushAjustL = new Vector3(-5.0f, -3.0f, 0);
    Vector3 dashAjust = new Vector3(0, -2.0f, 0); //走っているとき
    Vector3 normalAjust = new Vector3(0, -1.0f, 0); //通常状態

    /// <summary>
    /// 一部Spineの制御
    /// </summary>
    void Spine()
    {
        //プレイヤー位置調整　Spineの都合上浮いて見えるので 左右方向それぞれのイラストを位置調整する
        for(int i = 1; i <= 2; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("push") || anim.GetCurrentAnimatorStateInfo(0).IsName("push_motion"))
            {
                if(speed >= 0)
                    playerForms[i].transform.localPosition = pushAjustR;
                else playerForms[i].transform.localPosition = pushAjustL;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("dash") || anim.GetCurrentAnimatorStateInfo(0).IsName("dash_motion"))
            {
                playerForms[i].transform.localPosition = dashAjust;
            }
            else
            {
                playerForms[i].transform.localPosition = normalAjust;
            }
        }

        //ADキーが押されたとき、アニメーション・表示イラストを左右切り替える
        if(playerstate == PlayerState.Human)
        {
            //キーを押した瞬間、逆方向のキーが押されていない状態のとき
            if (Input.GetKeyDown(KeyCode.A) || (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                AnimFlipped("left");
            }
            if (Input.GetKeyDown(KeyCode.D) || (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)))
            {
                AnimFlipped("right");
            }
        }

        //走りモーション
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
            {
                anim.SetBool("Dash", true);
            }
        }

        //一定速度以下でアニメーションを停止する
        float animMinSpeed = 5.0f;
        if (speed <= animMinSpeed && speed >= -animMinSpeed && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Dash", false);
        }

        //速度に合わせて走りモーションの速度を上昇
        anim.SetFloat("Speed", Mathf.Abs(speed) * 0.1f + 1);
    }

    /// <summary>
    /// 人型アニメーションの左右反転
    /// </summary>
    /// <param name="key">入力されたキー</param>
    void AnimFlipped(string leftOrRight)
    {
        int lastAnim = 0; //前のアニメーション
        int nextAnim = 0; //次のアニメーション

        switch (leftOrRight)
        {
            //左移動
            case "left":
                lastAnim = 1;
                nextAnim = 2;
                break;
            //右移動
            case "right":
                lastAnim = 2;
                nextAnim = 1;
                break;
        }

        //アニメーションを初期状態にする
        playerAnims[lastAnim].SetBool("Change", false);
        playerAnims[lastAnim].SetBool("Jump", false);
        playerAnims[lastAnim].SetBool("Dash", false);

        anim = playerAnims[nextAnim]; //逆向きのアニメーションを操作できるように変更
        playerMeshs[nextAnim].enabled = true; //逆向きのイラストにする
        changeAnimEnd = playerForms[nextAnim].GetComponent<ChangeAnimEnd>(); //アニメーション終了判定を貰うスクリプトを変える

        //他のイラストは非表示
        playerMeshs[0].enabled = false;
        playerMeshs[lastAnim].enabled = false;
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            anim.SetBool("Jump", true);
            this.rb.AddForce(transform.up * jumpForce);
            audioSource.PlayOneShot(Jump);
            //Debug.Log(jumpForce);
        }
    }

    /// <summary>
    /// 移動音
    /// </summary>
    private void MoveSound()
    {
        //再生が終了したらまた再生
        if(soundSpan >= 0)
        {
            soundSpan -= Time.deltaTime;
        }

        //球体形態の時に移動中再生
        if (playerstate == PlayerState.Circle && speed != 0 && soundSpan <= 0)
        {
            audioSource.PlayOneShot(Move);
            soundSpan = 0.68f;
        }

        if (run)
        {
            if (speed <= 0.8f && speed >= -0.8f) //スピードが-0.5〜0.5になったらストップ
            {
                audioSource.Stop();
                soundSpan = 0.0f;
            }
        }

    }

    /// <summary>
    /// 速度上昇アイテム取得時の処理
    /// </summary>
    private void SpeedUp()
    {
        speedUpCount -= Time.deltaTime;
        if (speedUpCount >= 0)
        {
            //速度が1.2倍になる
            HumansSpeed = HumansSpeedUp;
            CirclesSpeed = CirclesSpeedUp;
        }
        else if (speedUpCount < 0)
        {
            //カウントが0になったら速度が元の戻る
            HumansSpeed = HumansAccelertion;
            CirclesSpeed = CirclesAccelertion;

            speedUpCount = 7.0f;
            speedUp = false;
        }
    }

    /// <summary>
    /// 無敵状態
    /// </summary>
    private void Invincible()
    {
        interval -= Time.deltaTime;
        
        if(interval <= 0)
        {
            //if(alpha == 255)
            //{
            //    alpha = 0;
            //}else
            //    alpha = 255;
            interval = 0.15f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //着地アニメーション
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }

        if (collision.gameObject.tag == "Ground")
        {
            angle = 0;
        }

        if (collision.gameObject.tag == "Box")
        {
            //箱を破壊
            if (Input.GetKey(KeyCode.D) && objectBreak)
            {
                speed -= speed * 0.2f;
                //Debug.Log("箱を破壊");
                //Destroy(collision.gameObject);
            }
            if (Input.GetKey(KeyCode.A) && objectBreak)
            {
                speed -= speed * 0.2f;
                //Debug.Log("箱を破壊");
                //Destroy(collision.gameObject);
            }

            //着地アニメーション 木箱の上での着地判定
            foreach (ContactPoint2D contact in collision.contacts)
            {
                var hitPoint = contact.point;
                var sub = hitPoint.y - transform.position.y;

                if (sub < -0.7f)
                {
                    isGround = true;
                    anim.SetBool("Jump", false);
                }
            }
        }

        if (collision.gameObject.tag == "Slope")
        {
            var slope = collision.gameObject.GetComponent<Slope>();
            angle = slope.Angle;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = true;
        }

        if (collision.gameObject.tag == "Wall")
        {
            speed = 0.0f; //壁に当たったら速度をリセット
        }

        //坂に当たったら坂を上るための角度を取得
        if (collision.gameObject.tag == "Slope")
        {
            var slope = collision.gameObject.GetComponent<Slope>();
            angle = slope.Angle;
            rb.gravityScale = 0;
        }

        if (collision.gameObject.tag == "Box")
        {
            foreach(ContactPoint2D contact in collision.contacts)
            {
                //衝突位置を取得
                var hitPoint = contact.point;
                var sub = hitPoint.y - transform.position.y;

                //左右に木箱があったら
                if (sub <= 0.7f && sub >= -0.7f)
                {
                    //左右方向でぶつかったら止まる
                    speed = 0.0f;

                    //人形態の時に箱に接触しているとき
                    if (playerstate == PlayerState.Human)
                    {
                        //D長押しで右に木箱を押す
                        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                        {
                            isPushCount = true;

                            var obj = collision.gameObject;
                            box = obj.GetComponent<Box>();
                            speed = 1.0f;
                        }

                        //A長押しで左に木箱を押す
                        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                        {
                            isPushCount = true;

                            var obj = collision.gameObject;
                            box = obj.GetComponent<Box>();
                            speed = -1.0f;
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" ||
            collision.gameObject.tag == "Slope")
        {
            isGround = false;
        }

        if(collision.gameObject.tag == "Box")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                var hitPoint = contact.point;
                var sub = hitPoint.y - transform.position.y;

                //木箱の上に立っていた場合
                if (sub < -0.7f)
                {
                    isGround = false;
                }
            }

            isPushing = false;
            isPushCount = false;
            anim.SetBool("Push", false);
        }

        if (collision.gameObject.tag == "Slope")
        {
            rb.gravityScale = 2;
            angle = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible) //無敵状態じゃないとき
        {
            //衝突相手が敵かつ人型かつ速度７割未満のとき　または衝突相手がトゲのとき、ダメージ処理
            if ((collision.gameObject.tag == "Enemy" && !objectBreak) || collision.gameObject.tag == "Thorn")
            {
                //カプセル状態解除
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                    anim = playerAnims[1];
                    playerMeshs[1].enabled = true;
                    playerMeshs[0].enabled = false;
                    playerMeshs[2].enabled = false;
                }

                speed -= speed * 0.5f;
                invincible = true;
                HpController.IsDamage = true;

                anim.SetTrigger("Damage");
            }
        }

        //穴に落ちた
        if (collision.gameObject.tag == "Hole")
        {
            HpController.IsFall = true;
        }

        if (collision.gameObject.tag == "SpeedUP") //スピードアップ
        {
            //Destroy(collision.gameObject);
            speedUpCount = 7.0f;
            speedUp = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!invincible)
        {
            if (collision.gameObject.tag == "Poison")
            {
                anim.SetTrigger("Damage");
                //カプセル状態解除
                if (playerstate == PlayerState.Circle)
                {
                    playerstate = PlayerState.Human;
                    anim = playerAnims[1];
                    playerMeshs[1].enabled = true;
                    playerMeshs[0].enabled = false;
                    playerMeshs[2].enabled = false;
                }

                speed -= speed * 0.5f;
                invincible = true;
                HpController.IsDamage = true;
            }
        }
    }

    IEnumerator ToBall()
    {
        //アニメーションが終わるまで待機
        yield return new WaitUntil(() => changeAnimEnd.IsEnd);

        //球体->人型になったとき、変形モーションの再生から始まらないように
        playerAnims[1].SetBool("Change", false);
        playerAnims[2].SetBool("Change", false);

        //操作するアニメーションを変更する
        anim = playerAnims[0];

        playerMeshs[0].enabled = true;
        playerMeshs[1].enabled = false;
        playerMeshs[2].enabled = false;

        playerstate = PlayerState.Circle;
        //sr.sprite = Circles;
        Debug.Log("球体です");
    }

    IEnumerator ToHuman()
    {
        //フォームチェンジアニメーション終了フラグをfalseにする
        changeAnimEnd.IsEnd = false;

        //エフェクト再生
        changeEffect.Play();

        //エフェクト再生中待機
        yield return new WaitForSeconds(0.7f);

        //操作するアニメーションを変更する
        anim = playerAnims[1];

        playerMeshs[1].enabled = true;
        playerMeshs[0].enabled = false;
        playerMeshs[2].enabled = false;

        playerstate = PlayerState.Human;
        //sr.sprite = Humans;
        Debug.Log("人です");
    }
}
