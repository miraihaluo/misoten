using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextScore : MonoBehaviour {

	/// <summary>
	/// 数字を表しているRawImageオブジェの各桁のアクセス用の列挙型
	/// </summary>
	enum E_KETA
	{
//		TEN = 0,	// 10の位
//		UNIT,		// 1の位

		_1 = 0,
		_10,
		_100,

		MAX
	}

	E_KETA eMode = E_KETA._1;

	/// <summary>
	/// 各桁のRawImageオブジェ
	/// </summary>
	private RawImage[] rawImageObjArray = new RawImage[(int)E_KETA.MAX];

	/// <summary>
	/// UV操作用のRectオブジェ
	/// </summary>
	private Rect UVRect;

	private Vector3[,] KETApos = new Vector3[3, 3];

	void Awake()
	{
		rawImageObjArray[(int)E_KETA._1] = this.transform.FindChild("1").GetComponent<RawImage>();
		rawImageObjArray[(int)E_KETA._10] = this.transform.FindChild("10").GetComponent<RawImage>();
		rawImageObjArray[(int)E_KETA._100] = this.transform.FindChild("100").GetComponent<RawImage>();

	}

	// Use this for initialization
	void Start () {
		// 各桁毎の各数字の座標を算出
		for (int i = 0; i < (int)E_KETA.MAX; i++)
		{
			for (int j = 0; j <= i; j++)
			{
				KETApos[i, j] = Vector3.right *	// X座標を原点から
								( (rawImageObjArray[j].rectTransform.rect.width / 2) +	// 横幅の半分と
								(rawImageObjArray[j].rectTransform.rect.width * (i - j)) - // 前に居るオブジェクト分右に移動
								rawImageObjArray[j].rectTransform.rect.width );// 無理やり左に一つ分ずらす
			
			}
		
		}

		UVRect = rawImageObjArray[0].uvRect;

		ChangeMode(eMode);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// ランキングスコアの更新
	/// </summary>
	/// <param name="score">現在の合計スコア</param>
	public void UpdateNextScore(int score)
	{
		if (score >= Mathf.Pow(10, ((int)eMode + 1)))
		{
			ChangeMode(eMode + 1);
		
		}

		for (int i = 0; i < (int)E_KETA.MAX; i++)
		{
			UVCalculation((int)(score / Mathf.Pow(10, i)));
			rawImageObjArray[i].uvRect = UVRect;

		}
	
	}

	void ChangeMode(E_KETA changeMode)
	{
		eMode = changeMode;

		for (int i = 0; i < (int)E_KETA.MAX; i++)
		{
			if (i > (int)changeMode)
			{
				rawImageObjArray[i].gameObject.SetActive(false);
				continue;
			
			}

			rawImageObjArray[i].gameObject.SetActive(true);
			rawImageObjArray[i].rectTransform.localPosition = KETApos[(int)changeMode, i];

		}

	}

	/// <summary>
	/// 値からUV値を算出する。8で折り返す
	/// </summary>
	/// <param name="val">値</param>
	private void UVCalculation(int val)
	{
		val = val % 10;

		// 8で折り返す
		if (val >= 8)
			UVRect.y = UVRect.height;
		else
			UVRect.y = UVRect.height * 2;

		val = val % 8;

		UVRect.x = val * UVRect.width;

	}

	/// <summary>
	/// 次に必要なスコアを再計算する
	/// 8で折り返す
	/// </summary>
	/// <param name="necessity">次の順位との差</param>
/*	public void UpdateNextScore(int necessity)
	{
		// 1桁目
		UVRect.x = ((necessity % 10) % 8) * UVRect.width + UVRectOffset.x;
		UVRect.y = ((necessity % 10) / 8) * -UVRect.height + UVRectOffset.y;
		timeObjArray[(int)E_ACCESS_NEXT_SCORE_VAL.UNIT].uvRect = UVRect;
		
		// 2桁目
		necessity /= 10;
		UVRect.x = (necessity % 8) * UVRect.width + UVRectOffset.x;
		UVRect.y = (necessity / 8) * -UVRect.height + UVRectOffset.y;
		timeObjArray[(int)E_ACCESS_NEXT_SCORE_VAL.TEN].uvRect = UVRect;
	
	}
*/

}
