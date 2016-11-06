using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private enum E_STATUS
	{
		ACTIVE,
		DAMAGE
	
	}

	/// <summary>
	/// 状態
	/// </summary>
	private E_STATUS eStatus = E_STATUS.ACTIVE;

	/// <summary>
	/// 秒間の移動量。
	/// インスペクターで設定する
	/// </summary>
	[SerializeField]
	private float moveSpeed;

	/// <summary>
	/// 秒間の回転量。
	/// インスペクターで設定する
	/// </summary>
	[SerializeField]
	private float rotationSpeed;

	/// <summary>
	/// 攻撃被ヒット時の硬直フレーム
	/// </summary>
	//[SerializeField]
	private float freezeTimePerFrame = 60.0f;

	/// <summary>
	/// 攻撃被ヒット時の回転モーションの速さ
	/// </summary>
	//[SerializeField]
	private float damageRotationSpeed = 5.0f;

	/// <summary>
	/// 硬直時間の残り時間（フレーム）
	/// </summary>
	private float remainingFreezeTime;

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
	private GameObject prefabAttackObj;

	/// <summary>
	/// 生成した攻撃オブジェクトの一時取得用
	/// </summary>
	private GameObject instansAttackObj;

	/// <summary>
	/// 攻撃可能かどうかのフラグ
	/// </summary>
	private bool enableAttack_f = true;

	/// <summary>
	/// rootの参照
	/// </summary>
	private GameObject rootObj;

	/// <summary>
	/// 自身のリジッドボディ
	/// </summary>
	private Rigidbody rigidbody;

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
	private const int SCORE_MAX = 3;

	/// <summary>
	/// 現在のスコア
	/// </summary>
	[SerializeField]
	private int score = 0;
	public int Score { get { return score; } }

	/// <summary>
	/// 現在の順位
	/// </summary>
	[SerializeField]
	private int rank = 1;
	public int Rank { set { rank = value; } get { return rank; } }

	/// <summary>
	/// 入力を判別する文字列
	/// </summary>
	private string horizontalStr = "Horizontal";
	private string verticalStr = "Vertical";
	private string upStr = "Up";
	private string attackStr = "Attack";

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
		prefabAttackObj = (GameObject)Resources.Load("Prefabs/GameMain/AttackObj");

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

	}
	
	// Update is called once per frame
	void Update () {

		switch (eStatus)
		{
			case E_STATUS.ACTIVE:
				// ボタン入力を取る
				// 戻り値は　-1から+1　の値
				axisX = Input.GetAxis(horizontalStr);	// 左右入力
				axisY = Input.GetAxis(verticalStr);	// 前後入力
				axisUpDown = Input.GetAxis(upStr);

				// 攻撃処理
				AttackAction();

				break;

			case E_STATUS.DAMAGE:
				remainingFreezeTime -= Time.deltaTime * 60.0f;

				rootObj.transform.Rotate(0.0f, Time.deltaTime * damageRotationSpeed * 60.0f, 0.0f, Space.Self);

				if (remainingFreezeTime < 0)
					eStatus = E_STATUS.ACTIVE;

				break;
		
		}

		if (Input.GetKeyDown(KeyCode.Return))
			AttackDamage();

		// 移動と回転の計算
		transform.Rotate(0.0f, axisX * Time.deltaTime * rotationSpeed, 0.0f, Space.Self);	// ローカル回転
		 transform.Translate(0.0f, axisUpDown * moveSpeed * Time.deltaTime, axisY * Time.deltaTime * moveSpeed);
		//rigidbody.AddForce(this.transform.forward.x * axisY * Time.deltaTime * moveSpeed, axisUpDown * moveSpeed * Time.deltaTime, this.transform.forward.z * axisY * Time.deltaTime * moveSpeed);

		// 減速処理
		//rigidbody.velocity = moveSpeed;

		// 移動範囲のチェック
		CheckLimitPos();

	}

	private void CheckLimitPos()
	{
		// 各成分に、限界値内に収めた値を代入する
		limitPos.x = Mathf.Clamp(transform.position.x, min, max);
		limitPos.y = transform.position.y;	// Y成分は制限無し
		limitPos.z = Mathf.Clamp(transform.position.z, min, max);

		// 現在位置ベクトルに、矯正した位置ベクトルを代入
		transform.position = limitPos;

	}

	/// <summary>
	/// 攻撃関係の処理
	/// </summary>
	private void AttackAction()
	{
		if (!enableAttack_f) return;

		if (Input.GetButtonDown(attackStr))
		{
			instansAttackObj = Instantiate(prefabAttackObj);
			instansAttackObj.transform.parent = this.transform;
			instansAttackObj.transform.position = this.transform.position;
			instansAttackObj.GetComponent<AttackObj>().ForwardVec = this.transform.forward;
			enableAttack_f = false;
		
		}
	
	}

	/// <summary>
	/// 攻撃を当てた時の処理
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
		AddScore(-1);

		remainingFreezeTime = freezeTimePerFrame;
		eStatus = E_STATUS.DAMAGE;
	
	}

	public void DestroyAttackObj()
	{
		Destroy(GetComponentInChildren<AttackObj>().gameObject);
		enableAttack_f = true;
	
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

	void OnCollisionEnter(Collision aite)
	{

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
			playerData.AddPlayerScoreArray(int.Parse(transform.name) - 1 , score);
			sceneObj.PlayerRankUpdate();
			score = 0;
		
		}

	}

}
