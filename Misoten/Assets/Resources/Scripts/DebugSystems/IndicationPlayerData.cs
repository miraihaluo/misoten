using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class IndicationPlayerData : MonoBehaviour {

	// UIで表示する用のテキストオブジェクト
	[SerializeField]
	private Text TextObj;

	//プレイヤーデータを保持しているアセットを取得
	private PlayerData playerData;

	// Use this for initialization
	void Awake()
	{
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");
	
	}
	
	// Update is called once per frame
	void Update () {

		// TextObjの初期化
		TextObj.text = "";

		TextObj.text = "参加人数：" + playerData.GetPlayerNum() + "\n";

		if (playerData.GetPlayerScoreArray().Length != 0)
		{
			TextObj.text += "プレイヤーのスコア\n";

			for (int i = 0; i < playerData.GetPlayerScoreArray().Length; i++)
			{
				TextObj.text += (i + 1) + "p" + playerData.GetPlayerScoreArray()[i] + "\n";
			
			}

		}
		else
			TextObj.text += "スコア配列が存在しません\n";

	}

}
