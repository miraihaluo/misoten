using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChildNumGauge : MonoBehaviour {

	/// <summary>
	/// カメの頭の状態
	/// </summary>
	private enum E_HEAD_STATUS
	{
		CLOSE,
		OPEN,
		MOGUMOGU,
		ADJUST,

		MAX
	};

	/// <summary>
	/// 状態
	/// </summary>
	private E_HEAD_STATUS eStatus = E_HEAD_STATUS.CLOSE;

	/// <summary>
	/// モグモグの状態
	/// </summary>
	private E_HEAD_STATUS eHeadStatus = E_HEAD_STATUS.CLOSE;

	/// <summary>
	/// アニメーション用のレクト
	/// </summary>
	private Rect UVRect;

	/// <summary>
	/// 子供アイコンのプレハブ
	/// </summary>
	private GameObject prefab_ChildrenIconObj;

	/// <summary>
	/// インスタンスを保存する一時オブジェ
	/// </summary>
	private GameObject instanseObj;

	/// <summary>
	/// カメの頭のオブジェクト
	/// </summary>
	private RawImage headObj_RawImage;

	/// <summary>
	/// 子供アイコンの親になるGameObjectのオブジェ
	/// </summary>
	private GameObject childrenObj;

	/// <summary>
	/// 所属するカメラオブジェ
	/// </summary>
	[SerializeField]
	private Camera camera;

	/// <summary>
	/// カメの頭が動くアニメーションの間隔
	/// </summary>
	[SerializeField]
	private float headAnimationTime = 0.25f;

	/// <summary>
	/// カメの頭が動くアニメーション用
	/// </summary>
	private float headAnimationNowTime;

	/// <summary>
	/// カメの頭が動くフラグのスタック
	/// 0で停止　1以上で稼働
	/// </summary>
	private int headOperateFlagStack = 0;

	/// <summary>
	/// モグモグする回数
	/// </summary>
	[SerializeField, Header("モグモグする回数")]
	private int headAnimationCount = 5;

	private int headAnimationNowCount;

	/// <summary>
	/// 子供を解放する間隔
	/// </summary>
	[SerializeField, Header("子供を解放する間隔")]
	private float childrenAdjustInterval = 0.2f;

	private float childrenAdjustNowTime;

	/// <summary>
	/// 解法していく子要素の番号
	/// </summary>
	private int adjustChildrenCount;

	// Use this for initialization
	void Start () {
		headAnimationNowTime = headAnimationTime;
		//GainChild(1);
		CloseInitialize();
	
	}

	void Awake()
	{
		prefab_ChildrenIconObj = (GameObject)Resources.Load("Prefabs/GameMain/UIs/ChildGauge_ChildIcon");

		foreach (Transform child in this.transform)
		{
			if (child.name == "Head")
				headObj_RawImage = child.GetComponent<RawImage>();

			if (child.name == "Children")
				childrenObj = child.gameObject;
		
		}

		UVRect = headObj_RawImage.uvRect;
	
	}
	
	// Update is called once per frame
	void Update () {

		switch (eStatus)
		{
			case E_HEAD_STATUS.CLOSE:
				CloseUpdate();
				break;

			case E_HEAD_STATUS.OPEN:
				OpenUpdate();
				break;

			case E_HEAD_STATUS.MOGUMOGU:
				MogumoguUpdate();
				break;

			case E_HEAD_STATUS.ADJUST:
				AdjustUpdate();
				break;
		
		}

	}

	/// <summary>
	/// 子供を獲得した時の処理
	/// </summary>
	/// <param name="num">獲得した子供の人数</param>
	public void GainChild(int num, Vector3 pos)
	{
		for (int i = 0; i < num; i++)
		{
			instanseObj = Instantiate(prefab_ChildrenIconObj);
			instanseObj.transform.parent = childrenObj.transform;
		//	instanseObj.transform.localPosition = Vector3.zero;
			instanseObj.transform.localPosition = RectTransformUtility.WorldToScreenPoint(camera, pos);
			instanseObj.SendMessage("SetGaugeObj", this);

		}

		headOperateFlagStack += num;

	}

	/// <summary>
	/// 子供アイコンの動作が完了した時
	/// カメの頭の動作フラグを減らす
	/// </summary>
	public void ConpletionHeadOperation()
	{
		headOperateFlagStack--;
	
	}

	public void ChildrenAdjust()
	{
		ChangeStatus(E_HEAD_STATUS.ADJUST);
	
	}

	private void ChangeStatus(E_HEAD_STATUS changeStatus)
	{
		switch (eStatus)
		{
			case E_HEAD_STATUS.CLOSE:
				CloseFinalize();
				break;

			case E_HEAD_STATUS.OPEN:
				OpenFinalize();
				break;

			case E_HEAD_STATUS.MOGUMOGU:
				MogumoguFinalize();
				break;
		
			case E_HEAD_STATUS.ADJUST:
				AdjustFinalize();
				break;

		}

		eStatus = changeStatus;
		switch (eStatus)
		{
			case E_HEAD_STATUS.CLOSE:
				CloseInitialize();
				break;

			case E_HEAD_STATUS.OPEN:
				OpenInitialize();
				break;

			case E_HEAD_STATUS.MOGUMOGU:
				MogumoguInitialize();
				break;

			case E_HEAD_STATUS.ADJUST:
				AdjustInitialize();
				break;

		}
	
	}

	private void CloseInitialize()
	{
		UVRect.y = 0 * UVRect.height;
		headObj_RawImage.uvRect = UVRect;
	
	}

	private void CloseUpdate()
	{
		if (headOperateFlagStack > 0)
			ChangeStatus(E_HEAD_STATUS.OPEN);
	
	}

	private void CloseFinalize()
	{ }

	private void OpenInitialize()
	{
		UVRect.y = 1 * UVRect.height;
		headObj_RawImage.uvRect = UVRect;
	
	}

	private void OpenUpdate()
	{
		if (headOperateFlagStack <= 0)
			ChangeStatus(E_HEAD_STATUS.MOGUMOGU);
	
	}

	private void OpenFinalize()
	{ }

	private void MogumoguInitialize()
	{
		headAnimationNowCount = headAnimationCount;
		headAnimationNowTime = headAnimationTime;
		eHeadStatus = E_HEAD_STATUS.CLOSE;
		UVRect.y = (int)eHeadStatus * UVRect.height;
		headObj_RawImage.uvRect = UVRect;
	
	}

	private void MogumoguUpdate()
	{
		headAnimationNowTime -= Time.deltaTime;

		if (headAnimationNowTime <= 0)
		{
			headAnimationNowTime = headAnimationTime;
			eHeadStatus = (E_HEAD_STATUS)(((int)eHeadStatus + 1) % ((int)E_HEAD_STATUS.OPEN + 1));
			UVRect.y = (int)eHeadStatus * UVRect.height;
			headObj_RawImage.uvRect = UVRect;
			headAnimationNowCount--;
		}

		if (headAnimationNowCount == 0)
			ChangeStatus(E_HEAD_STATUS.CLOSE);
	
	}

	private void MogumoguFinalize()
	{ }

	private void AdjustInitialize()
	{
		childrenAdjustNowTime = childrenAdjustInterval;
		adjustChildrenCount = 0;

		UVRect.y = 1 * UVRect.height;
		headObj_RawImage.uvRect = UVRect;
	
	}

	private void AdjustUpdate()
	{
		childrenAdjustNowTime -= Time.deltaTime;

		if (childrenAdjustNowTime <= 0)
		{
			childrenObj.transform.GetChild(adjustChildrenCount).SendMessage("ChangeModeOUT"); ;
			childrenAdjustNowTime = childrenAdjustInterval;
			adjustChildrenCount++;

		}

		if (adjustChildrenCount >= childrenObj.transform.childCount)
		{
			ChangeStatus(E_HEAD_STATUS.CLOSE);

		}

	}

	private void AdjustFinalize()
	{ }

}
