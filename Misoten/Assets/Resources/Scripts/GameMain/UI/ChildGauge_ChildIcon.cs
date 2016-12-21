using UnityEngine;
using System.Collections;

public class ChildGauge_ChildIcon : MonoBehaviour {

	/// <summary>
	/// 子供アイコンの状態
	/// </summary>
	public enum E_CHILD_ICON_STATUS
	{
		NONE,
		CREATE,
		MOVE,
        OUT,
		ROB,
		MAX
	}

	private E_CHILD_ICON_STATUS eStatus = E_CHILD_ICON_STATUS.NONE;

	/// <summary>
	/// 自分のRectTransformオブジェ
	/// </summary>
	private RectTransform rectTransObj;

	/// <summary>
	/// 自分の所属するゲージオブジェ
	/// </summary>
	private ChildNumGauge gaugeObj = null;

	/// <summary>
	/// 子供ゲージへのオフセット
	/// </summary>
	private Vector3 mainPosOffset;

	/// <summary>
	/// 移動限界範囲（ローカル座標）
	/// </summary>
	[SerializeField, Header("移動範囲")]
	private Vector3 limitPos = new Vector3(200, 100, 0.0f);

	/// <summary>
	/// 移動範囲のオフセット
	/// </summary>
	[SerializeField, Header("移動範囲のオフセット")]
	private Vector3 limitPosOffset = new Vector3(-50, -10, 0.0f);

	private Vector3 afterLimitPos = Vector3.zero;

	/// <summary>
	/// 移動方向
	/// </summary>
	private Vector3 moveVec;

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

    //ベジェ曲線
    private Bezier inBezier, outBezier, robBezier;
    private float t = 0.0f;

    void Awake()
	{
		rectTransObj =  this.GetComponent<RectTransform>();
	
	}

	// Use this for initialization
	void Start () {
		moveNowTime = RandomVal(moveTimeArea[0], moveTimeArea[1]);
		mainPosOffset.x = -this.transform.parent.transform.localPosition.x;
		mainPosOffset.y = -this.transform.parent.transform.localPosition.y;
		limitPosOffset += mainPosOffset;
		inBezier = new Bezier(this.transform.localPosition, Vector3.zero + mainPosOffset, new Vector3(400f, 0.0f, 0f) + mainPosOffset, new Vector3(limitPosOffset.x, mainPosOffset.y, 0f));
		outBezier = new Bezier(Vector3.up * -20.0f + mainPosOffset, new Vector3(300f, 0f, 0f), Vector3.zero + mainPosOffset, new Vector3(Random.Range(-100, 100) + mainPosOffset.x, 400f + mainPosOffset.y, 0f));
		robBezier = new Bezier(new Vector3(limitPosOffset.x, mainPosOffset.y, 0f), new Vector3(400f, 0.0f, 0f) + mainPosOffset, Vector3.zero + mainPosOffset, Vector3.up * 150.0f );

		//CreateInitialize();
        //inBezier = new Bezier(new Vector3(0f, 200.0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(400f, 0.0f, 0f), new Vector3(limitPosOffset.x, -20.0f, 0f));
        //outBezier = new Bezier(new Vector3(0f, 0f, 0f), new Vector3(300f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(Random.Range(-50, 50), 400f, 0f));
    }

    // Update is called once per frame
    void Update () {

		switch (eStatus)
		{
			case E_CHILD_ICON_STATUS.CREATE:
				CreateUpdate();
				break;

			case E_CHILD_ICON_STATUS.MOVE:
				MoveUpdate();
				break;

            case E_CHILD_ICON_STATUS.OUT:
                OutUpdate();
                break;

			case E_CHILD_ICON_STATUS.ROB:
				Robupdate();
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

	private void SetGaugeObj(ChildNumGauge obj)
	{
		gaugeObj = obj;

	}

	public void ChangeModeCreate()
	{
		ChangeChildIconStatus(E_CHILD_ICON_STATUS.CREATE);
	
	}

	/// <summary>
	/// 状態をOUTにする
	/// </summary>
	public void ChangeModeOUT()
	{
		ChangeChildIconStatus(E_CHILD_ICON_STATUS.OUT);
	
	}

	public void ChangeModeRob()
	{
		ChangeChildIconStatus(E_CHILD_ICON_STATUS.ROB);
	
	}

	private void ChangeChildIconStatus(E_CHILD_ICON_STATUS changeStatus)
	{
		// 各状態の終了処理
		switch (eStatus)
		{
			case E_CHILD_ICON_STATUS.CREATE:
				CreateFinalize();
				break;

			case E_CHILD_ICON_STATUS.MOVE:
				MoveFinalize();
				break;

			case E_CHILD_ICON_STATUS.OUT:
				OutFinalize();
				break;

			case E_CHILD_ICON_STATUS.ROB:
				RobFinalize();
				break;
		
		}

		// 初期化処理
		eStatus = changeStatus;
		switch (eStatus)
		{
			case E_CHILD_ICON_STATUS.CREATE:
				CreateInitialize();
				break;

			case E_CHILD_ICON_STATUS.MOVE:
				MoveInitialize();
				break;

			case E_CHILD_ICON_STATUS.OUT:
				OutInitialize();
				break;

			case E_CHILD_ICON_STATUS.ROB:
				RobInitialize();
				break;

		}
	
	}

	private void CreateInitialize()
	{
		this.transform.localPosition = Vector3.up * 200.0f;
		
	}

	private void CreateUpdate()
	{
        this.transform.localPosition = inBezier.GetPointAtTime(t);
        t += Time.deltaTime * 0.5f;
        if (t > 1f)
        {
            t = 0f;
            ChangeChildIconStatus(E_CHILD_ICON_STATUS.MOVE);
        }
        /*/   
        this.transform.localPosition -= Vector3.up * (25.0f * Time.deltaTime);
        if (this.transform.localPosition.y < 0)
			ChangeStatus(E_STATUS.MOVE);
	    //*/
	}

	private void CreateFinalize()
	{
		//this.transform.localPosition = Vector3.zero;
		gaugeObj.ConpletionHeadOperation();
	
	}

	private void MoveInitialize()
	{
		moveVec.x = Random.Range(-1.0f, 1.0f);
		moveVec.y = Random.Range(-1.0f, 1.0f);
		moveVec.z = 0.0f;

		moveVec.Normalize();

	}

	private void MoveUpdate()
	{
		/*
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
		*/

		this.transform.localPosition += moveVec * moveVal * Time.deltaTime;

		afterLimitPos.x = Mathf.Clamp(this.transform.localPosition.x, -limitPos.x + limitPosOffset.x, limitPos.x + limitPosOffset.x);
		afterLimitPos.y = Mathf.Clamp(this.transform.localPosition.y, -limitPos.y + limitPosOffset.y, limitPos.y + limitPosOffset.y);

		this.transform.localPosition = afterLimitPos;

		if (this.transform.localPosition.x <= -limitPos.x + limitPosOffset.x) moveVec.x *= -1;
		if (this.transform.localPosition.x >= limitPos.x + limitPosOffset.x) moveVec.x *= -1;
		if (this.transform.localPosition.y <= -limitPos.y + limitPosOffset.y) moveVec.y *= -1;
		if (this.transform.localPosition.y >= limitPos.y + limitPosOffset.y) moveVec.y *= -1;

	}

	private void MoveFinalize()
	{
//		this.gameObject.SetActive(false);

	}

	private void OutInitialize()
	{
		//outBezier = new Bezier(this.transform.localPosition, new Vector3(300f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(Random.Range(-50, 50), 400f, 0f));
		
	}

    private void OutUpdate()
    {
		this.transform.localPosition = outBezier.GetPointAtTime(t);
        t += Time.deltaTime * 0.5f;
        if (t > 1f)
        {
            t = 0f;
			ChangeChildIconStatus(E_CHILD_ICON_STATUS.NONE);
        }

    }

	private void OutFinalize()
	{
		this.gameObject.SetActive(false);
	
	}

	private void RobInitialize()
	{ }

	private void Robupdate()
	{
		this.transform.localPosition = robBezier.GetPointAtTime(t);
		t += Time.deltaTime * 0.5f;
		if (t > 1f)
		{
			t = 0f;
			ChangeChildIconStatus(E_CHILD_ICON_STATUS.NONE);
		}

	}

	private void RobFinalize()
	{
		this.gameObject.SetActive(false);
	
	}

	public bool GetGaugeIconActiveFlag()
	{
		if (eStatus == E_CHILD_ICON_STATUS.OUT)
			return true;
		else
			return false;

	}

	public bool GetGaugeIconRobFlag()
	{
		if (eStatus == E_CHILD_ICON_STATUS.ROB)
			return true;
		else
			return false;

	}

}
