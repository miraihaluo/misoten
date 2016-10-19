using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class ResultSystem : MonoBehaviour
{

    //プレイヤーデータを保持しているアセットを取得
    public PlayerData playerData;

    [SerializeField]
    private int PlayerNum;
    [SerializeField]
    private int[] PlayerRank;
    [SerializeField]
    private int[] PlayerScore;

    public int[] ScoreTest;
    public int[] RankTest;

    // Use this for initialization
    void Awake()
    {
        playerData = Resources.Load<PlayerData>("Assets/PlayerData");

    }

    void Start() {

        //初期化

        PlayerNum   = playerData.GetPlayerNum();
        PlayerRank  = new int[playerData.GetPlayerNum()];
        PlayerScore = new int[playerData.GetPlayerNum()];
        playerData.GetPlayerScoreArray().CopyTo( PlayerScore , 0);

        //ランク付け
        RankChecker();

    }

    // Update is called once per frame
    void Update()
    {

        PlayerRank.CopyTo(RankTest, 0);
        ScoreTest.CopyTo(PlayerScore, 0);
        RankChecker();

       
    }


    void RankChecker() {

        //プレイヤーnoの初期値
        for (int i = 0; PlayerNum > i; i++)
        {
            PlayerRank[i] = i + 1;
        }

        //順位づけ
        for (int i = 0; PlayerNum - 1 > i; i++)
        {
            for (int t = 0; PlayerNum - i - 1 > t; t++)
            {
                if (PlayerScore[t + 1] < PlayerScore[t])
                {
                    int dummy;
                    dummy = PlayerScore[t + 1];
                    PlayerScore[t + 1] = PlayerScore[t];
                    PlayerScore[t] = dummy;

                    dummy = PlayerRank[t + 1];
                    PlayerRank[t + 1] = PlayerRank[t];
                    PlayerRank[t] = dummy;

                }
            }
        }
        

        //入れ替え
        int[] DummyRank = new int[PlayerNum];
        PlayerRank.CopyTo(DummyRank,0);

        for (int i = PlayerNum-1; 0 <  i; i--) {
            PlayerRank[DummyRank[i]-1] = i%4;
        }

          /*
        //同順処理
        int rank = 1 ;
        for (int i = 1; PlayerNum > i; i++)
        {
            for (int t = 0; PlayerNum - i > t ; t++)
            {
                if (PlayerScore[PlayerNum - i] == PlayerScore[t])
                {
                    PlayerRank[DummyRank[t] - 1] = PlayerRank[PlayerNum - i];
                }
            }

          
        }       */
                  
            return;
    }

}
