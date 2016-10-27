using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextScore : MonoBehaviour {

	/// <summary>
	/// 数字を表しているRawImageオブジェの各桁のアクセス用の列挙型
	/// </summary>
	enum E_ACCESS_NEXT_SCORE_VAL
	{
		TEN = 0,	// 10の位
		UNIT,		// 1の位

		MAX
	}

	/// <summary>
	/// 数字を表示している子要素のRawImageオブジェを格納している配列
	/// </summary>
	private RawImage[] timeObjArray;

	/// <summary>
	/// UV操作用のRectオブジェ
	/// </summary>
	private Rect UVRect;
	private Rect UVRectOffset;

	void Awake()
	{
		timeObjArray = new RawImage[this.transform.childCount];

		// 子要素のRawImageオブジェを取得
		for (int i = 0; i < this.transform.childCount; i++)
			foreach (Transform child in this.transform)
			{
				if(int.Parse(child.name) == i)
					timeObjArray[i] = child.GetComponent<RawImage>();

			}

		// UVの初期値を入れる
		UVRect = UVRectOffset = timeObjArray[0].uvRect;

		UpdateNextScore(0);
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// 次に必要なスコアを再計算する
	/// 8で折り返す
	/// </summary>
	/// <param name="necessity">次の順位との差</param>
	public void UpdateNextScore(int necessity)
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

}
