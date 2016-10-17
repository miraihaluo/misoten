using UnityEngine;
using System.Collections;

/* 注意事項 */
// 変数を追加する際は、
// static 又は const を
// 付けないといけない

public static class Define
{
	///////////////////
	/* 全シーン共有 */
	//////////////////

	public const int MAX_PLAYER_NUM = 4;		// 最大参加可能人数

	// プレイヤーカラー
	public static int[][] PLAYER_RBGACOLOR_ARRAY = new int[][]
	{
		//			R			G			B			A
		new int[] {	255,		0,			0,			0},	// 1p
		new int[] {	0,			0,			255,		0},	// 2p
		new int[] {	0,			255,		0,			0},	// 3p
		new int[] {	255,		0,			255,		0}	// 4p
		
	};

	///////////////////
	/* QuarterScene */
	//////////////////

	public const int STAGE_BOARDER_SIZE = 300;	// キャラクターの行動限界範囲

}
