using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreResultPlayer : MonoBehaviour {

	private PlayerData playerData;

	/// <summary>
	/// 子供アイコンのプレハブ
	/// </summary>
	private GameObject prefab_ChildrenIconObj;

	/// <summary>
	/// インスタンスを保存する一時オブジェ
	/// </summary>
	private GameObject instanseObj;

	[SerializeField]
	private GameObject emptyParentObj;

	[SerializeField]
	private Text textObj;
	private int textSore = 0;

	[SerializeField]
	private RawImage rankObj;

	private GameObject[] emptyobj;

	[SerializeField]
	private int MAX_CHILD = 50;

	private int myScore;

	private int[] scoreArray = new int[4];
	private int[] sort = new int[4];
	private int myRank;
	private Rect UVRect;
	private Vector3 defPos = new Vector3(0, 1100, 0);
	private Vector3 childPos;
	[SerializeField, Header("子供アイコンの隙間")]
	private float childIconMarge = 10;

	[SerializeField, Header("子供アイコンの横のずらす距離")]
	private float childIconYoko = 50;

	[SerializeField, Header("子供アイコンの落ちる速度")]
	private float fallSpedd = 500;

	[SerializeField, Header("空のオブジェを横にずらす距離")]
	private float emptyObjYoko = 25;

	private int rightleftPos = -1;

	private int emptyCount = 0;
	private int childCount = 0;

	private int emptyNum = 0;

	void Awake()
	{
		// プレイヤーデータアセットの取得
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");
		prefab_ChildrenIconObj = (GameObject)Resources.Load("Prefabs/Result/ChildIcon");

		scoreArray = playerData.GetPlayerScoreArray();

		myScore = scoreArray[int.Parse(this.name) - 1];

		// 空のGameObjの生成
		emptyNum = (myScore / MAX_CHILD);
		if ((myScore % MAX_CHILD) == 0) emptyNum--;
		emptyNum++;
		emptyobj = new GameObject[emptyNum];
		for (int i = 0; i < emptyobj.Length; i++)
		{
			emptyobj[i] = Instantiate((GameObject)Resources.Load("Prefabs/Result/group"));
			emptyobj[i].transform.parent = emptyParentObj.transform;
			emptyobj[i].transform.localScale = Vector3.one;
			emptyobj[i].transform.localPosition = Vector3.zero;
		
		}

		// 子供オブジェの生成
		for (int i = 0; i < myScore; i++)
		{
			instanseObj = Instantiate(prefab_ChildrenIconObj);
			instanseObj.transform.parent = emptyobj[i / MAX_CHILD].transform;
			instanseObj.transform.localScale = Vector3.one;
			instanseObj.transform.localPosition = Vector3.zero;

		}

		// スコア配列をソート用配列にコピー
		Array.Copy(scoreArray, sort, scoreArray.Length);

		// ソート用配列を昇順にソート
		Array.Sort(sort);

		// ソート用配列を降順に変更
		Array.Reverse(sort);

		// i がプレイヤーID、 j が順位に対応
		for (int i = 0; i < 1; i++)
		{
			for (int j = 0; j < sort.Length; j++)
			{
				// IDが i 番のプレイヤーのスコアが、j 位のスコアと同じか比較
				if (scoreArray[int.Parse(this.name) - 1] == sort[j])
				{
					myRank = j;
					break; // j のfor文から抜ける

				}

			}

		}

		UVRect = rankObj.uvRect;

	}

	// Use this for initialization
	void Start () {
		textObj.text = myScore.ToString();

		UVRect.x = myRank * UVRect.width;
		rankObj.uvRect = UVRect;

		foreach (GameObject child in emptyobj)
		{
			foreach (Transform cchild in child.transform)
			{
				cchild.localPosition = defPos;
			}
		
		}

		childPos = defPos;
		childPos.x = -childIconYoko;

		textObj.text = textSore.ToString();
		rankObj.color = Color.clear;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!FallChildren()) return;

		rankObj.color = Color.white;

	}

	bool FallChildren()
	{
		if (emptyCount >= emptyobj.Length) return true;

		// 子供を落とす
		childPos.y -= fallSpedd * Time.deltaTime;
		emptyobj[emptyCount].transform.GetChild((emptyobj[emptyCount].transform.childCount - 1) - childCount).localPosition = childPos;

		// 場所に付いたら
		if (childPos.y <= ((childCount % (MAX_CHILD / 2)) * childIconMarge))
		{
			childPos.y = ((childCount % (MAX_CHILD / 2)) * childIconMarge);
			emptyobj[emptyCount].transform.GetChild((emptyobj[emptyCount].transform.childCount - 1) - childCount).localPosition = childPos;
			childPos.y = defPos.y;
			childCount++;

			textSore++;
			textObj.text = textSore.ToString();

			if (childCount % (MAX_CHILD / 2) == 0) childPos.x *= -1;

			if (childCount >= emptyobj[emptyCount].transform.childCount)
			{
				childCount = 0;
				emptyCount++;

				if (emptyCount >= emptyobj.Length) return false;

				for (int i = emptyCount - 1; i >= 0; i-- )
					emptyobj[i].transform.Translate(-emptyObjYoko, 0.0f, 0.0f);

			}

		}

		return false;
	}

}
