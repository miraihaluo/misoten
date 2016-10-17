using UnityEngine;
using System.Collections;

/* 注意事項 */
// 変数へのアクセスは
// 関数を通して行うこと
// 又、
// 関数の定義時に
// staticを付けないとダメ

public static class MasterData {

	private static int playerNum = 0;	// 参加しているのプレイヤー人数


	/////////////////////////
	/* 参加プレイヤー関係 */
	////////////////////////

	// 参加人数を引数の人数にする
	// 成功でtrue　失敗でfalseを返す
	public static bool SetPlayerNum(int setNum)
	{
		// 入れようとする人数が、参加可能人数より多いと失敗
		if (setNum > Define.MAX_PLAYER_NUM) return false;

		// 入れようとする人数が、0より小さいと失敗
		if (setNum < 0) return false;

		playerNum = setNum;

		return true;
	}

	// 参加人数に引数分、人数を加える
	// 成功でtrue　失敗でfalseを返す
	public static bool AddPlayerNum(int addNum)
	{
		// 参加可能人数を超過しようとすると失敗
		if (playerNum + addNum > Define.MAX_PLAYER_NUM) return false;

		// 参加人数を0より小さくしようとすると失敗
		if (playerNum + addNum < 0) return  false;

		playerNum += addNum;

		return true;
	}

	// 参加しているプレイヤー人数を返す
	public static int GetPlayerNum() { return playerNum; }



}
