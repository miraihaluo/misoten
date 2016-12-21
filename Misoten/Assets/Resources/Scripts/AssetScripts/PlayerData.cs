using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "CustomAssets/PlayerData")]
public class PlayerData : ScriptableObject
{

	[SerializeField]
	private int playerNum = 0;	// 参加しているのプレイヤー人数

	[SerializeField]
	private int[] playerScoreArray = null;

	[SerializeField]
	public readonly Color[] playerColorArray = {	new Color(255, 0, 0, 255),
											new Color(0, 0, 255, 255),
											new Color(0, 255, 0, 255),
											new Color(255, 255, 0, 255)};

	private const int MAX_PLAYER_NUM = 4;

	/////////////////////////
	/* 参加プレイヤー関係 */
	////////////////////////

	///<summary>
	///参加人数を引数の人数にする。
	///参加可能人数を超えると処理はされない
	///</summary>
	///<param name="setNum">人数にしたい値</param>
	///<returns>true:成功　false:失敗</returns>
	public bool SetPlayerNum(int setNum)
	{
		// 入れようとする人数が、参加可能人数より多いと失敗
		if (setNum > MAX_PLAYER_NUM) return false;

		// 入れようとする人数が、0より小さいと失敗
		if (setNum < 0) return false;

		playerNum = setNum;

		return true;
	}

	///<summary>
	///参加人数に引数の値を加える。
	///参加可能人数を超えると処理はされない
	///</summary>
	///<param name="addNum">人数に加えたい値</param>
	///<returns>true:成功　false:失敗</returns>
	public bool AddPlayerNum(int addNum)
	{
		// 参加可能人数を超過しようとすると失敗
		if (playerNum + addNum > MAX_PLAYER_NUM) return false;

		// 参加人数を0より小さくしようとすると失敗
		if (playerNum + addNum < 0) return false;

		playerNum += addNum;

		return true;
	}

	///<summary>
	///現在の参加人数を返す
	///</summary>
	///<returns>現在の参加人数</returns>
	public int GetPlayerNum() { return playerNum; }


	////////////////
	/* スコア関係 */
	////////////////

	/// <summary>
	///スコア配列の要素数を現在の参加人数で確定する
	/// </summary>
	/// <returns>true:成功　false:失敗</returns>
	public bool ScoreArrayInstance()
	{
		if (playerScoreArray.Length > 0) return false;
		if (playerNum <= 0) return false;

		playerScoreArray = new int[playerNum];

		return true;
	
	}

	///<summary>
	///スコア配列にスコアを加算する
	///</summary>>
	///<paparam name="ID">スコアを加算するプレイヤー番号</paparam>
	///<param name="addScore">加算するスコア値</param>
	///<returns>true:成功　false:失敗</returns>
	public bool AddPlayerScoreArray(int ID, int addScore)
	{
		if (ID < 0 || ID > playerNum - 1) return false;
		if (addScore < 0) return false;

		playerScoreArray[ID] += addScore;

		return true;
	}

	/// <summary>
	/// スコア配列を取得する
	/// </summary>
	/// <returns>スコア配列</returns>
	public int[] GetPlayerScoreArray() { return playerScoreArray; }

}
