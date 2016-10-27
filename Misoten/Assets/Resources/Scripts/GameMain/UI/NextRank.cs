using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextRank : MonoBehaviour {

	/// <summary>
	/// 表示する情報の対象プレイヤー
	/// </summary>
	[SerializeField]
	private PlayerControl targetPlayer;

	/// <summary>
	/// UV値をいじる為のrect変数
	/// </summary>
	private Rect UVRect;

	/// <summary>
	/// 操作するRawImageスクリプト
	/// </summary>
	private RawImage rawImageObj;

	void Awake()
	{
		rawImageObj = GetComponent<RawImage>();

		// 初期値のUVデータを取得する
		UVRect = rawImageObj.uvRect;

		UpdateNextRank();
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void UpdateNextRank()
	{
		if (targetPlayer.Rank == 1)
		{
			// 1位の時は2位を表示する
			UVRect.x = targetPlayer.Rank * UVRect.width;
			rawImageObj.uvRect = UVRect;

		}
		else
		{
			UVRect.x = (targetPlayer.Rank - 2) * UVRect.width;
			rawImageObj.uvRect = UVRect;

		}
	
	}

}
