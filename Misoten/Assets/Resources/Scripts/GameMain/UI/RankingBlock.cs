using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankingBlock : MonoBehaviour {

    /// <summary>
    /// 順位移動の種類
    /// </summary>
    private enum E_KIND
    {
        RANK_UP = 0,
        RANK_DOWN,
        STAY
    }

    /// <summary>
    /// ランクダウンの処理状態
    /// </summary>
    private enum E_RANK_DOWN
    {
        START_STAY =0,
        DOWN,
        END_STAY
    }

    /// <summary>
    /// ランクアップの処理状態
    /// </summary>
    private enum E_RANK_UP
    {
        LEFT=0,
        UP,
        RIGHT
    }

    /// <summary>
    /// 順位の処理状態用
    /// </summary>
    E_KIND eKindStatus = E_KIND.STAY;


    /// <summary>
    /// ランクアップの状態用
    /// </summary>
    E_RANK_UP eRankUpStatus = E_RANK_UP.RIGHT;

    /// <summary>
    /// ランクダウンの状態用
    /// </summary>
    E_RANK_DOWN eRankDownStatus = E_RANK_DOWN.START_STAY;

	/// <summary>
	/// 順位を見に行くプレイヤーオブジェ。
	/// インスペクターで指定する。
	/// </summary>
	[SerializeField]
	private PlayerControl targetPlayer;

	/// <summary>
	/// 操作するオブジェ。
	/// この変数で位置情報を変更する
	/// </summary>
	private RectTransform myRectTransform;

    /// <summary>
    /// 変化後の順位
    /// </summary>
    [SerializeField]
    private int newRanking = 1;

    /// <summary>
    /// 変化前の順位
    /// </summary>
    [SerializeField]
    private int oldRanking = 0;

    /// <summary>
    /// 未処理の順位処理
    /// </summary>
    private Queue<int> rankQueue = new Queue<int>();

	/// <summary>
	/// 初期位置
	/// </summary>
	private Vector2 defPos;

	/// <summary>
	/// 動かす値
	/// </summary>
	private Vector2 newMovePos = Vector2.zero;

    /// <summary>
	/// 古い動かす値
	/// </summary>
	private Vector2 oldMovePos = Vector2.zero;


    /// <summary>
    /// 移動スピード（X軸）
    /// </summary>
    [SerializeField]
	private float moveSpeedX;

    /// <summary>
	/// 移動スピード（Y軸）
	/// </summary>
	[SerializeField]
    private float moveSpeedY;

    /// <summary>
    /// 1アクションごとの移動にかける時間
    /// </summary>>
    [SerializeField]
    private float moveTime = 1.0f;

    /// <summary>
    /// 経過時間の作業用
    /// </summary>
    [SerializeField]
    private float elapsedTimeWork;

	/// <summary>
	/// ランクアップ処理用(true: 処理中 / false: 処理していない)
	/// </summary>
	private bool RankFuncFlag = false;



    //test
    [SerializeField]
    private int rankBuff;

	void Awake()
	{
		myRectTransform = GetComponent<RectTransform>();
		defPos = myRectTransform.anchoredPosition;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.A))
            RankFuncFlag = true;

		if (RankFuncFlag)
        {
            Demo();
        }
        else
        {
            //ランクが動いたかどうか
            bool rankMoveFlg = false;

            //キューから次のタスクを取りだし
            //Debug.Log(rankQueue.Count);
            if(rankQueue.Count > 0) { 
                newRanking = rankQueue.Dequeue();
            }

            //順位が変わったかどうか
            if (newRanking != oldRanking)
            {
                rankMoveFlg = true;
            }

            //タスクがあったとき
            if (rankMoveFlg == true)
            {

                // ランクアップ
                if (oldRanking > newRanking)
                {
                    
                    eKindStatus = E_KIND.RANK_UP;

                    // ワンアクションにかける時間から横移動スピードを決定
                    moveSpeedX = myRectTransform.sizeDelta.x / moveTime;

                    // ワンアクションにかける時間から縦移動スピードを決定
                    moveSpeedY = (myRectTransform.sizeDelta.y * (oldRanking - newRanking)) / moveTime;

                   
                    elapsedTimeWork = moveTime * 3;

                }
                else//ランクダウン
                {
                    
                    eKindStatus = E_KIND.RANK_DOWN;

                    // ワンアクションにかける時間から縦移動スピードを決定
                    moveSpeedY = (myRectTransform.sizeDelta.y * (oldRanking - newRanking)) / moveTime;

                    elapsedTimeWork = moveTime * 3;

                }

                
                RankFuncFlag = true;
            }
        }
			
	
	}

	/// <summary>
	/// デモ
	/// </summary>
	private void Demo()
	{
        
        switch (eKindStatus)
        {
            //ランクアップの処理
            case E_KIND.RANK_UP:
                elapsedTimeWork -= Time.deltaTime * 1;
                switch (eRankUpStatus)
                {
                    //右に移動
                    case E_RANK_UP.RIGHT:

                        newMovePos.x += moveSpeedX * Time.deltaTime * 1;

                        //moveTime経過したら
                        if (elapsedTimeWork <= moveTime * 2)
                        {
                            eRankUpStatus = E_RANK_UP.UP;
                            newMovePos.x =  myRectTransform.sizeDelta.x;
                        }
                            
                        break;

                    //上に移動
                    case E_RANK_UP.UP:

                        newMovePos.y += moveSpeedY * Time.deltaTime * 1;

                        //moveTime経過したら
                        if (elapsedTimeWork <= moveTime ){
                            eRankUpStatus = E_RANK_UP.LEFT;
                            newMovePos.y = oldMovePos.y + (myRectTransform.sizeDelta.y * (oldRanking - newRanking));
                        }
                        
                        
                        break;

                    //左に移動
                    case E_RANK_UP.LEFT:
                        newMovePos.x -= moveSpeedX * Time.deltaTime * 1;

                        //moveTime経過したら
                        if (elapsedTimeWork <= 0)
                        {
                            eRankUpStatus = E_RANK_UP.RIGHT;
                            newMovePos.x = 0;
                            oldRanking = newRanking;
                            oldMovePos = newMovePos;
                            RankFuncFlag = false;
                        }

                        break;
                }
                
                break;
            
            //ランクダウン処理
            case E_KIND.RANK_DOWN:
                elapsedTimeWork -= Time.deltaTime * 1;

                switch (eRankDownStatus)
                {
                    //ランクアップの処理待ち
                    case E_RANK_DOWN.START_STAY:
                        
                        if (elapsedTimeWork < moveTime * 2)
                        {
                            eRankDownStatus = E_RANK_DOWN.DOWN;
                        }
        
                        
                        break;
                    //下方向に移動
                    case E_RANK_DOWN.DOWN:


                        newMovePos.y += moveSpeedY * Time.deltaTime * 1;

                        if (elapsedTimeWork < moveTime)
                        {
                            eRankDownStatus = E_RANK_DOWN.END_STAY;
                            newMovePos.y = oldMovePos.y + (myRectTransform.sizeDelta.y * (oldRanking - newRanking));
                        }

                        break;

                    //ランクアップの処理待ち
                    case E_RANK_DOWN.END_STAY:
                        
                        if (elapsedTimeWork < 0)
                        {
                            eRankDownStatus = E_RANK_DOWN.START_STAY;
                            oldRanking = newRanking;
                            oldMovePos = newMovePos;
                            RankFuncFlag = false;
                        }
                        
                        break;
                }
                break;
        }

     
        this.myRectTransform.anchoredPosition = defPos + newMovePos;

	}

	/// <summary>
	/// 順位の更新
	/// </summary>
	/// <param name="ranking">変化後の順位</param>
	public void UpdateRanking(int ranking)
	{
        //キューに追加
        rankQueue.Enqueue(ranking);

    }

}
