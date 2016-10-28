using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class MissionController : MonoBehaviour
{

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
    private Vector3 startPosition;                                 //テキストの待機位置
    private float LimitTime = 0;                                   //テキストの寿命 

    private string mission1 = "UFOを撃墜せよ！";
    private string mission2 = "野良猫の妨害を回避せよ！";
    private string mission3 = "エイリアンから子供たちを守れ！";
    private string mission4 = "太陽を破壊せよ！";
    private string mission5 = "一定時間スピード2倍！";

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
        
        //一定時間ごとにテキストを流す
        if(Mathf.CeilToInt(sceneObj.NowTime) % 20 == 0 & onMission == eTEXT_STATE.DELETE)
        {
            //ランダムでミッション分岐
            onMission = eTEXT_STATE.MOVE;
            switch (Random.Range(0, 5))
            {
                case 0:
                    textObj.text = mission1;
                    break;
                case 1:
                    textObj.text = mission2;
                    break;
                case 2:
                    textObj.text = mission3;
                    break;
                case 3:
                    textObj.text = mission4;
                    break;
                case 4:
                    textObj.text = mission5;
                    break;
            }
            //テキストサイズを計算
            textSize.x = textObj.preferredWidth;


            startPosition = new Vector3(textSize.x + Screen.width, 0, 0);
            textObj.transform.localPosition = startPosition;
        }

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
}
