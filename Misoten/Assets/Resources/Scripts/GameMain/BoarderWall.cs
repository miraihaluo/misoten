using UnityEngine;
using System.Collections;

public class BoarderWall : MonoBehaviour {

	// 移動限界を表現する壁オブジェクトのプレハブ
	// インスペクターで指定する
	[SerializeField]
	private GameObject wallPrefab;

	// 生成した壁オブジェを格納するGameObject配列
	private GameObject[] wallObjArray;

	// 壁の高さ
	// インスペクターで設定する
	[SerializeField]
	private float height;

	// スケールで使うベクトル
	private Vector3 scale;

	// デファインデータ
	private DefineData	defineData;

	// Use this for initialization
	void Start () {

		// 壁オブジェ配列の要素数を設定
		wallObjArray = new GameObject[4];

		scale = new Vector3(defineData.STAGE_BOARDER_SIZE, height, 1.0f);

		// 必要な壁オブジェ分、プレハブから生成する
		for (int i = 0; i < wallObjArray.Length; i++)
		{
			// 生成したGameObjectを配列に格納する
			wallObjArray[i] = Instantiate(wallPrefab);

			// 生成したGameObjectの親をこのオブジェクトに設定し、親子関係にする
			wallObjArray[i].transform.parent = this.transform;

			// xとyの大きさを移動限界値分拡大する
			wallObjArray[i].transform.localScale = scale;

			// y位置を高さの半分分、プラス方向に移動する
			wallObjArray[i].transform.Translate(0.0f, height / 2.0f, 0.0f);

		}

		// 壁オブジェの配置調整
		ArrangeWalls();

	}

	void Awake()
	{
		defineData = Resources.Load<DefineData>("Assets/DefineData");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void ArrangeWalls()
	{
		// プラスZ側の壁
		wallObjArray[0].name = "+Z";
		wallObjArray[0].transform.Translate(0.0f, 0.0f, defineData.STAGE_BOARDER_SIZE / 2.0f);

		// マイナスZ側の壁
		wallObjArray[1].name = "-Z";
		wallObjArray[1].transform.Translate(0.0f, 0.0f, -(defineData.STAGE_BOARDER_SIZE / 2.0f));
		wallObjArray[1].transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

		// プラスX側の壁
		wallObjArray[2].name = "+X";
		wallObjArray[2].transform.Translate(defineData.STAGE_BOARDER_SIZE / 2.0f, 0.0f, 0.0f);
		wallObjArray[2].transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
	
		// マイナスX側の壁
		wallObjArray[3].name = "-X";
		wallObjArray[3].transform.Translate(-(defineData.STAGE_BOARDER_SIZE / 2.0f), 0.0f, 0.0f);
		wallObjArray[3].transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);

	}
}
