using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChildrenVal : MonoBehaviour {

	/// <summary>
	/// 各桁のRawImageオブジェにアクセスするためのenum
	/// </summary>
	private enum E_KETA
	{
		_1,
		_10,

		MAX
	};

	/// <summary>
	/// 持っている子供が1桁なのか2桁なのかで表示を変える
	/// </summary>
	private enum E_MODE_STATUS
	{
		_1,
		_10,

		MAX
	}

	private E_MODE_STATUS eMode = E_MODE_STATUS._1;

	/// <summary>
	/// 監視するプレイヤー
	/// </summary>
	[SerializeField]
	private PlayerControl targetPlayer;

	/// <summary>
	/// 各桁のRawImageオブジェ
	/// </summary>
	private RawImage[] rawImageObjArray = new RawImage[(int)E_KETA.MAX];

	/// <summary>
	/// UV
	/// </summary>
	private Rect UVRect;

	/// <summary>
	/// 持ている子供が10以上の時の1桁目の数字の位置
	/// </summary>
	private Vector3 pos10 = Vector3.zero;

	/// <summary>
	/// 今持っている子供の数
	/// </summary>
	private int childVal = 0;

	void Awake()
	{
		rawImageObjArray[(int)E_KETA._1] = this.transform.FindChild("1").GetComponent<RawImage>();
		rawImageObjArray[(int)E_KETA._10] = this.transform.FindChild("10").GetComponent<RawImage>();

	}

	// Use this for initialization
	void Start () {
		pos10.x = rawImageObjArray[(int)E_KETA._1].rectTransform.rect.width / 2;
		ChangeMode(E_MODE_STATUS._1);
		UVRect = rawImageObjArray[0].uvRect;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate()
	{
		if (childVal == targetPlayer.Score) return;

		childVal = targetPlayer.Score;

		UVCalculation(childVal % 10);
		rawImageObjArray[(int)E_KETA._1].uvRect = UVRect;

		UVCalculation(childVal / 10);
		rawImageObjArray[(int)E_KETA._10].uvRect = UVRect;

		if (childVal < 10)
			ChangeMode(E_MODE_STATUS._1);
		else
			ChangeMode(E_MODE_STATUS._10);

	}

	/// <summary>
	/// 値からUV値を算出する。8で折り返す
	/// </summary>
	/// <param name="val">値</param>
	private void UVCalculation(int val)
	{
		// 8で折り返す
		if (val >= 8)
			UVRect.y = UVRect.height;
		else
			UVRect.y = UVRect.height * 2;

		val = val % 8;

		UVRect.x = val * UVRect.width;

	}

	void ChangeMode(E_MODE_STATUS changeMode)
	{
		switch (changeMode)
		{
			case E_MODE_STATUS._1:
				rawImageObjArray[(int)E_KETA._1].rectTransform.localPosition = Vector3.zero;
				rawImageObjArray[(int)E_KETA._10].gameObject.SetActive(false);
				eMode = E_MODE_STATUS._1;

				break;
		
			case E_MODE_STATUS._10:
				rawImageObjArray[(int)E_KETA._1].rectTransform.localPosition = pos10;
				rawImageObjArray[(int)E_KETA._10].gameObject.SetActive(true);
				eMode = E_MODE_STATUS._10;

				break;
		
		}

	}

}
