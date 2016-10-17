using UnityEngine;
using System.Collections;

public class MiniMapIcon : MonoBehaviour {

	// 追従するGameObject（プレイヤー）
	// インスペクターで指定
	[SerializeField]
	private Transform targetPlayer;

	// プレビュー上の追従するプレイヤーとの相対位置を格納
	private Vector3 offset;

	// プレイヤーが行動可能なエリアの大きさ
	private int stage_size;

	// スケールで使うベクトル
	// (1.0f, 1.0f, 1.0f)で初期化
	private Vector3 scale = Vector3.one;

	// ミニマップに対する大きさの比率
	// インスペクターで指定
	[SerializeField]
	private float sizePerMiniMap;

	// Use this for initialization
	void Awake()
	{
		// 追従するプレイヤーを親にする
		transform.parent = targetPlayer;

		// 行動可能なエリアの大きさを取得
		stage_size = Define.STAGE_BOARDER_SIZE;

		// エリアの大きさを、XとYのスケールにそのまま代入し、反映させる
		// プレイヤーの子の関係になっているので、プレイヤーのScaleの影響を受ける
		// Scaleを変更する際は上記を考慮して、代入ではなく、現在のScaleと乗算する
		scale.x = scale.y = stage_size * (sizePerMiniMap / 100.00f);
		transform.localScale = Vector3.Scale(transform.localScale, scale);

	}

	// Update is called once per frame
	void Update()
	{

	}
}
