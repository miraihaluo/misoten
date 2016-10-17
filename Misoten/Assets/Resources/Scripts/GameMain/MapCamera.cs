using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {

	// カメラオブジェ
	// インスペクターから指定する
	[SerializeField]
	private Camera cameraObj;

	// Use this for initialization
	void Start () {

		// 画面サイズを移動限界範囲の半分の大きさに設定する
		cameraObj.orthographicSize = Define.STAGE_BOARDER_SIZE / 2.0f;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
