using UnityEngine;
using System.Collections;
using UnityEngine.UI;	// UI弄るのに必要

public class MiniMap_PlayerIcon : MonoBehaviour {

	private enum E_RAWOBJ
	{
		ICON,
		VEC,

		MAX
	}

	private RawImage[] imageObjArray;
	private Vector3 pos = Vector3.one;
	private Quaternion rot = Quaternion.identity;
	private PlayerData playerDataObj;

	[SerializeField]
	private PlayerControl targetPlayer;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		playerDataObj = Resources.Load<PlayerData>("Assets/PlayerData");

		imageObjArray = new RawImage[(int)E_RAWOBJ.MAX];

		foreach (Transform child in transform)
		{
			if (child.name == "PlayerIcon")
				imageObjArray[(int)E_RAWOBJ.ICON] = child.GetComponent<RawImage>();
			if (child.name == "Vec")
				imageObjArray[(int)E_RAWOBJ.VEC] = child.GetComponent<RawImage>();

		}

		imageObjArray[(int)E_RAWOBJ.ICON].color = playerDataObj.playerColorArray[int.Parse(targetPlayer.name) - 1];

	}


	// Update is called once per frame
	void LateUpdate () {
		// (ミニマップの大きさ / 2) * プレイヤーの座標 / (ステージの広さ / 2)
		pos.x = (200 / 2) * targetPlayer.transform.position.x / (500 / 2);
		pos.y = (200 / 2) * targetPlayer.transform.position.z / (500 / 2);

		this.transform.localPosition = pos;

		rot.z = targetPlayer.transform.rotation.y;	// プレイヤーのY軸回転をZ軸回転として受け取る
		rot.w = -targetPlayer.transform.rotation.w;	// 回転の実数値
		imageObjArray[(int)E_RAWOBJ.VEC].transform.rotation = rot;

	}
}
