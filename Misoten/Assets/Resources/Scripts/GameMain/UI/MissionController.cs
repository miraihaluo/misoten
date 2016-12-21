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
								"五つ子が現れた！ひとつで五人分だ！",
								"余所のドローンが現れた！子供達を取り戻そう！",
								"さあ、最後の追い上げだ！"
								};

    // 残り時間を持っているGameMainSceneControllerオブジェの入れ子
    private GameMainSceneController sceneObj;

    [SerializeField]
    private Text textObj;
    [SerializeField]
    private RawImage BGObj;

    [SerializeField]
    private float textSpeed;  //テキストの流れるスピード

    enum eTEXT_STATE{
        DELETE,
        MOVE,
        REMOVE,
        STOP
    };
    private eTEXT_STATE onMission = eTEXT_STATE.DELETE ;           //テキストの状態
    private float LimitTime = 0;                                   //テキストの寿命 

    private Vector2 textSize;       //テキストの大きさ

    // Use this for initialization
    void Start()
    {

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
    }

	public void CallMissionText(E_MISSION_TYPE callMissionType)
	{
		onMission = eTEXT_STATE.MOVE;
		textObj.text = missionText[(int)callMissionType];
		//テキストサイズを計算
		textSize.x = textObj.preferredWidth;

		textObj.transform.localPosition = Vector3.right * (textSize.x + Screen.width);
	
	}

}
