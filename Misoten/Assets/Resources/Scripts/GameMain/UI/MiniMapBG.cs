using UnityEngine;
using System.Collections;

public class MiniMapBG : MonoBehaviour {

	// プレイヤーが行動可能なエリアの大きさ
	private int stage_size;

	// スケールで使うベクトル
	// (1.0f, 1.0f, 1.0f)で初期化
	private Vector3 scale = Vector3.one;

	// Use this for initialization
	void Start () {

		// 行動可能なエリアの大きさを取得
		stage_size = Define.STAGE_BOARDER_SIZE;

		// エリアの大きさを、XとYのスケールにそのまま代入し、反映させる
		scale.x = scale.y = stage_size;
		transform.localScale = scale;

	}

	void Update()
	{
	}

}
