﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;  // シーン遷移に使う関数がある名前空間
using System;	// Arrayを使うのに必要

public class GameMainSceneController : MonoBehaviour
{

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

	private PlayerControl[] players;

	private int[] scoreArray;
	private int[] sort;

	/// <summary>
	/// プレイヤーの現在のランクを表示するオブジェ群を格納している配列
	/// </summary>
	[SerializeField]
	private Rank[] nowRankObjArray;

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

	/// <summary>
	/// ランキングオブジェ配列
	/// </summary>
	[SerializeField]
	private Rank[] rankingObjArray;

	/// <summary>
	/// ランキングに表示している取得スコアオブジェ配列
	/// </summary>
	[SerializeField]
	private NextScore[] rankingScoreObjArray;

    // Use this for initialization
    void Start(){
		
    }

	void Awake()
	{
		// プレイヤーデータアセットの取得
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");
		
		// プレイヤーオブジェのデータを取得するためテキトーに検索して取得
		GameObject workGameObj = GameObject.Find("Players");
		players = new PlayerControl[workGameObj.transform.childCount];

		for (int i = 0; i < workGameObj.transform.childCount; i++)
		{
			players[i] = workGameObj.transform.GetChild(i).transform.GetComponent<PlayerControl>();

		}

		// スコア配列の参照を取得
		scoreArray = playerData.GetPlayerScoreArray();

		// スコア配列ソート用の配列を別に生成
		sort = new int[scoreArray.Length];

	}

    // Update is called once per frame
    void Update(){

		// 残り時間を減らす
		limitTime -= Time.deltaTime * 1;

		// 特定のボタンを押すか、残り時間が0になったら遷移
        if (Input.GetButtonDown(sceneChangeVirtualKeyName) || limitTime <= 0)
        {
            SceneManager.LoadScene(nextSceneName);

        }

    }

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
			for(int j = 0; j < sort.Length; j++)
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

		foreach (Rank rankingObj in rankingObjArray)
			rankingObj.UpdateNowRank();

		for (int i = 0; i < rankingScoreObjArray.Length; i++)
		{
			rankingScoreObjArray[i].UpdateNextScore(scoreArray[i % 4]);
		
		}

	}

}
