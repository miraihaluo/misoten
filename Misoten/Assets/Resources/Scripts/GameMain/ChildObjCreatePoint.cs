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
	/// 次に子供オブジェをアクティブにするまでの時間
	/// </summary>
	private float activeCoolTime;

	/// <summary>
	/// 存在する最大子供の人数
	/// </summary>
	private const int MAX_CHILDREN = 20;

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

		}
	
	}

	void Awake()
	{
		prefabChildObj = (GameObject)Resources.Load("Prefabs/GameMain/UIs/MiniMap_Child");

	}

	// Update is called once per frame
	void Update () {


	}

}
