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
    private float textSpeed = -100.0f;      //テキストが流れるスピード
    private bool onMission = false;         //テキストが流れているかどうか
    private Vector3 startPosition;          //テキストのスタート座標

    private string mission1 = "ミッション１だよ～";
    private string mission2 = "ミッション２だよ～";
    private string mission3 = "ミッション３だよ～";

    private float textSize;               //テキストのサイズ

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        // GameMainSceneControllerのゲームオブジェクトを取得
        sceneObj = GameObject.FindObjectOfType<GameMainSceneController>();

        //テキストをスタート位置に
        startPosition = new Vector3(1500, 0, 0);
        textObj.transform.position = Camera.main.WorldToScreenPoint(startPosition);

        //テキストサイズを取得
        textSize = textObj.preferredWidth*3;

    }
    
    void Update()
    {
        //20秒ごとにテキストを流す
        if(Mathf.CeilToInt(sceneObj.NowTime) % 20 == 0 & !onMission)
        {
            onMission = true;
            //ランダムで３パターン
            switch (Random.Range(0, 2))
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
            }
        }

        if (onMission)
        {
            //テキストを流す処理
            textObj.transform.Translate(Time.deltaTime * textSpeed, 0, 0);

            //画面端に行くとスタート位置に戻す
            if (textObj.transform.localPosition.x < -(textSize + Screen.width))
            {
                textObj.transform.localPosition = startPosition;
                onMission = false;
            }
        }

    }
}
