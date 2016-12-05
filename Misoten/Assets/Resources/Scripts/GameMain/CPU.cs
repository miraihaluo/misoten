﻿using UnityEngine;
using System.Collections;

public class CPU : MonoBehaviour
{

    /// <summary>
    /// プレイヤーの状態を表す用の列挙型
    /// </summary>
    private enum E_STATUS
    {
        ACTIVE,	// 通常
        DAMAGE,	// 攻撃被ヒット時の子供を奪われる状態
        CRASH,	// 子供を奪われた状態

        MAX
    }

    /// <summary>
    /// 点滅の状態を表す用の列挙型
    /// </summary>
    private enum E_FLASHING
    {
        NORMAL,
        CHANGE,

        MAX
    }

    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    [SerializeField, Header("プレイヤーの状態")]
    private E_STATUS eStatus = E_STATUS.ACTIVE;

    /// <summary>
    /// 点滅の状態
    /// </summary>
    private E_FLASHING eFlash = E_FLASHING.NORMAL;

    /// <summary>
    /// 攻撃被ヒット後の無敵状態の有無
    /// </summary>
    private bool muteki_f = false;

    /// <summary>
    /// 秒間の移動の加速量。。
    /// インスペクターで設定する
    /// </summary>
    [SerializeField, Header("秒間の移動の加速量")]
    private float moveAcceleration;

    /// <summary>
    /// 現在の移動量
    /// </summary>
    //[SerializeField, Header("現在の速度")]
    private Vector3 moveSpeed;

    /// <summary>
    /// 移動の最高速度
    /// </summary>
    [SerializeField, Header("移動の最高速度")]
    private float moveMaxspeed;

    /// <summary>
    /// 移動の減速の割合
    /// </summary>
    [SerializeField, Header("移動の減速割合")]
    private float moveResistivity;

    /// <summary>
    /// 秒間の回転の加速量。
    /// インスペクターで設定する
    /// </summary>
    [SerializeField, Header("秒間の回転の加速量")]
    private float rotationAcceleration;

    /// <summary>
    /// 現在の回転量
    /// </summary>
    //[SerializeField, Header("現在の回転量")]
    private Vector3 rotationSpeed;

    /// <summary>
    /// 回転の最高速度
    /// </summary>
    [SerializeField, Header("回転の最高速度")]
    private float rotationMaxspeed;

    /// <summary>
    /// 回転の減速の割合
    /// </summary>
    [SerializeField, Header("回転の減速割合")]
    private float rotationResistivity;

    /// <summary>
    /// 浮遊の現在の速度
    /// </summary>
    //[SerializeField, Header("現在の浮遊の速度")]
    private float floating;

    /// <summary>
    /// 浮遊の加速度
    /// </summary>
    [SerializeField, Header("浮遊の加速度")]
    private float floatingAcceleration;

    /// <summary>
    /// 上下移動のの減衰値
    /// </summary>
    [SerializeField, Header("上下移動のの減衰値")]
    private float floatingResistivity;

    /// <summary>
    /// 上下移動の最高速度
    /// </summary>
    [SerializeField, Header("上下移動の最高速度")]
    private float floatingMaxspeed;

    /// <summary>
    /// 上下移動の加速度
    /// </summary>
    [SerializeField, Header("上下移動の加速度")]
    private float floatingMoveAcceleration;


    /// <summary>
    /// 子供を奪われる状態の時間。
    /// インスペクターで設定
    /// </summary>
    [SerializeField, Header("子供が奪われる状態の時間（秒）")]
    private float damageTimePerSec;

    /// <summary>
    /// 子供を奪われる状態の残り時間
    /// </summary>
    private float remainingDamageTime;

    /// <summary>
    /// 子供を奪われた後の硬直時間。
    /// インスペクターで設定
    /// </summary>
    [SerializeField, Header("子供が奪われた後の硬直時間（秒）")]
    private float crashTimePerSec;

    /// <summary>
    /// 子供を奪われた時の回転モーションの回転回数。
    /// インスペクターで設定
    /// </summary>
    [SerializeField, Header("子供が奪われた時の回転回数")]
    private float crashRotationFrequency = 5.0f;

    /// <summary>
    /// 攻撃被ヒット時の点滅間隔。
    /// インスペクターで設定
    /// </summary>
    [SerializeField, Header("点滅の間隔（秒）")]
    private float flashingOnCrashTimePreSec;

    /// <summary>
    /// 攻撃被ヒット時の点灯時間
    /// </summary>
    private float remainingFlashingTime;

    /// <summary>
    /// X方向のAxisを格納する　範囲-1～1
    /// </summary>
    private float axisX;

    /// <summary>
    /// Y方向のAxisを格納する　範囲-1～1
    /// </summary>
    private float axisY;

    /// <summary>
    /// Y軸の上下するAxisを格納する　範囲-1～1
    /// </summary>
    private float axisUpDown;

    /// <summary>
    /// シーンコントローラーオブジェクト
    /// </summary>
    private GameMainSceneController sceneObj;

    /// <summary>
    /// 子供生成オブジェ
    /// </summary>
    private ChildObjCreatePoint childObjCreatePointObj;

    /// <summary>
    /// 攻撃オブジェクトのプレハブ
    /// </summary>
    //	private GameObject prefabAttackObj;

    /// <summary>
    /// 生成した攻撃オブジェクトの一時取得用
    /// </summary>
    //	private GameObject instansAttackObj;

    /// <summary>
    /// 攻撃可能かどうかのフラグ
    /// </summary>
    //	private bool enableAttack_f = true;

    /// <summary>
    /// rootの参照
    /// </summary>
    private GameObject rootObj;

    /// <summary>
    /// 水鉄砲パーティクルオブジェ
    /// </summary>
    private ParticleSystem attackWaterObj;

    /// <summary>
    /// 自身のリジッドボディ
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// レンダラーオブジェ配列
    /// </summary>
    private Renderer rendererObj;

    /// <summary>
    /// ダメージを受けた時の、点滅の色
    /// </summary>
    //	[SerializeField]
    private Color damageColor = Color.cyan;

    // 移動限界値の最小最大値　正四角形が大前提
    private float max;
    private float min;

    // 移動限界位置一時格納用ベクトル
    private Vector3 limitPos = Vector3.one;

    // シーンを跨ぐプレイヤーデータを扱うアセットデータ
    private PlayerData playerData;
    private DefineData defineData;

    /// <summary>
    /// 一度に持てる子供の最大数
    /// </summary>
    private const int SCORE_MAX = 99;

    /// <summary>
    /// 現在持っている子供の人数
    /// </summary>
    [SerializeField, Header("今持っている子供の人数")]
    private int score = 0;
    public int Score { get { return score; } }


    // Use this for initialization
    void Start()
    {

    }
    void Awake()
    {
        // アセットの参照受け取り
        playerData = Resources.Load<PlayerData>("Assets/PlayerData");
        defineData = Resources.Load<DefineData>("Assets/DefineData");

        // ゲームオブジェクトの参照受け取り
        sceneObj = FindObjectOfType<GameMainSceneController>();
        childObjCreatePointObj = FindObjectOfType<ChildObjCreatePoint>();

        // プレハブの読み込み
        //		prefabAttackObj = (GameObject)Resources.Load("Prefabs/GameMain/AttackObj");

        // パーティクルオブジェ取得
        foreach (Transform child in transform)
            if (child.name == "AttackWater")
                attackWaterObj = child.GetComponent<ParticleSystem>();


        // 移動限界値の設定
        max = defineData.STAGE_BOARDER_SIZE / 2.0f;
        min = -(defineData.STAGE_BOARDER_SIZE / 2.0f);

        // rootの参照を取得
        foreach (Transform child in transform)
            if (child.name == "root")
                rootObj = child.gameObject;

        // リジッドボディの取得
        rigidbody = GetComponent<Rigidbody>();

        // レンダラーコンポーネントの取得
        rendererObj = GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        switch (eStatus)
        {
            // 通所状態
            case E_STATUS.ACTIVE:
                // 入力を取る
                thought();

                // 攻撃処理
                AttackAction();

                // 移動と回転の計算
                //				transform.Rotate(0.0f, axisX * Time.deltaTime * rotationSpeed, 0.0f, Space.Self);	// ローカル回転
                //				transform.Translate(0.0f, axisUpDown * moveSpeed * Time.deltaTime, axisY * Time.deltaTime * moveSpeed);
                //transform.Rotate(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z, Space.Self);	// ローカル回転
                //transform.Translate(0.0f,moveSpeed.y, moveSpeed.z);

                //速度の代入
                rigidbody.velocity = (new Vector3(0, moveSpeed.y, 0) + transform.forward * moveSpeed.z) + HitBackSpeed;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //回転のＸ、Ｚ軸を0に固定、freezeだとずれる
                rigidbody.angularVelocity = (new Vector3(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z));


                break;

            // 子供を奪われる状態
            case E_STATUS.DAMAGE:
                attackWaterObj.Stop();

                remainingDamageTime -= Time.deltaTime;
                if (remainingDamageTime < 0)
                {
                    rendererObj.materials[0].color = Color.white;
                    rendererObj.materials[1].color = Color.white;
                    eStatus = E_STATUS.ACTIVE;
                    break;

                }

                remainingFlashingTime -= Time.deltaTime;
                if (remainingFlashingTime < 0)
                {
                    remainingFlashingTime = flashingOnCrashTimePreSec;
                    eFlash = (E_FLASHING)(((int)eFlash + 1) % (int)E_FLASHING.MAX);

                }

                switch (eFlash)
                {
                    case E_FLASHING.NORMAL:
                        rendererObj.materials[0].color = Color.white;
                        rendererObj.materials[1].color = Color.white;

                        break;

                    case E_FLASHING.CHANGE:
                        rendererObj.materials[0].color = damageColor;
                        rendererObj.materials[1].color = damageColor;

                        break;

                }

                break;

            // 子供を奪われた後の状態
            case E_STATUS.CRASH:
                remainingDamageTime -= Time.deltaTime;

                if (remainingDamageTime < 0)
                {
                    //rootObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                    rendererObj.materials[0].color = Color.white;
                    rendererObj.materials[1].color = Color.white;
                    eStatus = E_STATUS.ACTIVE;
                }

                //rootObj.transform.rotation = Quaternion.Euler(0, remainingDamageTime / crashTimePerSec * crashRotationFrequency, 0);

                remainingFlashingTime -= Time.deltaTime;
                if (remainingFlashingTime < 0)
                {
                    remainingFlashingTime = flashingOnCrashTimePreSec;
                    eFlash = (E_FLASHING)(((int)eFlash + 1) % (int)E_FLASHING.MAX);

                }

                switch (eFlash)
                {
                    case E_FLASHING.NORMAL:
                        rendererObj.materials[0].color = Color.white;
                        rendererObj.materials[1].color = Color.white;

                        break;

                    case E_FLASHING.CHANGE:
                        rendererObj.materials[0].color = damageColor;
                        rendererObj.materials[1].color = damageColor;

                        break;

                }

                break;

        }

        //計算用関数
        speedCalculation();

        // 移動範囲のチェック
        CheckLimitPos();

    }

    /// <summary>
    /// 移動限界エリアの判定
    /// </summary>
    private void CheckLimitPos()
    {
        // 各成分に、限界値内に収めた値を代入する
        limitPos.x = Mathf.Clamp(transform.position.x, min, max);
        limitPos.y = Mathf.Clamp(transform.position.y, 5, 100);
        limitPos.z = Mathf.Clamp(transform.position.z, min, max);

        // 現在位置ベクトルに、矯正した位置ベクトルを代入
        transform.position = limitPos;

    }

    /// <summary>
    /// 攻撃関係の処理
    /// </summary>
    private void AttackAction()
    {


    }


    /// <summary>
    /// 攻撃を当てられた時の処理
    /// </summary>
    public void AttackDamage()
    {
        //		AddScore(-1);

        remainingDamageTime = damageTimePerSec;
        remainingFlashingTime = flashingOnCrashTimePreSec;
        eStatus = E_STATUS.DAMAGE;

    }

    /// <summary>
    /// 攻撃オブジェの削除
    /// テスト時のものなので今は使っていません
    /// </summary>
    public void DestroyAttackObj()
    {
        Destroy(GetComponentInChildren<AttackObj>().gameObject);
        //		enableAttack_f = true;

    }

    /// <summary>
    /// スコアを加算、減算する
    /// </summary>
    /// <param name="addScore">スコアに加算する値</param>
    /// <returns>true：成功　false：失敗</returns>
    private bool AddScore(int addScore)
    {
        if (score + addScore > SCORE_MAX) return false;
        if (score + addScore < 0) return false;

        score += addScore;

        return true;

    }

    /// <summary>
    /// 状態を返す
    /// </summary>
    /// <returns>状態をint返す</returns>
    public int GetStatus() { return (int)eStatus; }

    /// <summary>
    /// 子供を奪われた時の処理
    /// </summary>
    public void RobbedChildren()
    {
        eStatus = E_STATUS.CRASH;

    }

    //移動、回転、上下降速度計算
    void speedCalculation()
    {
        float absNum;
        int key_flag = 1;
        if (KeyStop != 0)
            key_flag = 0;

        //回転速度計算 ============== 

        //____減衰計算
        rotationSpeed.y = rotationSpeed.y - rotationSpeed.y * rotationResistivity * Time.deltaTime;
        //____加速計算
        rotationSpeed.y += (axisX * key_flag) * Time.deltaTime * rotationAcceleration;
        //____速度制限
        absNum = System.Math.Abs(rotationSpeed.y);
        if (rotationMaxspeed < absNum)
            rotationSpeed.y = rotationMaxspeed * (rotationSpeed.y / absNum);

        //移動速度計算 ============== 

        //____減衰計算
        moveSpeed.z = moveSpeed.z - moveSpeed.z * moveResistivity * Time.deltaTime;
        //____加速計算
        moveSpeed.z += (axisY * key_flag) * Time.deltaTime * moveAcceleration;
        //____速度制限
        absNum = System.Math.Abs(moveSpeed.z);
        if (moveMaxspeed < absNum)
            moveSpeed.z = moveMaxspeed * (moveSpeed.z / absNum);




        //浮遊計算 ==============       

        //____減衰計算
        moveSpeed.y = moveSpeed.y - moveSpeed.y * floatingResistivity * Time.deltaTime;

        //キー入力があれば浮遊を停止する
        if (key_flag != 0 && axisUpDown != 0)
        {
            moveSpeed.y += axisUpDown * Time.deltaTime * floatingMoveAcceleration;
        }
        else
        {
            if (Mathf.Abs(floatingAcceleration) < Mathf.Abs(floating))
            {
                floating = floatingAcceleration;
                floatingAcceleration *= -1;
            }
            floating += floatingAcceleration * Time.deltaTime;
            moveSpeed.y += floating;
        }

        //____速度制限
        absNum = System.Math.Abs(moveSpeed.y);
        if (floatingMaxspeed < absNum)
            moveSpeed.y = floatingMaxspeed * (moveSpeed.y / absNum);


        //反射速度計算

        HitBackSpeed = HitBackSpeed - (HitBackSpeed * HitBackResistivity * Time.deltaTime);

        if (KeyStop != 0)
        {
            KeyStop -= Time.deltaTime;
            if (0.016 > KeyStop)
                KeyStop = 0;

        }
        return;
    }


    //反射での速度
    private Vector3 HitBackSpeed;

    [SerializeField, Header("弾力,反射時の速度の反射率(壁との判定用) ")]
    private float Elasticity;
    [SerializeField, Header("弾力,反射時の速度の反射率(プレイヤとの判定用) ")]
    private float PlayerElasticity;
    [SerializeField, Header("反射速度の減衰値）")]
    private float HitBackResistivity;

    //key入力の禁止時間の残り時間
    private float KeyStop;

    [SerializeField, Header("反射時のkey入力の禁止時間")]
    private float KeyStopTime;

    [SerializeField, Header("プレイヤーと当たった時のノックバック速度")]
    private float PlayerHitBackSpeed;

    Vector3 HitBackPlayer(PlayerControl PC, ContactPoint contact)
    {
        Vector3 speed = (new Vector3(0, moveSpeed.y, 0) + transform.forward * moveSpeed.z) + HitBackSpeed;

        Vector3 vec = PC.transform.position - transform.position;

        speed = Vector3.Reflect(speed, contact.normal) * PlayerElasticity + PC.rigidbody.velocity + Vector3.Reflect(vec.normalized * PlayerHitBackSpeed, contact.normal);

        return speed;

    }


    void OnCollisionEnter(Collision aite)
    {
        ContactPoint contact = aite.contacts[0];
        switch (aite.transform.tag)
        {


            case "Player":
                PlayerControl PC = aite.transform.GetComponent<PlayerControl>();

                Vector3 speed = HitBackPlayer(PC, contact);

                KeyStop = KeyStopTime;
                HitBackSpeed = speed;
                moveSpeed = new Vector3(0, 0, 0);
                //   rigidbody.velocity = HitBackSpeed;

                break;

            default:
                HitBackSpeed = (new Vector3(0, moveSpeed.y, 0) + transform.forward * moveSpeed.z) + HitBackSpeed;


                moveSpeed = new Vector3(0, 0, 0);
                rotationSpeed = new Vector3(0, 0, 0);
                HitBackSpeed = Vector3.Reflect(HitBackSpeed, contact.normal) * Elasticity;

                KeyStop = KeyStopTime;
                rigidbody.velocity = HitBackSpeed;


                break;

        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Child")
        {
            if (AddScore(1))
            {
                childObjCreatePointObj.DestroyChild(uint.Parse(other.transform.name));

            }

        }

        if (other.tag == "Goal")
        {
            playerData.AddPlayerScoreArray(int.Parse(transform.name) - 1, score);
            sceneObj.PlayerRankUpdate();
            score = 0;

        }

        if (other.tag == "Player")
        {

            // 自分が通常状態じゃなければ除外
            if (eStatus != E_STATUS.ACTIVE) return;

            // 所持子供数がマックスなら除外
            if (score == SCORE_MAX) return;

            PlayerControl playerObj = other.GetComponent<PlayerControl>();


            // 相手がダメージ状態じゃなければ除外
            if ((E_STATUS)playerObj.GetStatus() != E_STATUS.DAMAGE) return;

            // 相手の所持子供人数を1減らして、自分の所持子供人数を1増やす
            if (playerObj.AddScore(-1))
            {
                playerObj.RobbedChildren();
                score++;

            }

        }

    }

    void OnParticleCollision(GameObject obj)
    {
        if (obj.transform.parent == this.transform) return;	// 自分の出したパーティクルなら除外
        if (muteki_f) return;						// 無敵フラグがtureなら除外
        if (eStatus != E_STATUS.ACTIVE) return;		// 状態が通常時以外なら除外

        // 当たった時の処理
        AttackDamage();

    }


 /*
    private float axisX;

    private float axisY;

    private float axisUpDown;    
  */
/*
    必要な情報
        プレイヤ位置
        子供の位置
        壁の認識
        
*/

    public GameObject ko1;
    public GameObject ko2;
    bool moku_flag=true;
    float hosei = 1f;
    public float move;
   public float angle;

    void thought(){
        axisUpDown = 0;
        axisX = 0;
        axisY = 0;

        Vector3 pos;
        if(moku_flag)
            pos = ko1.transform.position;
        else
            pos = ko2.transform.position;

         move = Vector3.Distance(transform.position,pos);

         angle = Vector3.Angle(transform.position,pos);
         rot = Math.atan2(y, x) * 180 / Math.PI;
         angle = (transform.eulerAngles.y + angle)%360;

        if (hosei > System.Math.Abs(move) + System.Math.Abs(rigidbody.velocity.z) + System.Math.Abs(rigidbody.velocity.x))
        {
            axisUpDown = -1; 
        }
        else
        {
            

            if (System.Math.Abs(angle) > 31 )
                axisX = 1 - 2*(angle/180);
            if (System.Math.Abs(angle) < 30)
                axisY = 1;

        }



    }

}



