using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class IndicationInputKey : MonoBehaviour
{

	// チェックするヴァーチャルボタン名群
	// ヴァーチャルボタンの設定は、Edit -> Project Settings -> Input
	private string[] checkButtonNameArray = {
                "Horizontal", "Vertical",							// 縦横移動系　アナログOK
                "Fire1",     "Fire2",	"Fire3",	"Jump",			// ボタン入力
                "Mouse X",   "Mouse Y",	"Mouse ScrollWheel",		// マウス入力　アナログOK
                "Submit",    "Cancel",	"Quit",						// 決定他入力　Quitはエスケープキーでの終了用
                null};     // 終端文字

    // 押されているボタンのトリガー、ホールドを識別するためのbool配列
    private bool[] buttonHoldeFlagArray = null;

	// UI表示用Textオブジェクト
	[SerializeField]
	private Text TextObj;

	// Textに表示する用の文字列配列
	private string[] checkButtonStateArray;


	// Use this for initialization
	void Start () {

		// bool配列の要素数をチェックするヴァーチャルボタン群と同じ数、確保する。
		buttonHoldeFlagArray = new bool[checkButtonNameArray.Length];

		// bool配列の初期化
		for (int i = 0; i < checkButtonNameArray.Length; i++)
			buttonHoldeFlagArray[i] = false;

		// Text表示用の文字列配列の要素数をチェックするヴァーチャルボタン群と同じ数、確保する。
		checkButtonStateArray = new string[checkButtonNameArray.Length];

	}
	
	// Update is called once per frame
	void Update () {

		// 配列がNULLなら抜ける
		if (checkButtonNameArray == null)	return;
		if (buttonHoldeFlagArray == null)	return;

		string axisSignString = null;

		// Text表示用の文字列配列の初期化
		for (int i = 0; i < checkButtonNameArray.Length; i++)
			checkButtonStateArray[i] = checkButtonNameArray[i] + "\t：";

		// Textの初期化
		TextObj.text = "";

        // ボタンが押されている間、押されたボタンのをログに表示する
        if(Input.anyKey)
        {
			// 各ボタンのトリガーとホールドのチェック
            for (int i = 0; checkButtonNameArray[i] != null; i++)
            {
				// ボタンが押されていたら
				if (Input.GetButton(checkButtonNameArray[i]))
				{
					// 押されたボタンが移動系のキーなら
					if (checkButtonNameArray[i] == "Horizontal" || checkButtonNameArray[i] == "Vertical")
					{
						// 0より大きいとプラス
						if (0 < Input.GetAxis(checkButtonNameArray[i]))
							axisSignString = "プラス";

						// 0より小さいとマイナス
						if (0 > Input.GetAxis(checkButtonNameArray[i]))
							axisSignString = "マイナス";

						// 0の時は同時押し
						if (0 == Input.GetAxis(checkButtonNameArray[i]))
							axisSignString = "同時押し";

					}

					// トリガーかホールドかの判別
					if (buttonHoldeFlagArray[i])
					{
						// trueならホールド
						// コンソール画面に出力
						//Debug.Log(checkButtonNameArray[i] + axisSignString + "ホールド");

						// Text表示用の文字列配列に、文字列を連結
						checkButtonStateArray[i] += axisSignString + "ホールド";

					}
					else
					{
						// falseならトリガー
						//Debug.Log(checkButtonNameArray[i] + axisSignString + "トリガー");

						// Text表示用の文字列配列に、文字列を連結
						checkButtonStateArray[i] += axisSignString + "トリガー";

						// buttonHoldFlagをtrueにしておく
						buttonHoldeFlagArray[i] = true;

					}

					axisSignString = null;

				}

			} // 各ボタンのトリガーとホールドのチェック

		} // ボタンが押されている間、押されたボタンのをログに表示する

		// 各ボタンのリリースのチェック
		for (int i = 0; checkButtonNameArray[i] != null; i++)
		{
			// ボタンが離されたら
			if (Input.GetButtonUp(checkButtonNameArray[i]))
			{
				// コンソール画面に出力
				//Debug.Log(checkButtonNameArray[i] + "リリース");

				// Text表示用の文字列配列に、文字列を連結
				checkButtonStateArray[i] += axisSignString + "リリース";

				// buttonHoldFlagをfalseにする
				buttonHoldeFlagArray[i] = false;

			}

		} // 各ボタンのリリースのチェック

		// Textで文字を出力
		for (int i = 0; i < checkButtonStateArray.Length; i++)
			TextObj.text += checkButtonStateArray[i] + "\n";

	} // Update

}
