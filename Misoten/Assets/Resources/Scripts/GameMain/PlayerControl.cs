using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

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
	/// 子供ゲージのオブジェ
	/// </summary>
	[SerializeField, Header("子供ゲージのオブジェ")]
	private ChildNumGauge childNumGaugeObj;

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
	/// 頭上に表示するプレイヤーアイコンのオブジェ
	/// </summary>
	private Transform numIconObj;

	/// <summary>
	/// プレイヤーアイコンのローカル位置
	/// </summary>
	private Vector3 numIconPos;

	/// <summary>
	/// 自身のリジッドボディ
	/// </summary>
	public Rigidbody rigidbody;

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
	private const int SCORE_MAX = 20;

	/// <summary>
	/// 現在持っている子供の人数
	/// </summary>
	[SerializeField, Header("今持っている子供の人数")]
	private int score = 0;
	public int Score { get { return score; } }

	/// <summary>
	/// 現在の順位
	/// </summary>
	[SerializeField, Header("今の順位")]
	private int rank = 1;
	public int Rank { set { rank = value; } get { return rank; } }

	/// <summary>
	/// 入力を判別する文字列
	/// </summary>
	private string horizontalStr = "Horizontal";
	private string verticalStr = "Vertical";
	private string upStr = "Up";
	private string attackStr = "Attack";

	//音楽データ
	[SerializeField]
	private AudioSource Water_SE;
	[SerializeField]
	private AudioSource Hit_SE;
	[SerializeField]
	private AudioSource ChildGet_SE;
	[SerializeField]
	private AudioSource ScoreGet_SE;
	[SerializeField]
	private AudioSource Colision_SE;


	// Use this for initialization
	void Start () {
	
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
		foreach(Transform child in transform)
			if (child.name == "AttackWater")
				attackWaterObj = child.GetComponent<ParticleSystem>();

		// プレイヤーアイコンオブジェの取得
		numIconObj = this.transform.FindChild("Player_NumIcon");
		numIconPos = numIconObj.localPosition;

		// ボタン文字列に自分のIDを追加
		horizontalStr += this.name;
		verticalStr += this.name;
		upStr += this.name;
		attackStr += this.name;

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
	void Update () {

		switch (eStatus)
		{
			// 通所状態
			case E_STATUS.ACTIVE:
				ActiveUpdate();
				break;

			// 子供を奪われる状態
			case E_STATUS.DAMAGE:
				DamageUpdate();
				break;

			// 子供を奪われた後の状態
			case E_STATUS.CRASH:
				CrashUpdate();
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
/*		if (!enableAttack_f) return;

		if (Input.GetButtonDown(attackStr))
		{
			instansAttackObj = Instantiate(prefabAttackObj);
			instansAttackObj.transform.parent = this.transform;
			instansAttackObj.transform.position = this.transform.position;
			instansAttackObj.GetComponent<AttackObj>().ForwardVec = this.transform.forward;
			enableAttack_f = false;
		
		}
*/

		if (Input.GetButtonDown(attackStr))
			attackWaterObj.Play();

		if (Input.GetButtonUp(attackStr))
			attackWaterObj.Stop();

		if (Input.GetKeyDown(KeyCode.A))
		{
			Water_SE.Play();
			attackWaterObj.Play();
		}

		if (Input.GetKeyUp(KeyCode.A))
		{
			Water_SE.Stop();
			attackWaterObj.Stop();
		}
	}

	/// <summary>
	/// 攻撃を当てた時の処理
	/// テスト用、今は使っていない
	/// </summary>
	public void AttackSuccess()
	{
//		AddScore(1);
	
	}

	/// <summary>
	/// 攻撃を当てられた時の処理
	/// </summary>
	public void AttackDamage()
	{
//		AddScore(-1);

		Hit_SE.Play();
		ChangeStatus(E_STATUS.DAMAGE);
	
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
	/// スコアを加算する
	/// </summary>
	/// <param name="addScore">スコアに加算する値</param>
	/// <param name="pos">衝突した子供オブジェの位置</param>
	/// <returns>true：成功　false：失敗</returns>
	private bool AddScore(int addScore, Vector3 pos)
	{
		if (addScore <= 0) return false;
		if (score + addScore > SCORE_MAX) return false;

		childNumGaugeObj.GainChild(addScore, pos);
		score += addScore;

		return true;

	}

	/// <summary>
	/// スコアを減算する　値は正
	/// </summary>
	/// <param name="decScore">減算する値　値は正</param>
	/// <returns>正誤判定</returns>
	public bool DecreaseScore(int decScore)
	{
		if (decScore <= 0) return false;
		if (score - decScore < 0) return false;

		score -= decScore;

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

	private void ChangeStatus(E_STATUS changeStatus)
	{
		// 終了処理
		switch (eStatus)
		{
			case E_STATUS.ACTIVE:
				ActiveFinalize();
				break;

			case E_STATUS.DAMAGE:
				DamageFinalize();
				break;
		
			case E_STATUS.CRASH:
				CrashFinalize();
				break;


		}

		eStatus = changeStatus;

		// 初期化処理
		switch (eStatus)
		{
			case E_STATUS.ACTIVE:
				ActiveInitialize();
				break;

			case E_STATUS.DAMAGE:
				DamageInitialize();
				break;
		
			case E_STATUS.CRASH:
				CrashInitilize();
				break;
		
		}
	
	
	}

	private void ActiveInitialize()
	{ }

	private void ActiveUpdate()
	{
		// ボタン入力を取る
		// 戻り値は　-1から+1　の値
		axisX = Input.GetAxis(horizontalStr);	// 左右入力
		axisY = Input.GetAxis(verticalStr);	// 前後入力
		axisUpDown = Input.GetAxis(upStr);

		// 攻撃処理
		AttackAction();

		// 移動と回転の計算
		//				transform.Rotate(0.0f, axisX * Time.deltaTime * rotationSpeed, 0.0f, Space.Self);	// ローカル回転
		//				transform.Translate(0.0f, axisUpDown * moveSpeed * Time.deltaTime, axisY * Time.deltaTime * moveSpeed);
		//				transform.Rotate(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z, Space.Self);	// ローカル回転
		//				transform.Translate(0.0f,moveSpeed.y, moveSpeed.z);

		//速度の代入
		rigidbody.velocity = (new Vector3(0, moveSpeed.y, 0) + transform.forward * moveSpeed.z) + HitBackSpeed;
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //回転のＸ、Ｚ軸を0に固定、freezeだとずれる
		rigidbody.angularVelocity = (new Vector3(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z));

	}

	private void ActiveFinalize()
	{ }

	private void DamageInitialize()
	{
		attackWaterObj.Stop();
		remainingDamageTime = damageTimePerSec;
		remainingFlashingTime = flashingOnCrashTimePreSec;

	}

	private void DamageUpdate()
	{
		remainingDamageTime -= Time.deltaTime;
		if (remainingDamageTime < 0)
		{
			ChangeStatus(E_STATUS.ACTIVE);
			return;

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

	}

	private void DamageFinalize()
	{
		rendererObj.materials[0].color = Color.white;
		rendererObj.materials[1].color = Color.white;
	
	}

	private void CrashInitilize()
	{ }

	private void CrashUpdate()
	{
		remainingDamageTime -= Time.deltaTime;

		if (remainingDamageTime < 0)
		{
			ChangeStatus(E_STATUS.ACTIVE);
			return;
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

	}

	private void CrashFinalize()
	{
		//rootObj.transform.rotation = Quaternion.Euler(0, 0, 0);
		rendererObj.materials[0].color = Color.white;
		rendererObj.materials[1].color = Color.white;
	
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
	
    float test_atack_resist = 6f;
    void slipback(Vector3 opponentSpeed)
    {
        moveSpeed = (moveSpeed * -1 * test_atack_resist) - opponentSpeed;

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
            if (AddScore(1, other.transform.position))
            {
//                ChildGet_SE.Play();
                childObjCreatePointObj.DestroyChild(uint.Parse(other.transform.name));

            }

        }

        if (other.tag == "Goal")
        {
            if (score == 0) return;

//			ScoreGet_SE.Play();
			playerData.AddPlayerScoreArray(int.Parse(transform.name) - 1 , score);
			sceneObj.PlayerRankUpdate();
			childNumGaugeObj.ChildrenAdjust();
			score = 0;

        }

        if (other.tag == "Player")
        {

            Colision_SE.Play();
            // 自分が通常状態じゃなければ除外
            if (eStatus != E_STATUS.ACTIVE) return;

            // 所持子供数がマックスなら除外
            if (score == SCORE_MAX) return;

            PlayerControl playerObj = other.GetComponent<PlayerControl>();

            // 相手がダメージ状態じゃなければ除外
            if ((E_STATUS)playerObj.GetStatus() != E_STATUS.DAMAGE) return;
			
			// 相手の所持子供人数を1減らして、自分の所持子供人数を1増やす
			if (playerObj.DecreaseScore(1))
			{
				playerObj.RobbedChildren();
				score++;
			}

        }

    }

    void OnParticleCollision(GameObject obj)
    {
        if (obj.transform.parent == this.transform) return; // 自分の出したパーティクルなら除外
        if (muteki_f) return;                       // 無敵フラグがtureなら除外
        if (eStatus != E_STATUS.ACTIVE) return;     // 状態が通常時以外なら除外

        // 当たった時の処理
        AttackDamage();

    }

}
/*
public class PlayerControl : MonoBehaviour
{

    /// <summary>
    /// プレイヤーの状態を表す用の列挙型
    /// </summary>
    private enum E_STATUS
    {
        ACTIVE, // 通常
        DAMAGE, // 攻撃被ヒット時の子供を奪われる状態
        CRASH,  // 子供を奪われた状態

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
    /// 頭上に表示するプレイヤーアイコンのオブジェ
    /// </summary>
    private Transform numIconObj;

    /// <summary>
    /// プレイヤーアイコンのローカル位置
    /// </summary>
    private Vector3 numIconPos;

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
    private const int SCORE_MAX = 20;

    /// <summary>
    /// 現在持っている子供の人数
    /// </summary>
    [SerializeField, Header("今持っている子供の人数")]
    private int score = 0;
    public int Score { get { return score; } }

    /// <summary>
    /// 現在の順位
    /// </summary>
    [SerializeField, Header("今の順位")]
    private int rank = 1;
    public int Rank { set { rank = value; } get { return rank; } }

    /// <summary>
    /// 入力を判別する文字列
    /// </summary>
    private string horizontalStr = "Horizontal";
    private string verticalStr = "Vertical";
    private string upStr = "Up";
    private string attackStr = "Attack";


    [SerializeField]
    private AudioSource Water_SE;
    [SerializeField]
    private AudioSource Hit_SE;
    [SerializeField]
    private AudioSource ChildGet_SE;
    [SerializeField]
    private AudioSource ScoreGet_SE;
    [SerializeField]
    private AudioSource Colision_SE;
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

        // プレイヤーアイコンオブジェの取得
        numIconObj = this.transform.FindChild("Player_NumIcon");
        numIconPos = numIconObj.localPosition;

        // ボタン文字列に自分のIDを追加
        horizontalStr += this.name;
        verticalStr += this.name;
        upStr += this.name;
        attackStr += this.name;

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
                // ボタン入力を取る
                // 戻り値は　-1から+1　の値
                axisX = Input.GetAxis(horizontalStr);   // 左右入力
                axisY = Input.GetAxis(verticalStr); // 前後入力
                axisUpDown = Input.GetAxis(upStr);

                // 攻撃処理
                AttackAction();

                // 移動と回転の計算
                //				transform.Rotate(0.0f, axisX * Time.deltaTime * rotationSpeed, 0.0f, Space.Self);	// ローカル回転
                //				transform.Translate(0.0f, axisUpDown * moveSpeed * Time.deltaTime, axisY * Time.deltaTime * moveSpeed);
                transform.Rotate(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z, Space.Self);    // ローカル回転
                transform.Translate(0.0f, moveSpeed.y, moveSpeed.z);

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
        /*		if (!enableAttack_f) return;

                if (Input.GetButtonDown(attackStr))
                {
                    instansAttackObj = Instantiate(prefabAttackObj);
                    instansAttackObj.transform.parent = this.transform;
                    instansAttackObj.transform.position = this.transform.position;
                    instansAttackObj.GetComponent<AttackObj>().ForwardVec = this.transform.forward;
                    enableAttack_f = false;

                }
        */
/*
        if (Input.GetButtonDown(attackStr))
            attackWaterObj.Play();

        if (Input.GetButtonUp(attackStr))
            attackWaterObj.Stop();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Water_SE.Play();
            attackWaterObj.Play();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Water_SE.Stop();
            attackWaterObj.Stop();
        }
    }

    /// <summary>
    /// 攻撃を当てた時の処理
    /// テスト用、今は使っていない
    /// </summary>
    public void AttackSuccess()
    {
        AddScore(1);

    }

    /// <summary>
    /// 攻撃を当てられた時の処理
    /// </summary>
    public void AttackDamage()
    {
        //		AddScore(-1);

        Hit_SE.Play();
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

        //回転速度計算 ============== 

        //____減衰計算
        rotationSpeed.y = rotationSpeed.y - rotationSpeed.y * (1.0f - rotationResistivity);
        //____加速計算
        rotationSpeed.y += axisX * Time.deltaTime * rotationAcceleration;
        //____速度制限
        absNum = System.Math.Abs(rotationSpeed.y);
        if (rotationMaxspeed < absNum)
            rotationSpeed.y = rotationMaxspeed * (rotationSpeed.y / absNum);

        //移動速度計算 ============== 

        //____減衰計算
        moveSpeed.z = moveSpeed.z - moveSpeed.z * (1.0f - moveResistivity);
        //____加速計算
        moveSpeed.z += axisY * Time.deltaTime * moveAcceleration;
        //____速度制限
        absNum = System.Math.Abs(moveSpeed.z);
        if (moveMaxspeed < absNum)
            moveSpeed.z = moveMaxspeed * (moveSpeed.z / absNum);




        //浮遊計算 ==============       

        //____減衰計算
        moveSpeed.y = moveSpeed.y - moveSpeed.y * (1.0f - floatingResistivity * Time.deltaTime);

        //キー入力があれば浮遊を停止する
        if (axisUpDown != 0)
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

        return;
    }

    float test_atack_resist = 6f;
    void slipback(Vector3 opponentSpeed)
    {
        moveSpeed = (moveSpeed * -1 * test_atack_resist) - opponentSpeed;

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
    void OnCollisionEnter(Collision aite)
    {

    }

		if (other.tag == "Child")
		{
			if (AddScore(1, other.transform.position))
			{
				ChildGet_SE.Play();
				childObjCreatePointObj.DestroyChild(uint.Parse(other.transform.name));

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Child")
        {
            if (AddScore(1))
            {
                ChildGet_SE.Play();
                childObjCreatePointObj.DestroyChild(uint.Parse(other.transform.name));

            }

        }

			ScoreGet_SE.Play();
			playerData.AddPlayerScoreArray(int.Parse(transform.name) - 1 , score);
			sceneObj.PlayerRankUpdate();
			childNumGaugeObj.ChildrenAdjust();
			score = 0;
		
		}

		if (other.tag == "Player")
		{

			Colision_SE.Play();

			// 自分が通常状態じゃなければ除外
			if (eStatus != E_STATUS.ACTIVE) return;
			
			// 所持子供数がマックスなら除外
			if (score == SCORE_MAX) return;

        if (other.tag == "Goal")
        {
            if (score == 0) return;

            ScoreGet_SE.Play();
            playerData.AddPlayerScoreArray(int.Parse(transform.name) - 1, score);
            sceneObj.PlayerRankUpdate();
            score = 0;

        }

        if (other.tag == "Player")
        {

            Colision_SE.Play();
            // 自分が通常状態じゃなければ除外
            if (eStatus != E_STATUS.ACTIVE) return;

            // 所持子供数がマックスなら除外
            if (score == SCORE_MAX) return;

			// 相手の所持子供人数を1減らして、自分の所持子供人数を1増やす
			if (playerObj.DecreaseScore(1))
			{
				playerObj.RobbedChildren();
				score++;

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
        if (obj.transform.parent == this.transform) return; // 自分の出したパーティクルなら除外
        if (muteki_f) return;                       // 無敵フラグがtureなら除外
        if (eStatus != E_STATUS.ACTIVE) return;     // 状態が通常時以外なら除外

        // 当たった時の処理
        AttackDamage();

    }

    void OnWillRenderObject()
    {
        /*		if (this.transform.position.y < Camera.current.transform.position.y) return;

                if (Mathf.Asin((this.transform.position.y - Camera.current.transform.position.y) / Vector3.Distance(Camera.current.transform.position, this.transform.position)) * 180 / Mathf.PI < 15.0f)
                    numIconObj.localPosition = numIconPos;
                else
                    numIconObj.localPosition = -numIconPos;
        */
/*
    }

}
*/