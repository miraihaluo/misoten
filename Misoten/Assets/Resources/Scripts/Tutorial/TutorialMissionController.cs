using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class TutorialMissionController : MonoBehaviour
{

	/// <summary>
	/// ミッションタイプ
	/// </summary>
	public enum E_MISSION_TYPE
	{
		MAEOKI = 0,
		ZENSIN_KOUTAI,
		KAITEN,
		UP_DOWN,
		CHILD,
		GAOL,
		ATTACK,

		MAX
	};

	private string[] missionText = {
								"あそびかた",
								"左スティック上で前進、下で後退",
								"左スティック左右で旋回",
								"R1で上昇　L1で下降",
								"子供をどんどん集めよう",
								"ステージ中央上空のクジラに届けると得点獲得",
								"○ボタンの水鉄砲を当てて子供を奪えるぞ"
								};

	// 残り時間を持っているGameMainSceneControllerオブジェの入れ子
	private GameMainSceneController sceneObj;

	[SerializeField]
	private Text textObj;
	[SerializeField]
	private RawImage BGObj;

	/// <summary>
	/// 画面を区切るフレームのオブジェ
	/// </summary>
	[SerializeField, Header("画面を区切るフレームのオブジェ")]
	private Frame frameObj;

	[SerializeField]
	private float textSpeed;  //テキストの流れるスピード

	enum eTEXT_STATE
	{
		DELETE,
		MOVE,
		REMOVE,
		STOP
	};
	private eTEXT_STATE onMission = eTEXT_STATE.DELETE;           //テキストの状態
	private float LimitTime = 0;                                   //テキストの寿命 

	private Vector2 textSize;       //テキストの大きさ

	// Use this for initialization
	void Start()
	{
		textSize.x = textObj.preferredWidth;
		textObj.transform.localPosition = Vector3.right * (textSize.x + Screen.width);

	}

	void Awake()
	{
		// GameMainSceneControllerのゲームオブジェクトを取得
		sceneObj = GameObject.FindObjectOfType<GameMainSceneController>();
	}

	void Update()
	{
		//テキストを流す処理
		if (onMission == eTEXT_STATE.MOVE | onMission == eTEXT_STATE.REMOVE)
		{
			//テキストを流す
			textObj.transform.Translate(Time.deltaTime * textSpeed, 0, 0);

			//画面真ん中まで行くと一時停止
			if (textObj.transform.localPosition.x <= 0 & onMission == eTEXT_STATE.MOVE)
			{
				onMission = eTEXT_STATE.STOP;
				LimitTime = 3;
			}

			//画面端まで行くと停止
			if (textObj.transform.localPosition.x < -(textSize.x + Screen.width))
			{
				onMission = eTEXT_STATE.DELETE;
			}
		}

		//テキスト一時停止の処理
		if (onMission == eTEXT_STATE.STOP)
		{
			LimitTime -= Time.deltaTime * 1;
			if (LimitTime <= 0)
			{
				onMission = eTEXT_STATE.REMOVE;
			}
		}
	}

	public void CallMissionText(E_MISSION_TYPE callMissionType)
	{
		onMission = eTEXT_STATE.MOVE;
		textObj.text = missionText[(int)callMissionType];

//		frameObj.SetFrameStatus(callMissionType);

		//テキストサイズを計算
		textSize.x = textObj.preferredWidth;

		textObj.transform.localPosition = Vector3.right * (textSize.x + Screen.width);

	}

}
