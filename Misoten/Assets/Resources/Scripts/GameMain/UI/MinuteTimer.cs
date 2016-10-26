using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class MinuteTimer : MonoBehaviour
{
	enum E_TIME
	{
		MINUTE = 0,
		SEC_2nd,
		SEC_1st,

		MAX
	}

	E_TIME eTimeFigure;

	// 残り時間を持っているGameMainSceneControllerオブジェの入れ子
	private GameMainSceneController sceneObj;

	// 残り時間表示用
	private int minute;					// 分の値
	private int[] sec =  new int[2];	// 秒の値　要素数が小さい方が桁が小さい

	private RawImage[] rawImageObjArray;

	private Rect UVRect;

	// Use this for initialization
	void Start () {

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
	void Update () {

		// 分を算出
		minute = (int)sceneObj.NowTime / 60;

		// 秒の各桁を算出　
		for (int i = 0; i < sec.Length; i++)
			sec[i] = ((int)sceneObj.NowTime % 60) / (int)Mathf.Pow(10, i) % 10;

		UVCalculation(minute);
		rawImageObjArray[(int)E_TIME.MINUTE].uvRect = UVRect;

		UVCalculation(sec[1]);
		rawImageObjArray[(int)E_TIME.SEC_2nd].uvRect = UVRect;

		UVCalculation(sec[0]);
		rawImageObjArray[(int)E_TIME.SEC_1st].uvRect = UVRect;


		/*
		for (int i = 0; i < comma.Length; i++)
		{
			comma[i] = (int)(sceneObj.NowTime * Mathf.Pow(10, comma.Length - i) % 10);

		}
		 */

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
