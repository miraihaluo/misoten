using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class MissionController : MonoBehaviour
{

	/// <summary>
	/// ミッションタイプ
	/// </summary>
	public enum E_MISSION_TYPE
	{
		SPECIAL_CHILDREN,
		ENEMY_DRONE,
		FEVER,
	
		MAX
	};

	private string[] missionText = {
								"五つ子が現れた！",
								"余所のドローンが現れた！",
								"残り30秒！"
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

	/// <summary>
	/// テキスト背景の流れる速さ
	/// </summary>
	[SerializeField, Header("テキスト背景の流れる早さ")]
	private float BGSpeed = 100;

    enum eTEXT_STATE{
        DELETE,
        MOVE,
        REMOVE,
        STOP
    };
    private eTEXT_STATE onMission = eTEXT_STATE.DELETE ;           //テキストの状態
    private float LimitTime = 0;                                   //テキストの寿命 

    private Vector2 textSize;       //テキストの大きさ

	private Vector3 defTextPos;
	private Vector3 defBGPos;
	private Vector3 BGPos;

    // Use this for initialization
    void Start()
	{
		textSize.x = textObj.preferredWidth;
		defTextPos = textObj.transform.localPosition;
		defTextPos.x = textSize.x + Screen.width;
		textObj.transform.localPosition = defTextPos;

		defBGPos = BGObj.transform.localPosition;
		defBGPos.x = BGObj.rectTransform.rect.width;
		BGObj.transform.localPosition = -defBGPos;

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
        if(onMission == eTEXT_STATE.STOP)
        {
            LimitTime -= Time.deltaTime * 1;
            if(LimitTime <= 0)
            {
                onMission = eTEXT_STATE.REMOVE;
            }
        }

		// テキスト背景の処理
		switch (onMission)
		{
			case eTEXT_STATE.MOVE:
				if (BGPos.x <= 0) break;
				BGPos.x -= BGSpeed * Time.deltaTime;
				if (BGPos.x <= 0) BGPos.x = 0;
				BGObj.transform.localPosition = BGPos;

				break;

			case eTEXT_STATE.DELETE:
				if (BGPos.x <= -defBGPos.x) break;
				BGPos.x -= BGSpeed * Time.deltaTime;
				BGObj.transform.localPosition = BGPos;

				break;

		}

    }

	public void CallMissionText(E_MISSION_TYPE callMissionType)
	{
		onMission = eTEXT_STATE.MOVE;
		textObj.text = missionText[(int)callMissionType];

		frameObj.SetFrameStatus(callMissionType);

		//テキストサイズを計算
		textSize.x = textObj.preferredWidth;
		defTextPos.x = textSize.x + Screen.width;

		textObj.transform.localPosition = defTextPos;
		BGObj.transform.localPosition = BGPos = defBGPos;
	
	}

}
