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

		MAX
	}

	/// <summary>
	/// カメの頭の状態
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

	// Use this for initialization
	void Start () {
		headAnimationNowTime = headAnimationTime;
		GainChild(1);
	
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

	//	if (headOperateFlagStack == 0) return;

		headAnimationNowTime -= Time.deltaTime;

		if (headAnimationNowTime <= 0)
		{
			headAnimationNowTime = headAnimationTime;
			eHeadStatus = (E_HEAD_STATUS)(((int)eHeadStatus + 1) % (int)E_HEAD_STATUS.MAX);
			UVRect.y = (int)eHeadStatus * UVRect.height;
			headObj_RawImage.uvRect = UVRect;

		}

	}

	/// <summary>
	/// 子供を獲得した時の処理
	/// </summary>
	/// <param name="num">獲得した子供の人数</param>
	public void GainChild(int num)
	{
		for (int i = 0; i < num; i++)
		{
			instanseObj = Instantiate(prefab_ChildrenIconObj);
			instanseObj.transform.parent = childrenObj.transform;
			instanseObj.transform.localPosition = Vector3.zero;

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

}
