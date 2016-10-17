using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	// 追従するGameObject（プレイヤー）
	// インスペクターで指定する
	[SerializeField]
    private Transform targetPlayer;

	// 追従するプレイヤーからの相対位置
	// インスペクターから設定する
	[SerializeField]
    private Vector3 posOffset;

	// 追従するプレイヤーからの相対位置で注視点を指定
	// (0, 0, 0)で親オブジェの原点を注視
	// インスペクターから設定する
	[SerializeField]
	private Vector3 lockAtOffset;

	// Use this for initialization
	void Start () {

		// 追従するプレイヤーを親にする
		transform.parent = targetPlayer;

		// 自分のポジションに、追従するプレイヤーからの相対位置を代入する
		transform.position = targetPlayer.position + posOffset;

	}

	// Update is called once per frame
	void LateUpdate () {

	}
}
