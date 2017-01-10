using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChildObjCreatePoint : MonoBehaviour {

	/// <summary>
	/// ミニマップの子供アイコンプレハブ
	/// </summary>
	private GameObject prefabChildObj;
	private GameObject instansChildObj;

	/// <summary>
	/// このUpdateを走らすかどうかのフラグ
	/// </summary>
	private bool activeFlag = false;

	/// <summary>
	/// 次に子供オブジェをアクティブにするまでの時間
	/// </summary>
	private float activeCoolTime;

	/// <summary>
	/// 次に特別な子供オブジェをアクティブにするまでの時間
	/// </summary>
	private float specialActiveCoolTime = 5;

	/// <summary>
	/// リスポンのランダム範囲
	/// </summary>
	private Vector2 activeCoolTimeRange = new Vector2(2, 4);

	/// <summary>
	/// リスポンさせる子供のID
	/// </summary>
	private int activeChildID;

	/// <summary>
	/// 存在する最大子供の人数
	/// </summary>
	[SerializeField, Header("普通の子供の出現する最大個数")]
	private int MAX_CHILDREN = 5;

	/// <summary>
	/// 
	/// </summary>
	[SerializeField, Header("特別の子供の出現する最大個数")]
	private int MAX_SPECIAL_CHILDREN = 5;

	/// <summary>
	/// アクティブ状態の子供の個数
	/// </summary>
	private int activeChildCount;

	/// <summary>
	/// ミニマップの子供アイコンを生成するオブジェ
	/// </summary>
	[SerializeField, Header("ミニマップアイコン生成用オブジェ")]
	private Transform[] miniMapChildIconObj;

	/// <summary>
	/// 普通の子供管理オブジェ
	/// </summary>
	[SerializeField, Header("普通の子供の管理オブジェ")]
	private Transform normalChildrenObj;

	/// <summary>
	/// 特別な子供管理オブジェ
	/// </summary>
	[SerializeField, Header("特別な子供の管理オブジェ")]
	private Transform specialChildrenObj;

	private Vector3 childPos = Vector3.zero;

	private bool feverFlag = false;

	// Use this for initialization
	void Start()
	{
		// 普通の子供のミニマップアイコンをインスタンス
		foreach (Transform childObj in normalChildrenObj)
		{
			// 1p～4pの四つ分生成する
			for (int i = 0; i < 4; i++)
			{
				instansChildObj = Instantiate(prefabChildObj);
				instansChildObj.transform.parent = miniMapChildIconObj[i];
				childObj.GetComponent<ChildObj>().miniMapIcon[i] = instansChildObj;

				// (ミニマップの大きさ / 2) * 子供オブジェの座標 / (ステージの広さ / 2)
				childPos.x = (200 / 2) * childObj.position.x / (500 / 2);
				childPos.y = (200 / 2) * childObj.position.z / (500 / 2);

				instansChildObj.transform.localPosition = childPos;

			}

			childObj.SendMessage("ActiveOff");

		}
		
		// 特別の子供のミニマップアイコンをインスタンス
		foreach (Transform childObj in specialChildrenObj)
		{
			// 1p～4pの四つ分生成する
			for (int i = 0; i < 4; i++)
			{
				instansChildObj = Instantiate(prefabChildObj);
				instansChildObj.transform.parent = miniMapChildIconObj[i];
				childObj.GetComponent<ChildObj>().miniMapIcon[i] = instansChildObj;

				// (ミニマップの大きさ / 2) * 子供オブジェの座標 / (ステージの広さ / 2)
				childPos.x = (200 / 2) * childObj.position.x / (500 / 2);
				childPos.y = (200 / 2) * childObj.position.z / (500 / 2);

				instansChildObj.transform.localPosition = childPos;
				instansChildObj.GetComponent<RawImage>().color = Color.green;

			}

			childObj.SendMessage("ActiveOff");

		}

		activeCoolTime = Random.Range(activeCoolTimeRange.x, activeCoolTimeRange.y);

		for (int i = 0; i < MAX_CHILDREN; i++)
			ActiveChild(normalChildrenObj);

		activeFlag = true;

	}

	void Awake()
	{
		prefabChildObj = (GameObject)Resources.Load("Prefabs/GameMain/UIs/MiniMap_Child");

	}

	// Update is called once per frame
	void Update () {
		if (!activeFlag) return;

		NormalChildUpdate();
		SpecialChildUpdate();

	}

	private void NormalChildUpdate()
	{
		// アクティブの子供の個数を数える
		activeChildCount = 0;
		foreach (Transform child in normalChildrenObj.transform)
		{
			if (child.gameObject.activeSelf) activeChildCount++;

		}

		if (!feverFlag)
		{
			if (activeChildCount > MAX_CHILDREN) return;
		}
		else
		{
			if (activeChildCount >= normalChildrenObj.childCount) return;
		
		}


			activeCoolTime -= Time.deltaTime;

		if (activeCoolTime <= 0)
		{
			activeCoolTime = Random.Range(activeCoolTimeRange.x, activeCoolTimeRange.y);
			ActiveChild(normalChildrenObj);

		}

	}

	private void SpecialChildUpdate()
	{
		if (!feverFlag) return;
		activeChildCount = 0;
		foreach (Transform child in specialChildrenObj)
			if (child.gameObject.activeSelf) activeChildCount++;

		if (activeChildCount > MAX_SPECIAL_CHILDREN) return;

		specialActiveCoolTime -= Time.deltaTime;

		if (specialActiveCoolTime <= 0)
		{
			specialActiveCoolTime = 5;
			ActiveChild(specialChildrenObj);
		
		}

	}

	private void ActiveChild(Transform childArray)
	{
		activeChildID = Random.Range(0, childArray.childCount - 1);

		while (childArray.GetChild(activeChildID).gameObject.activeSelf)
			activeChildID = (activeChildID + 1) % childArray.childCount;

		childArray.GetChild(activeChildID).gameObject.SetActive(true);
		childArray.GetChild(activeChildID).SendMessage("ActiveOn");
	
	}

	public void ChangeFever()
	{
		ActiveSpecialChild();
		feverFlag = true;

	}

	public void ActiveSpecialChild()
	{
		foreach (Transform child in specialChildrenObj)
		{
			child.gameObject.SetActive(true);
			child.SendMessage("ActiveOn");
		
		}
	
	}

}
