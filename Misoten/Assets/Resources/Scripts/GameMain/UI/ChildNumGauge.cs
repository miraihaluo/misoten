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
		ROB,

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

	private PlayerData playerData;

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
		
		// アセットの参照受け取り
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");

		foreach (Transform child in this.transform)
		{
			if (child.name == "Head")
				headObj_RawImage = child.GetComponent<RawImage>();

			if (child.name == "Children")
				childrenObj = child.gameObject;
		
		}

		// 子供アイコンをあらかじめ生成しておく
		for (int i = 0; i < playerData.MAX_SCORE_CHILDREN; i++)
		{
			instanseObj = Instantiate(prefab_ChildrenIconObj);
			instanseObj.transform.parent = childrenObj.transform;
			instanseObj.transform.localPosition = Vector3.zero;
			instanseObj.transform.localScale = Vector3.one;
			instanseObj.SendMessage("SetGaugeObj", this);
			instanseObj.gameObject.SetActive(false);

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
		
			case E_HEAD_STATUS.ROB:
				RobUpdate();
				break;

		}

	}

	/// <summary>
	/// 子供を獲得した時の処理
	/// </summary>
	/// <param name="num">獲得した子供の人数</param>
	public void GainChild(int num, Vector3 pos)
	{
/*		for (int i = 0; i < num; i++)
		{
			instanseObj = Instantiate(prefab_ChildrenIconObj);
			instanseObj.transform.parent = childrenObj.transform;
			instanseObj.transform.localPosition = Vector3.up * 150;
		//	instanseObj.transform.position = RectTransformUtility.WorldToScreenPoint(camera, pos);
		//	instanseObj.transform.localPosition = instanseObj.transform.position;
			instanseObj.SendMessage("SetGaugeObj", this);

		}
*/

		for (int i = 0; i < num; i++)
		{
			foreach (Transform obj in childrenObj.transform)
			{
				if (!obj.gameObject.activeSelf)
				{
					obj.gameObject.SetActive(true);
					obj.GetComponent<ChildGauge_ChildIcon>().ChangeModeCreate();
					break;

				}
			
			}

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

			case E_HEAD_STATUS.ROB:
				RobFinalize();
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

			case E_HEAD_STATUS.ROB:
				RobInitialize();
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

	private int activeChildrenCount;
	private void AdjustInitialize()
	{
		childrenAdjustNowTime = childrenAdjustInterval;
		adjustChildrenCount = 0;

		UVRect.y = 1 * UVRect.height;
		headObj_RawImage.uvRect = UVRect;

		activeChildrenCount = 0;
		foreach (Transform obj in childrenObj.transform)
		{
			if (obj.gameObject.activeSelf) activeChildrenCount++;
		
		}
	
	}

	private void AdjustUpdate()
	{
		childrenAdjustNowTime -= Time.deltaTime;

		if (childrenAdjustNowTime <= 0)
		{
		//	childrenObj.transform.GetChild(adjustChildrenCount).SendMessage("ChangeModeOUT");
			foreach(Transform obj in childrenObj.transform)
			{
				if(!obj.gameObject.activeSelf) continue;
				if (obj.GetComponent<ChildGauge_ChildIcon>().GetGaugeIconActiveFlag()) continue;

				obj.SendMessage("ChangeModeOUT");
				break;

			}
			childrenAdjustNowTime = childrenAdjustInterval;
			adjustChildrenCount++;

		}

		if (adjustChildrenCount >= activeChildrenCount)
		{
			ChangeStatus(E_HEAD_STATUS.CLOSE);

		}

	}

	private void AdjustFinalize()
	{ }

	public void ChangeRobStatus(int dec)
	{
		decChildren = dec;
		ChangeStatus(E_HEAD_STATUS.ROB);
	
	}

	private int decChildren;
	private void RobInitialize()
	{
		childrenAdjustNowTime = 0.5f; ;
		adjustChildrenCount = decChildren;

		UVRect.y = 1 * UVRect.height;
		headObj_RawImage.uvRect = UVRect;

	}

	private void RobUpdate()
	{
		childrenAdjustNowTime -= Time.deltaTime;

		if (childrenAdjustNowTime <= 0)
		{
			foreach (Transform obj in childrenObj.transform)
			{
				if (!obj.gameObject.activeSelf) continue;
				if (obj.GetComponent<ChildGauge_ChildIcon>().GetGaugeIconRobFlag()) continue;

				obj.SendMessage("ChangeModeRob");
				break;

			}
			childrenAdjustNowTime = 0.5f;
			adjustChildrenCount--;

		}

		if (adjustChildrenCount <= 0)
			ChangeStatus(E_HEAD_STATUS.CLOSE);


	}

	private void RobFinalize()
	{ }

}
