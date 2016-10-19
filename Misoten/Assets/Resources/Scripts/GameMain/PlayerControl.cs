using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	// 秒間の移動量
	// インスペクターで設定する
	[SerializeField]
	private float moveSpeed;

	// 秒間の回転量
	// インスペクターで設定する
	[SerializeField]
	private float rotationSpeed;

	private DefineData defineData;

	// 移動限界値の最小最大値　正四角形が大前提
	private float max;
	private float min;

	// 移動限界位置一時格納用ベクトル
	private Vector3 limitPos = Vector3.one;

	// シーンを跨ぐプレイヤーデータを扱うアセットデータ
	private PlayerData playerData;
	
	// 一度に持てる子供の最大人数
	private const int SCORE_MAX = 3;

	//現在のスコア
	[SerializeField]
	private int score = 0;
	public int Score { get { return score; } }

	// 現在の順位
	[SerializeField]
	private int rank = 1;
	public int Rank { set { rank = value; } get { return rank; } }

	private string horizontalStr = "Horizontal";
	private string verticalStr = "Vertical";

	// Use this for initialization
	void Start () {
	
	}
	void Awake()
	{
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");
		defineData = Resources.Load<DefineData>("Assets/DefineData");

		horizontalStr += this.name;
		verticalStr += this.name;

		max = defineData.STAGE_BOARDER_SIZE / 2.0f;
		min = -(defineData.STAGE_BOARDER_SIZE / 2.0f);
	
	}
	
	// Update is called once per frame
	void Update () {

		// ボタン入力を取る
		// 戻り値は　-1から+1　の値
		float rotY = Input.GetAxis(horizontalStr);	// 左右入力
		float moveZ = Input.GetAxis(verticalStr);	// 前後入力

		// 秒間の値をフレーム間の値に変換する
		float deltaMoveSpeed = Time.deltaTime * moveSpeed;
		float deltaRotationSpeed = Time.deltaTime * rotationSpeed;

		// 移動と回転の計算
		transform.Translate(0.0f, 0.0f, moveZ * deltaMoveSpeed);
		transform.Rotate(0.0f, rotY * deltaRotationSpeed, 0.0f, Space.Self);	// ローカル回転

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


	void OnCollisionEnter(Collision aite)
	{

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "Child")
		{
			if (score < SCORE_MAX)
			{
				Destroy(other.gameObject);
				score++;

			}

		}

		if (other.tag == "Goal")
		{
			playerData.AddPlayerScoreArray(int.Parse(transform.name) - 1 , score);
			score = 0;
		
		}

	}

}
