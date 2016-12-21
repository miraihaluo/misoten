using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;  // シーン遷移に使う関数がある名前空間
using System;	// Arrayを使うのに必要
using UnityEngine.UI;

public class TutorialSceneControllar : MonoBehaviour
{
	/// <summary>
	/// ゲームの進行状態
	/// </summary>
	public enum E_STATUS
	{
		MIWATASI,	// 開始のデモムービー
		SYUTUGEN,	// プレイヤーが降りてくる
		START,// UIの出現、開始のカウントダウン
		GAME,	// ゲームメイン
		END,	// 終了動作

		MAX
	};

	private E_STATUS eStatus = E_STATUS.GAME;

	// スペースキーで画面遷移
	public string sceneChangeVirtualKeyName = "Jump";

	[SerializeField]    // 変数をインスペクターから変更できるようにする
	private string nextSceneName;

	/// <summary>
	/// 残り時間
	/// </summary>
	[SerializeField]
	private float limitTime = 60;
	public float NowTime { get { return limitTime; } } // limitTimeのゲッター

	private PlayerData playerData;

	[SerializeField]
	private PlayerControl[] players;

	private int[] scoreArray;
	private int[] sort;

	/// <summary>
	/// ランキングブロックの配列
	/// </summary>
	private int[] rankingBlockRankArray = new int[] { 0, 1, 2, 3 };

	/// <summary>
	/// プレイヤーの現在のランクを表示するオブジェ群を格納している配列
	/// </summary>
	[SerializeField]
	private Rank[] nowRankObjArray;
	/*
	/// <summary>
	/// プレイヤーの次のランクを表示するオブジェ群を格納している配列
	/// </summary>
	[SerializeField]
	private NextRank[] nextRankObjArray;

	/// <summary>
	/// 次の順位に必要なスコアを表示するオブジェ群を格納している配列
	/// </summary>
	[SerializeField]
	private NextScore[] nextScoreArray;
	*/

	/// <summary>
	/// ランキングに表示している取得スコアオブジェ配列
	/// </summary>
	[SerializeField]
	private NextScore[] rankingScoreObjArray;

	/// <summary>
	/// ランキングブロック配列
	/// </summary>
	[SerializeField]
	private RankingBlock[] rankingBlockObjArray;

	//音楽データ
	[SerializeField]
	private AudioSource TimeUP_SE;

	/// <summary>
	/// 四画面スクリーンの親ゲームオブジェ
	/// </summary>
	[SerializeField, Header("四画面スクリーンの親ゲームオブジェ")]
	private GameObject screenParentObj;

	private Vector3[] screenPosition = new Vector3[]{
		new Vector3(-0.5f, 0.5f, 0.0f),
		new Vector3(0.5f, 0.5f, 0.0f),
		new Vector3(-0.5f, -0.5f, 0.0f),
		new Vector3(0.5f, -0.5f, 0.0f),
	};

	/// <summary>
	/// 四つのカメラの親ゲームオブジェ
	/// </summary>
	[SerializeField, Header("四つのカメラの親ゲームオブジェ")]
	private GameObject cameraParentObj;

	/// <summary>
	/// キャンバスオブジェ
	/// </summary>
	[SerializeField, Header("キャンバスオブジェ")]
	private Canvas UIParentObj;

	/// <summary>
	/// 全UIのRawImageオブジェ
	/// </summary>
	private RawImage[] UIRawImageObjArray;

	/// <summary>
	/// UIのWorldIconのゲームオブジェ
	/// </summary>
	[SerializeField, Header("UIのWorldIconのゲームオブジェ")]
	private GameObject woldIconObj;

	/// <summary>
	/// フレームのオブジェ
	/// </summary>
	[SerializeField, Header("フレームオブジェ")]
	private RawImage frameObj;

	/// <summary>
	/// ミッションコントローラーオブジェ
	/// </summary>
	[SerializeField, Header("ミッションコントローラーオブジェ")]
	private TutorialMissionController missionControllerObj;
	private int missionFlag = 0;

	/// <summary>
	/// 子供オブジェ管理オブジェ
	/// </summary>
	[SerializeField, Header("子供オブジェ管理オブジェ")]
	private ChildObjCreatePoint childCreateObj;

	/// <summary>
	/// CPUオブジェ
	/// </summary>
	[SerializeField, Header("CPUオブジェ")]
	private CPU cpuObj;

	//音楽データ
	[SerializeField]
	private AudioSource sound01;

	// Use this for initialization
	void Start()
	{

	}

	void Awake()
	{
		// プレイヤーデータアセットの取得
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");

		// プレイヤーオブジェのデータを取得するためテキトーに検索して取得
		/*		GameObject workGameObj = GameObject.Find("Players");
				players = new PlayerControl[workGameObj.transform.childCount];

				for (int i = 0; i < workGameObj.transform.childCount; i++)
				{
					players[i] = workGameObj.transform.GetChild(i).transform.GetComponent<PlayerControl>();

				}
				*/
		// スコア配列の参照を取得
		scoreArray = playerData.GetPlayerScoreArray();

		// スコア配列ソート用の配列を別に生成
		sort = new int[scoreArray.Length];
		
	}

	// Update is called once per frame
	void Update()
	{
		switch (eStatus)
		{
			case E_STATUS.MIWATASI:
				MiwatasiUpdate();
				break;

			case E_STATUS.SYUTUGEN:
				SyutugenUpdate();
				break;

			case E_STATUS.START:
				StartUpdate();
				break;

			case E_STATUS.GAME:
				GameUpdate();
				break;

			case E_STATUS.END:
				EndUpdate();
				break;

		}

		// 特定のボタンを押すか、残り時間が0になったら遷移
		if (Input.GetButtonDown(sceneChangeVirtualKeyName))
		{
			sound01.Play();
			//FadeManager.Instance.LoadLevel(nextSceneName, 1.0f);

			SceneManager.LoadScene(nextSceneName);

		}

	}

	private void ChangeStatus(E_STATUS changeStatus)
	{
		// 終了処理
		switch (eStatus)
		{
			case E_STATUS.MIWATASI:
				MiwatasiFinalize();
				break;

			case E_STATUS.SYUTUGEN:
				SyutugenFinalize();
				break;

			case E_STATUS.START:
				StartFinalize();
				break;

			case E_STATUS.GAME:
				GameFinalize();
				break;

			case E_STATUS.END:
				EndFinalize();
				break;

		}

		eStatus = changeStatus;

		switch (eStatus)
		{
			case E_STATUS.MIWATASI:
				MiwatasiInitialize();
				break;

			case E_STATUS.SYUTUGEN:
				SyutugenInitialize();
				break;

			case E_STATUS.START:
				StartInitialize();
				break;

			case E_STATUS.GAME:
				GameInitialize();
				break;

			case E_STATUS.END:
				EndInitialize();
				break;

		}

	}

	private void MiwatasiInitialize()
	{
		// 四つのスクリーンを真ん中に置いて画面いっぱいの大きさ（二倍）にする
		foreach (Transform child in screenParentObj.transform)
		{
			child.localPosition = Vector3.zero;
			child.localScale = Vector3.one * 2;

		}

		// 1pにあたるスクリーン以外を非アクティブにする
		for (int i = 1; i < screenParentObj.transform.childCount; i++)
		{
			screenParentObj.transform.GetChild(i).gameObject.SetActive(false);

		}

		// 1pにあたるカメラ以外を非アクティブにする
		for (int i = 1; i < cameraParentObj.transform.childCount; i++)
		{
			cameraParentObj.transform.GetChild(i).gameObject.SetActive(false);

		}

	}

	private void MiwatasiUpdate()
	{ }

	private void MiwatasiFinalize()
	{

	}

	/// <summary>
	/// 四つの画面が分かれる所要時間
	/// </summary>
	[SerializeField, Header("四つの画面が分かれる所要時間")]
	private float SYUTUGEN_TIME = 1.0f;
	private float syutugenNowTime;

	private void SyutugenInitialize()
	{
		// 四つの画面の大きさを1にする
		for (int i = 0; i < screenParentObj.transform.childCount; i++)
		{
			screenParentObj.transform.GetChild(i).gameObject.SetActive(true);
			//			screenParentObj.transform.GetChild(i).localScale = Vector3.one;
			//			screenParentObj.transform.GetChild(i).localPosition = screenPosition[i];

		}

		// カメラの初期化
		for (int i = 0; i < cameraParentObj.transform.childCount; i++)
		{
			cameraParentObj.transform.GetChild(i).gameObject.SetActive(true);
			cameraParentObj.transform.GetChild(i).transform.localPosition = cameraParentObj.transform.GetChild(0).transform.localPosition;
			cameraParentObj.transform.GetChild(i).transform.localRotation = cameraParentObj.transform.GetChild(0).transform.localRotation;
			cameraParentObj.transform.GetChild(i).SendMessage("ChangeStatus", E_STATUS.SYUTUGEN);

		}

		foreach (PlayerControl playObj in players)
			playObj.ChangePlayerStatus(PlayerControl.E_STATUS.SYUTUGEN);

		frameObj.gameObject.SetActive(true);

		syutugenNowTime = 0;

	}

	private void SyutugenUpdate()
	{
		syutugenNowTime += (Time.deltaTime / SYUTUGEN_TIME);

		for (int i = 0; i < screenParentObj.transform.childCount; i++)
		{
			screenParentObj.transform.GetChild(i).localScale = Vector3.Lerp(Vector3.one * 2, Vector3.one, syutugenNowTime);
			screenParentObj.transform.GetChild(i).localPosition = Vector3.Lerp(Vector3.zero, screenPosition[i], syutugenNowTime);

		}

		if (syutugenNowTime >= 1.0f)
			ChangeStatus(E_STATUS.START);

	}

	private void SyutugenFinalize()
	{
		foreach (Transform child in cameraParentObj.transform)
		{
			child.SendMessage("ChangeStatus", E_STATUS.START);

		}

	}

	/// <summary>
	/// UIが出てくる所要時間
	/// </summary>
	[SerializeField, Header("UIが出てくる所要時間")]
	private float UI_SYUTUGEN_TIME = 1.0f;
	private float UIsyutugenNowTime = 0;
	private Color start_color = new Color(1, 1, 1, 0);

	private void StartInitialize()
	{
		foreach (PlayerControl playObj in players)
			playObj.ChangePlayerStatus(PlayerControl.E_STATUS.START);

		/*
		foreach (RawImage UIObj in UIRawImageObjArray)
			UIObj.color = start_color;
		*/
		UIsyutugenNowTime = 0;

	}

	private void StartUpdate()
	{
		UIsyutugenNowTime += Time.deltaTime / UI_SYUTUGEN_TIME;

		foreach (RawImage UIObj in UIRawImageObjArray)
		{
			start_color = UIObj.color;
			start_color.a = Mathf.Lerp(0.0f, 1.0f, UIsyutugenNowTime);
			UIObj.color = start_color;
		}

		if (UIsyutugenNowTime >= 1.0f)
			ChangeStatus(E_STATUS.GAME);

	}

	private void StartFinalize()
	{
		foreach (Transform child in cameraParentObj.transform)
		{
			child.SendMessage("ChangeStatus", E_STATUS.GAME);

		}

	}

	private void GameInitialize()
	{
		foreach (PlayerControl playObj in players)
			playObj.ChangePlayerStatus(PlayerControl.E_STATUS.ACTIVE);

		woldIconObj.SetActive(true);

	}

	private void GameUpdate()
	{
		// 残り時間を減らす
		limitTime -= Time.deltaTime * 1;

		if (limitTime < 60 && missionFlag <= 0)
		{
			missionControllerObj.CallMissionText(TutorialMissionController.E_MISSION_TYPE.MAEOKI);
			missionFlag++;
		}

		if (limitTime < 50 && missionFlag <= 1)
		{
			missionControllerObj.CallMissionText(TutorialMissionController.E_MISSION_TYPE.ZENSIN_KOUTAI);
			missionFlag++;
		}

		if (limitTime < 40 && missionFlag <= 2)
		{
			missionControllerObj.CallMissionText(TutorialMissionController.E_MISSION_TYPE.KAITEN);
			missionFlag++;
		}

		if (limitTime < 30 && missionFlag <= 3)
		{
			missionControllerObj.CallMissionText(TutorialMissionController.E_MISSION_TYPE.UP_DOWN);
			missionFlag++;
		}

		if (limitTime < 20 && missionFlag <= 4)
		{
			missionControllerObj.CallMissionText(TutorialMissionController.E_MISSION_TYPE.CHILD);
			missionFlag++;
		}

		if (limitTime < 15 && missionFlag <= 5)
		{
			missionControllerObj.CallMissionText(TutorialMissionController.E_MISSION_TYPE.GAOL);
			missionFlag++;
		}

		if (limitTime < 10 && missionFlag <= 6)
		{
			missionControllerObj.CallMissionText(TutorialMissionController.E_MISSION_TYPE.ATTACK);
			missionFlag++;
		}


		if (limitTime <= 0)
			ChangeStatus(E_STATUS.END);

	}

	private void GameFinalize()
	{ }

	private void EndInitialize()
	{
		playerData.ResetPlayerScoreArray();
	
	}

	private void EndUpdate()
	{
		//FadeManager.Instance.LoadLevel(nextSceneName, 1.0f);
		SceneManager.LoadScene(nextSceneName);

	}

	private void EndFinalize()
	{ }

	public void SetStatus(E_STATUS newStatus)
	{
		ChangeStatus(newStatus);

	}

	private int[] work = new int[4];
	public void PlayerRankUpdate()
	{
		// スコア配列をソート用配列にコピー
		Array.Copy(scoreArray, sort, scoreArray.Length);

		// ソート用配列を昇順にソート
		Array.Sort(sort);

		// ソート用配列を降順に変更
		Array.Reverse(sort);

		// i がプレイヤーID、 j が順位に対応
		for (int i = 0; i < scoreArray.Length; i++)
		{
			for (int j = 0; j < sort.Length; j++)
			{
				// IDが i 番のプレイヤーのスコアが、j 位のスコアと同じか比較
				if (scoreArray[i] == sort[j])
				{
					// j はゼロ始まり、順位はイチ始まりなので、+1をして合わせる
					players[i].Rank = j + 1;
					break; // j のfor文から抜ける

				}

			}

		}

		foreach (Rank rankObj in nowRankObjArray)
			rankObj.UpdateNowRank();
		/*
		foreach (NextRank nextRankObj in nextRankObjArray)
			nextRankObj.UpdateNextRank();

		for (int i = 0; i < nextScoreArray.Length; i++)
		{
			// １位の時は２位との差を表示する
			if (players[i].Rank == 1)
			{
				// 自分のスコアから、１つ下のランクのスコアを引いた値を送る
				nextScoreArray[i].UpdateNextScore(sort[players[i].Rank - 1] - sort[players[i].Rank]);
			}
			else
			{
				// 一つ上のスコアから、自分のスコアを引いた値を送る
				nextScoreArray[i].UpdateNextScore(sort[players[i].Rank - 2] - sort[players[i].Rank - 1]);
			
			}
		}
		*/

		for (int i = 0; i < rankingScoreObjArray.Length; i++)
			rankingScoreObjArray[i].UpdateNextScore(scoreArray[i % 4]);

		/*** ランキングブロックの処理 ***/
		for (int i = 0; i < work.Length; i++)
			work[i] = -1;

		// 要素番号を順位に、中身をプレイヤー番号で、work配列に並べていく
		for (int playerID = 0; playerID < rankingBlockRankArray.Length; playerID++)
		{
			for (int ranking = 0; ranking < sort.Length; ranking++)
			{
				if (scoreArray[rankingBlockRankArray[playerID]] != sort[ranking]) continue;
				if (work[ranking] != -1) continue;  // 同率なら、先に高い順位だった方が高い順位になる

				work[ranking] = rankingBlockRankArray[playerID];
				break;

			}

		}

		// 要素番号をプレイヤー番号に、中身を順位に変換する。
		for (int i = 0; i < rankingBlockRankArray.Length; i++)
			rankingBlockRankArray[work[i]] = i;

		// workの中身をランキングブロックの順位を入れている配列にコピー
		//work.CopyTo(rankingBlockRankArray, 0);

		// 各ランキングブロックに順位を入れる
		for (int playerID = 0; playerID < rankingBlockObjArray.Length; playerID++)
			rankingBlockObjArray[playerID].UpdateRanking(rankingBlockRankArray[playerID % 4] + 1);

	}

}
