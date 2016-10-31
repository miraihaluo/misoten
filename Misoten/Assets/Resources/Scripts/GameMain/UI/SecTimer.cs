using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class SecTimer : MonoBehaviour
{
	enum E_TIME
	{
		SEC_2nd,
		SEC_1st,
		COMMA_1st,
		COMMA_2nd,

		MAX
	}

	E_TIME eTimeFigure;

	// 残り時間を持っているGameMainSceneControllerオブジェの入れ子
	private GameMainSceneController sceneObj;

	// 残り時間表示用
	private int[] sec = new int[2];	// 秒の値　要素数が小さい方が桁が小さい
	private int[] comma = new int[2];	// 小数点以下の値　要素数が小さい方が桁が小さい

	private RawImage[] rawImageObjArray;

	private Rect UVRect;

	// Use this for initialization
	void Start()
	{

	}

	void Awake()
	{
		// GameMainSceneControllerのゲームオブジェクトを取得
		sceneObj = GameObject.FindObjectOfType<GameMainSceneController>();

		// このスクリプトを付けているTextオブジェを取得
		RawImage[] childArray = GetComponentsInChildren<RawImage>();

		rawImageObjArray = new RawImage[childArray.Length];

		for (int i = 0; i < childArray.Length; i++)
		{
			foreach (RawImage rawImageObj in childArray)
			{
				if (int.Parse(rawImageObj.name) == i)
					rawImageObjArray[i] = rawImageObj;

			}

		}

		UVRect = rawImageObjArray[0].uvRect;

	}

	// Update is called once per frame
	void Update()
	{

		// 秒の各桁を算出　
		for (int i = 0; i < sec.Length; i++)
			sec[i] = ((int)sceneObj.NowTime % 60) / (int)Mathf.Pow(10, i) % 10;

		for (int i = 0; i < comma.Length; i++)
			comma[i] = (int)(sceneObj.NowTime * Mathf.Pow(10, comma.Length - i) % 10);

		UVCalculation(sec[1]);
		rawImageObjArray[(int)E_TIME.SEC_2nd].uvRect = UVRect;

		UVCalculation(sec[0]);
		rawImageObjArray[(int)E_TIME.SEC_1st].uvRect = UVRect;

		UVCalculation(comma[0]);
		rawImageObjArray[(int)E_TIME.COMMA_1st].uvRect = UVRect;

		UVCalculation(comma[1]);
		rawImageObjArray[(int)E_TIME.COMMA_2nd].uvRect = UVRect;

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


}
