using UnityEngine;
using System.Collections;
using UnityEngine.UI;		// Textをいじるのに必要

public class ResultSystem : MonoBehaviour
{

    //プレイヤーデータを保持しているアセットを取得
    public PlayerData playerData;

    private int PlayerNum;
    public int[] PlayerRank;
    private int[] PlayerScore;
    private int[] DummyRank;
    private int[] DummyScore;
    
    //データ入力テスト用
    public int[] ScoreTest;

    public int[] GetRank(){
        return PlayerRank;
    }
    public int[] GetScore()
    {
        return PlayerScore;
    }

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
        DummyScore = new int[playerData.GetPlayerNum()];
        playerData.GetPlayerScoreArray().CopyTo( PlayerScore , 0);
        PlayerScore.CopyTo(DummyScore,0);

        DummyRank = new int[PlayerNum];


        ScoreTest.CopyTo(PlayerScore, 0);//スコアテスト用
        //ランク付け
        RankChecker();

        DummyScore.CopyTo(PlayerScore,0);

        ScoreTest.CopyTo(PlayerScore, 0);////スコアテスト用

    }

    // Update is called once per frame
    void Update()
    {
  
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
                if (PlayerScore[t + 1] > PlayerScore[t])
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
              
        
        PlayerRank.CopyTo(DummyRank,0);
          
        //同順処理
        int rank = 2;
        PlayerRank[DummyRank[0]-1] = 1;
        for (int i = 1; PlayerNum > i; i++)
        {
            if (PlayerScore[i] != PlayerScore[i - 1])
            {
                PlayerRank[DummyRank[i]-1] = rank;
            }
            else
            {
                PlayerRank[DummyRank[i]-1] = PlayerRank[DummyRank[i - 1]-1];
            }
            rank++;
        } 
        return;
    }

}
