using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class IndicationDeltaTime : MonoBehaviour
{
	// UIで表示する用のテキストオブジェクト
	[SerializeField]
	private Text TextObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// TextObjの初期化
		TextObj.text = "";

		// FPSを格納する変数
		float fps = 1.0f / Time.deltaTime;

		// コンソールにFPSを表示
		//Debug.Log(fps);

		// Textで文字列を出力
		TextObj.text += fps.ToString() + "\n";
	
	}
}
