using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class Timer : MonoBehaviour {

	// 残り時間を持っているGameMainSceneControllerオブジェの入れ子
	private GameMainSceneController sceneObj;

	// 残り時間表示用
	private int minute;					// 分の値
	private int[] sec =  new int[2];	// 秒の値　要素数が小さい方が桁が小さい
	private int[] comma = new int[2];	// 小数点以下の値　要素数が小さい方が桁が小さい

	[SerializeField]
	private Text textObj;

	// Use this for initialization
	void Start () {

	}

	void Awake()
	{
		// GameMainSceneControllerのゲームオブジェクトを取得
		sceneObj = GameObject.FindObjectOfType<GameMainSceneController>();

		// このスクリプトを付けているTextオブジェを取得
		textObj = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {

		// テキストの初期化
		textObj.text = "";

		// 分を算出
		minute = (int)sceneObj.NowTime / 60;

		// 秒の各桁を算出　
		for (int i = 0; i < sec.Length; i++)
			sec[i] = ((int)sceneObj.NowTime % 60) / (int)Mathf.Pow(10, i) % 10;

		if(minute > 0)
			textObj.text = minute.ToString() + "：";

		for (int i = sec.Length - 1; i >= 0; i--)
			textObj.text += sec[i].ToString();

		if (minute <= 0)
		{
			for (int i = 0; i < comma.Length; i++)
			{
				comma[i] = (int)(sceneObj.NowTime * Mathf.Pow(10, comma.Length - i) % 10);

			}

			textObj.text +=".";
		
			for(int i = comma.Length - 1; i >= 0; i--)
				textObj.text += comma[i];

		}

		if (sceneObj.NowTime < 0)
			textObj.text = "終　了";

	
	}
}
