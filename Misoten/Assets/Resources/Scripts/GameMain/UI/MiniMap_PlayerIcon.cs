using UnityEngine;
using System.Collections;
using UnityEngine.UI;	// UI弄るのに必要

public class MiniMap_PlayerIcon : MonoBehaviour {

	private Image imageObj;
	private Vector3 pos = Vector3.one;
	private Quaternion rot = Quaternion.identity;

	[SerializeField]
	private GameObject targetPlayer;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		imageObj = this.GetComponent<Image>();
	
	}


	// Update is called once per frame
	void Update () {
		// (ミニマップの大きさ / 2) * プレイヤーの座標 / (ステージの広さ / 2)
		pos.x = (200 / 2) * targetPlayer.transform.position.x / (300 / 2);
		pos.y = (200 / 2) * targetPlayer.transform.position.z / (300 / 2);

		imageObj.transform.localPosition = pos;

		rot.z = targetPlayer.transform.rotation.y;	// プレイヤーのY軸回転をZ軸回転として受け取る
		rot.w = -targetPlayer.transform.rotation.w;	// 回転の実数値
		imageObj.transform.rotation = rot;

	}
}
