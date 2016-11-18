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
	/// 秒間の移動の加速量。。
	/// インスペクターで設定する
	/// </summary>
	[SerializeField]
    private float moveAcceleration;

	/// <summary>
	/// 秒間の回転の加速量。
	/// インスペクターで設定する
	/// </summary>
	[SerializeField]
    private float rotationAcceleration;

    /// <summary>
    /// 現在の移動量
    /// </summary>
 //   [SerializeField]
    private Vector3 moveSpeed;

    /// <summary>
    /// 現在の回転量
    /// </summary>
 //   [SerializeField]
    private Vector3 rotationSpeed;

    /// <summary>
    /// 移動の減速の割合
    /// </summary>
    [SerializeField]
    private float moveResistivity;

    /// <summary>
    /// 回転の減速の割合
    /// </summary>
    [SerializeField]
    private float rotationResistivity;

    /// <summary>
    /// 移動の最高速度
    /// </summary>
    [SerializeField]
    private float moveMaxspeed;

    /// <summary>
    /// 回転の最高速度
    /// </summary>
    [SerializeField]
    private float rotationMaxspeed;

    /// <summary>
    /// 浮遊の速度
//    /// </summary>
  //  [SerializeField]
    private float floating;

    /// <summary>
    ///浮遊の加速度
    /// </summary>
    [SerializeField]
    private float floatingAcceleration;

    /// <summary>
    /// 上下移動の加速度
    /// </summary>
    [SerializeField]
    private float floatingMoveAcceleration;

    /// <summary>
    /// 上下移動のの減衰値
    /// </summary>
    [SerializeField]
    private float floatingResistivity;


    /// <summary>
    /// 上下移動の最高速度
    /// </summary>
    [SerializeField]
    private float floatingMaxspeed;


    /// <summary>
    /// 上下移動の減衰値
    /// </summary>
    [SerializeField]
    private float floatingRevision;

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


        //計算用関数
        speedCalculation();

		// 移動と回転の値の代入
      //  transform.Rotate(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z, Space.Self);	// ローカル回転
	//	transform.Translate(0.0f,moveSpeed.y, moveSpeed.z);
        rigidbody.AddTorque(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z,ForceMode.Force);
        rigidbody.AddForce(0.0f, moveSpeed.y, moveSpeed.z, ForceMode.Force);

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
            moveSpeed.z = moveMaxspeed * (moveSpeed.z / absNum ); 




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

}
