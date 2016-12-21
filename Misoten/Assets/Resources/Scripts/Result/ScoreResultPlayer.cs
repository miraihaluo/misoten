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

	void Awake()
	{
		// プレイヤーデータアセットの取得
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");
		prefab_ChildrenIconObj = (GameObject)Resources.Load("Prefabs/Result/ChildIcon");

		scoreArray = playerData.GetPlayerScoreArray();

		myScore = scoreArray[int.Parse(this.name) - 1];

		emptyobj = new GameObject[(myScore / MAX_CHILD) + 1];

		for (int i = 0; i < emptyobj.Length; i++)
		{
			emptyobj[i] = Instantiate((GameObject)Resources.Load("Prefabs/Result/group"));
			emptyobj[i].transform.parent = emptyParentObj.transform;
			emptyobj[i].transform.localScale = Vector3.one;
		
		}

		for (int i = 0; i < myScore; i++)
		{
			instanseObj = Instantiate(prefab_ChildrenIconObj);
			instanseObj.transform.parent = emptyobj[i / MAX_CHILD].transform;
			instanseObj.transform.localScale = Vector3.one;

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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
