using UnityEngine;
using System.Collections;

public class ChildGauge_ChildIcon : MonoBehaviour {

	/// <summary>
	/// 子供アイコンの状態
	/// </summary>
	private enum E_STATUS
	{
		CREATE,
		MOVE,

		MAX
	}

	private E_STATUS eStatus = E_STATUS.CREATE;

	/// <summary>
	/// 自分のRectTransformオブジェ
	/// </summary>
	private RectTransform rectTransObj;

	/// <summary>
	/// 移動限界範囲（ローカル座標）
	/// </summary>
	[SerializeField, Header("移動範囲")]
	private Vector2 limitPos = new Vector2(200, 100);

	private Vector3 afterLimitPos = Vector3.zero;

	/// <summary>
	/// 乱数を格納する一時変数
	/// </summary>
	private float randomVal;

	/// <summary>
	/// 移動量
	/// </summary>
	[SerializeField, Header("移動量")]
	private float moveVal = 5.0f;

	/// <summary>
	/// 一回の回転の範囲
	/// </summary>
	[SerializeField, Header("回転の範囲")]
	private float rotateValArea = 180.0f;

	/// <summary>
	/// 回転量
	/// </summary>
	private float rotateVal;

	/// <summary>
	/// 移動時間
	/// </summary>
	[SerializeField, Header("一回の移動時間の振れ幅")]
	private float[] moveTimeArea = new float[2] {0.5f, 1.0f};

	/// <summary>
	/// 今の移動時間
	/// </summary>
	private float moveNowTime;

	private const int randomPer = 5;

	void Awake()
	{
		rectTransObj =  this.GetComponent<RectTransform>();
	
	}

	// Use this for initialization
	void Start () {
		moveNowTime = RandomVal(moveTimeArea[0], moveTimeArea[1]);
		CreateInitialize();

	}
	
	// Update is called once per frame
	void Update () {

		switch (eStatus)
		{
			case E_STATUS.CREATE:
				CreateUpdate();
				break;

			case E_STATUS.MOVE:
				MoveUpdate();
				break;
		
		
		}

	}

	private float RandomVal(float min, float max)
	{
		randomVal = 0;
		for (int i = 0; i < randomPer; i++)
			randomVal += Random.Range(min, max);

		return randomVal / randomPer;

	}

	private void ChangeStatus(E_STATUS changeStatus)
	{
		// 各状態の終了処理
		switch (eStatus)
		{
			case E_STATUS.CREATE:
				CreateFinalize();
				break;

			case E_STATUS.MOVE:
				MoveFinalize();
				break;
		
		}

		// 初期化処理
		eStatus = changeStatus;
		switch (eStatus)
		{
			case E_STATUS.CREATE:
				CreateInitialize();
				break;

			case E_STATUS.MOVE:
				MoveInitialize();
				break;

		}
	
	}

	private void CreateInitialize()
	{
		this.transform.localPosition = Vector3.up * 200.0f;

	}

	private void CreateUpdate()
	{
		this.transform.localPosition -= Vector3.up * (25.0f * Time.deltaTime);

		if(this.transform.localPosition.y < 0)
			ChangeStatus(E_STATUS.MOVE);
	
	}

	private void CreateFinalize()
	{
		this.transform.localPosition = Vector3.zero;
	
	}

	private void MoveInitialize()
	{ }

	private void MoveUpdate()
	{
		moveNowTime -= Time.deltaTime;

		if (moveNowTime <= 0)
		{
			rotateVal = RandomVal(-rotateValArea, rotateValArea);
			this.transform.Rotate(0.0f, 0.0f, rotateVal * Time.deltaTime);
			this.transform.Translate(this.transform.up.x * (moveVal * Time.deltaTime), this.transform.up.y * (moveVal * Time.deltaTime), 0.0f);

			afterLimitPos.x = Mathf.Clamp(this.transform.localPosition.x, -limitPos.x, limitPos.x);
			afterLimitPos.y = Mathf.Clamp(this.transform.localPosition.y, -limitPos.y, limitPos.y);

			this.transform.localPosition = afterLimitPos;

		}

	}

	private void MoveFinalize()
	{ }

}
