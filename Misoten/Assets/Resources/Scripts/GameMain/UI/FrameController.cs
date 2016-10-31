using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FrameController : MonoBehaviour
{
	/// <summary>
	/// 各プレイヤー画面にアクセスする用
	/// </summary>
	private enum E_PLAYER_NUM
	{
		PLAYER_1 = 0,
		PLAYER_2,
		PLAYER_3,
		PLAYER_4,

		MAX
	};

	/// <summary>
	/// 枠の各辺にアクセスする用
	/// </summary>
	private enum E_LINE_NUM
	{
		FROM_LEFT_UP_TO_LEFT_DOWN = 0,	// 左上から左下
		FROM_LEFT_DOWN_TO_RIGHT_DOWN,	// 左下から右下
		FROM_RIGHT_DOWN_TO_RIGHT_UP,	// 右下から右上
		FROM_RIGHT_UP_TO_LEFT_UP,		// 右上から左上
		
		MAX
	}

	/// <summary>
	/// 枠の色を変える処理の分岐用
	/// </summary>
	private enum E_CHANGE_COLOR_FUNCTION
	{
		NONE,			// 無し
		ONETIME,		// 即座に変わる
		LEVOROTATION,	// 反時計回り
        FLASHING,       // 点灯
	
		MAX
	}

	E_CHANGE_COLOR_FUNCTION changeColorFunctionID = E_CHANGE_COLOR_FUNCTION.NONE;

	/// <summary>
	/// 色を変えるドットを指定する変数用
	/// </summary>
	private enum E_CHANGE_COLOR_NUM
	{
		PLAYER_NUM = 0,
		LINE_NUM,
		DOT_NUM,

		MAX
	}

    /// <summary>
    /// 点灯時の状態
    /// </summary>
    private enum E_FLASHING_COLOR_STATE
    {
        FLASH_ON = 0,
        FLASH_OFF
    }

    private const int SCREEN_WIDTH = 960;
	private const int SCREEN_HEIGHT = 540;

	/// <summary>
	/// 子要素の名前
	/// </summary>
	private string[] childName = new string[] { "1p", "2p", "3p", "4p" };

	/// <summary>
	/// 枠を構成するGameObjectを格納している多次元配列。
	/// [どのプレイヤー画面化][どの辺か][どのドットか]
	/// </summary>
	private RawImage[][][] frameDotJaggeArray;

	/// <summary>
	/// 枠のドットの大きさ
	/// </summary>
	[SerializeField, Header("ドットの大きさ"), Tooltip("1か2の倍数の値にすること")]
	private int dotSize = 1;

	/// <summary>
	/// 枠の太さ
	/// </summary>
	[SerializeField, Header("枠の太さ")]
	private int width = 10;

	/// <summary>
	/// とりあえずのテスト用
	/// </summary>
	private Color[] defaultColorArray = new Color[] {Color.red, Color.blue, Color.green, Color.yellow};

    /// <summary>
    /// 点滅の状態
    /// </summary>
    private int flashingState = (int)E_FLASHING_COLOR_STATE.FLASH_OFF;

    /// <summary>
    /// 点滅時の仮色(ON)
    /// </summary>
    private Color[] flashingColorArrayON = new Color[] { Color.white, Color.white, Color.white, Color.white };

    /// <summary>
    /// 点滅時の仮色(OFF)
    /// </summary>
    private Color[] flashingColorArrayOFF = new Color[] { Color.black, Color.black, Color.black, Color.black };

    /// <summary>
    /// 点滅間隔
    /// </summary>
    private float flashingInterval = 0.5f;

    /// <summary>
    /// 点滅間隔処理の一時記憶
    /// </summary>
    private float flashingIntervalWork = 0;

    /// <summary>
    /// 点滅時間
    /// </summary>
    private float flashingTime = 10f;

    /// <summary>
    /// 点滅間隔処理の一時記憶
    /// </summary>
    private float flashingTimeWork = 0;

    /// <summary>
    /// 変化後の色
    /// </summary>
    private Color changeColor;

	/// <summary>
	/// ドットの色が変わっていく速さ
	/// </summary>
	[SerializeField]
	private int changeColorSpeed = 60;

	/// <summary>
	/// 色を変えるドットの番号
	/// </summary>
	private int[] changeColorDotNumArray = new int[(int)E_CHANGE_COLOR_NUM.MAX] {0, 0, 0};

	/// <summary>
	/// 前フレーム時の色を変えるドットの番号
	/// </summary>
	private int[] changeColorDotNum_OldArray = new int[(int)E_CHANGE_COLOR_NUM.MAX] { 0, 0, 0 };

	/// <summary>
	/// 一時記憶用の作業用変数
	/// </summary>
	private int changeColorDotWork = 0;

	/// <summary>
	/// 各プレイヤー画面のドットの合計数
	/// </summary>
	private int playerDotNum_SUM = 0;

	void Awake()
	{
		GameObject prefabDot = (GameObject)Resources.Load("Prefabs/GameMain/UIs/WhiteDot");

		// 子のGameObjectを取得
		GameObject[] children = new GameObject[childName.Length];
		for (int i = 0; i < children.Length; i++)
		{
			foreach (Transform child in this.transform)
				if (child.name == childName[i])
					children[i] = child.gameObject;
		
		}

		// フレームドット配列の要素数初期化と各ドットのインスタンスの生成
		frameDotJaggeArray = new RawImage[(int)E_PLAYER_NUM.MAX][][];
		for (int playerNum = 0; playerNum < (int)E_PLAYER_NUM.MAX; playerNum++)
		{
			frameDotJaggeArray[playerNum] = new RawImage[(int)E_LINE_NUM.MAX][];

			for (int lineNum = 0; lineNum < (int)E_LINE_NUM.MAX; lineNum++)
			{
				// 縦列の場合
				if ((lineNum + 1) % 2 == 1) // 0割回避
					frameDotJaggeArray[playerNum][lineNum] = new RawImage[SCREEN_HEIGHT / dotSize];
				else// 横列の場合
					frameDotJaggeArray[playerNum][lineNum] = new RawImage[SCREEN_WIDTH / dotSize];

				for (int dotNum = 0; dotNum < frameDotJaggeArray[playerNum][lineNum].Length; dotNum++)
				{
					// 枠を構成するドットを生成
					frameDotJaggeArray[playerNum][lineNum][dotNum] = Instantiate(prefabDot).GetComponent<RawImage>();

					// ドットを各プレイヤー画面に対応している子のオブジェの子にしていく
					frameDotJaggeArray[playerNum][lineNum][dotNum].transform.parent = children[playerNum].transform;
					
					// 何故かスケールの初期値が(0, 0, 0)なので、(1.0, 1.0, 1.0)に初期化
					frameDotJaggeArray[playerNum][lineNum][dotNum].transform.localScale = Vector3.one;
					
					// エディタのヒエラルキー上で分かりやすいようにnameを付ける
					frameDotJaggeArray[playerNum][lineNum][dotNum].name = lineNum.ToString() + "_" + dotNum.ToString();

					// 所属する辺ごとに、位置と縦横の大きさを初期化していく
					switch (lineNum)
					{
						case (int)E_LINE_NUM.FROM_LEFT_UP_TO_LEFT_DOWN:
							// 画面中心の十字になる部分は太さを半分にする
							if (playerNum == (int)E_PLAYER_NUM.PLAYER_2 || playerNum == (int)E_PLAYER_NUM.PLAYER_4)
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(width / 2.0f, dotSize);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2((-SCREEN_WIDTH / 2.0f) + (width / 2.0f / 2.0f), ((SCREEN_HEIGHT / 2.0f) - (dotSize / 2)) - (dotNum * dotSize));

							}
							else
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(width, dotSize);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2((-SCREEN_WIDTH / 2.0f) + (width / 2.0f), ((SCREEN_HEIGHT / 2.0f) - (dotSize / 2)) - (dotNum * dotSize));
							
							}
							
							break;

						case (int)E_LINE_NUM.FROM_LEFT_DOWN_TO_RIGHT_DOWN:
							if (playerNum == (int)E_PLAYER_NUM.PLAYER_1 || playerNum == (int)E_PLAYER_NUM.PLAYER_2)
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(dotSize, width / 2.0f);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2(((-SCREEN_WIDTH / 2.0f) + (dotSize / 2)) + (dotSize * dotNum), ((-SCREEN_HEIGHT / 2.0f) + (width /2.0f / 2)));

							}
							else
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(dotSize, width);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2(((-SCREEN_WIDTH / 2.0f) + (dotSize / 2)) + (dotSize * dotNum), ((-SCREEN_HEIGHT / 2.0f) + (width / 2)));
							
							}

							break;

						case (int)E_LINE_NUM.FROM_RIGHT_DOWN_TO_RIGHT_UP:
							if (playerNum == (int)E_PLAYER_NUM.PLAYER_1 || playerNum == (int)E_PLAYER_NUM.PLAYER_3)
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(width /2.0f, dotSize);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2((SCREEN_WIDTH / 2.0f) - (width / 2.0f / 2.0f), ((-SCREEN_HEIGHT / 2.0f) + (dotSize / 2)) + (dotNum * dotSize));
							
							}
							else
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(width, dotSize);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2((SCREEN_WIDTH / 2.0f) - (width / 2.0f), ((-SCREEN_HEIGHT / 2.0f) + (dotSize / 2)) + (dotNum * dotSize));
							
							}

							break;

						case (int)E_LINE_NUM.FROM_RIGHT_UP_TO_LEFT_UP:
							if (playerNum == (int)E_PLAYER_NUM.PLAYER_3 || playerNum == (int)E_PLAYER_NUM.PLAYER_4)
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(dotSize, width / 2.0f);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2(((SCREEN_WIDTH / 2.0f) - (dotSize / 2)) - (dotSize * dotNum), ((SCREEN_HEIGHT / 2.0f) - (width / 2.0f / 2)));
							

							}
							else
							{
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.sizeDelta =
									new Vector2(dotSize, width);
								frameDotJaggeArray[playerNum][lineNum][dotNum].rectTransform.anchoredPosition =
									new Vector2(((SCREEN_WIDTH / 2.0f) - (dotSize / 2)) - (dotSize * dotNum), ((SCREEN_HEIGHT / 2.0f) - (width / 2)));
							
							}

							break;

					} // switch 各辺のドットの初期化

				} // for 各粒のインスタンス生成

			} // for 各辺の要素数初期化

		} // for プレイヤー数分の要素数初期化

	}

	// Use this for initialization
	void Start () {
		// ドットの合計数を記録しておく
		playerDotNum_SUM = frameDotJaggeArray[0][0].Length * 2 + frameDotJaggeArray[0][1].Length * 2;

		// とりあえず各プレイヤーの色にする
		for (int playerNum = 0; playerNum < frameDotJaggeArray.Length; playerNum++)
			for (int lineNum = 0; lineNum < frameDotJaggeArray[playerNum].Length; lineNum++)
				for (int dotNum = 0; dotNum < frameDotJaggeArray[playerNum][lineNum].Length; dotNum++)
				{
					frameDotJaggeArray[playerNum][lineNum][dotNum].color = defaultColorArray[playerNum];

				}
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (changeColorFunctionID)
		{
			case E_CHANGE_COLOR_FUNCTION.ONETIME:
				changeColorOneTime();
                
                break;

			case E_CHANGE_COLOR_FUNCTION.LEVOROTATION:
				ChangeColorLevoRotation();
                
                break;
                
            case E_CHANGE_COLOR_FUNCTION.FLASHING:
                ChangeColorFlashing();
                
                break;
		}

		// とりあえず色変えが発生している時は、変更を受け付けない
		if (changeColorFunctionID != E_CHANGE_COLOR_FUNCTION.NONE) return;

		if (Input.GetKeyDown(KeyCode.C))
		{
			SetChangeColorFunction(E_CHANGE_COLOR_FUNCTION.ONETIME, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f));

		}

		if (Input.GetKeyDown(KeyCode.R))
			SetChangeColorFunction(E_CHANGE_COLOR_FUNCTION.LEVOROTATION, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f));

        //枠点滅
        if (Input.GetKeyDown(KeyCode.F))
            SetChangeColorFunction(E_CHANGE_COLOR_FUNCTION.FLASHING, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f));
        
            

    }

	/// <summary>
	/// 色を変える関数の指定と初期化
	/// </summary>
	/// <param name="functionID">使う関数のID</param>
	/// <param name="afterColor">変化後の色</param>
	private void SetChangeColorFunction(E_CHANGE_COLOR_FUNCTION functionID, Color afterColor)
	{
		for (int i = 0; i < changeColorDotNumArray.Length; i++)
			changeColorDotNumArray[i] = 0;

		for (int i = 0; i < changeColorDotNum_OldArray.Length; i++)
			changeColorDotNum_OldArray[i] = 0;

		changeColorFunctionID = functionID;
		changeColor = afterColor;
	
	}

	/// <summary>
	/// 色を変える関数の終了処理
	/// </summary>
	private void EndChangeColorFunction()
	{
		changeColorFunctionID = E_CHANGE_COLOR_FUNCTION.NONE;
	
	}

	/// <summary>
	/// 即座にすべてのドットを同じ色にする
	/// </summary>
	private void changeColorOneTime()
	{
		for (int playerNum = 0; playerNum < frameDotJaggeArray.Length; playerNum++)
			for (int lineNum = 0; lineNum < frameDotJaggeArray[playerNum].Length; lineNum++)
				for (int dotNum = 0; dotNum < frameDotJaggeArray[playerNum][lineNum].Length; dotNum++)
					frameDotJaggeArray[playerNum][lineNum][dotNum].color = changeColor;

		EndChangeColorFunction();

	}

	/// <summary>
	/// 反時計回りに枠の色を変える
	/// </summary>
	private void ChangeColorLevoRotation()
	{
		// 前フレームのドット番号を記憶
		changeColorDotNumArray.CopyTo(changeColorDotNum_OldArray, 0);

		// 経過した時間分色を変えるドットの番号を進ませる
		changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.DOT_NUM] += (int)(changeColorSpeed * Time.deltaTime);

		// 現在指定しているラインのドット数を超えたら、ラインのドット数と同じにする
		if (changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.DOT_NUM] > frameDotJaggeArray[0][changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.LINE_NUM]].Length)
			changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.DOT_NUM] = frameDotJaggeArray[0][changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.LINE_NUM]].Length;

		// 各プレイヤー画面同時に変わる
		for (int playerNum = 0; playerNum < frameDotJaggeArray.Length; playerNum++)
		{
			// 前回のドットの番号から、進んだ分のドットの色変える
			for (int dotNum = changeColorDotNum_OldArray[(int)E_CHANGE_COLOR_NUM.DOT_NUM]; dotNum < changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.DOT_NUM]; dotNum++)
				frameDotJaggeArray[playerNum][changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.LINE_NUM]][dotNum].color = changeColor;
		
		}

		// 現在しているラインのドット数に達したら、ドットを0にして、ラインに1足す
		if (changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.DOT_NUM] >= frameDotJaggeArray[0][changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.LINE_NUM]].Length)
		{
			changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.DOT_NUM] = 0;
			changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.LINE_NUM]++;
		
		}

		// ドットの最大数を超えたら終了
		if (changeColorDotNumArray[(int)E_CHANGE_COLOR_NUM.LINE_NUM] < frameDotJaggeArray[0].Length) return;
		
		EndChangeColorFunction();
		
	
	}

    /// <summary>
    /// 全部の枠を二色で点滅させる
    /// </summary>
    private void ChangeColorFlashing()
    {
        flashingTimeWork = flashingTime;
        flashingIntervalWork = flashingInterval;
        Debug.Log("点滅間隔");

        //点滅時間
        do{

            //点灯間隔過ぎたら色変える
            flashingIntervalWork -= Time.deltaTime * 1;
            Debug.Log(flashingIntervalWork);

            //
            flashingTimeWork -= Time.deltaTime * 1;
            Debug.Log(flashingTimeWork);

            //点滅の間隔
            if (flashingIntervalWork <= 0)
            {
                
                //現在の点灯の状態
                switch (flashingState)
                {
                    case (int)E_FLASHING_COLOR_STATE.FLASH_ON:

                        //プレイヤーの色全て点灯ON時の色に変更
                        for (int playerNum = 0; playerNum < frameDotJaggeArray.Length; playerNum++)
                            for (int lineNum = 0; lineNum < frameDotJaggeArray[playerNum].Length; lineNum++)
                                for (int dotNum = 0; dotNum < frameDotJaggeArray[playerNum][lineNum].Length; dotNum++)
                                {
                                    frameDotJaggeArray[playerNum][lineNum][dotNum].color = flashingColorArrayOFF[playerNum];

                                }
                        flashingState = (int)E_FLASHING_COLOR_STATE.FLASH_ON;
                    
                        break;
                    case (int)E_FLASHING_COLOR_STATE.FLASH_OFF:


                        //プレイヤーの色全て点灯OFF時の色に変更
                        for (int playerNum = 0; playerNum < frameDotJaggeArray.Length; playerNum++)
                            for (int lineNum = 0; lineNum < frameDotJaggeArray[playerNum].Length; lineNum++)
                                for (int dotNum = 0; dotNum < frameDotJaggeArray[playerNum][lineNum].Length; dotNum++)
                                {
                                    frameDotJaggeArray[playerNum][lineNum][dotNum].color = flashingColorArrayON[playerNum];

                                }

                        flashingState = (int)E_FLASHING_COLOR_STATE.FLASH_OFF;
                    
                        break;
                }
                EndChangeColorFunction();
                flashingIntervalWork = flashingInterval;
            }
        } while (flashingTimeWork > 0);

    }

}
