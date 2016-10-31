using UnityEngine;
using System.Collections;

//
public class PlayerPreparationSystem : MonoBehaviour {

    //準備プレイヤーのオブジェクト
    public GameObject[] players = new GameObject[4];

    //現在の参加人数のデータ受け取り用
    private PlayerData playerData;

    //現在の参加人数
    [SerializeField]
    private int NowPlayerNum;

    // Use this for initialization
    void Awake()
    {

        //プレイヤーの参加情報を取ってくる
        playerData = Resources.Load<PlayerData>("Assets/PlayerData");

        

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー人数取得
        NowPlayerNum = playerData.GetPlayerNum();
        StartToActivePlayers(NowPlayerNum);

    }

    //参加したプレイヤーをアクティブな状態に
    void StartToActivePlayers(int playerNum)
    {
        switch (playerNum)
        {
            case 0:
               
                //オブジェクトを非アクティブにせずに何か動きをつけるときは以下の関数を呼ぶ
                players[0].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                players[1].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                players[2].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                players[3].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);

                break;
            case 1:
             
                players[0].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[1].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                players[2].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                players[3].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                break;
            case 2:
              
                players[0].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[1].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[2].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                players[3].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                break;
            case 3:
               
                players[0].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[1].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[2].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[3].GetComponent<PreparationActivationFunc>().SetActivateFlg(false);
                break;
            case 4:
               
                players[0].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[1].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[2].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                players[3].GetComponent<PreparationActivationFunc>().SetActivateFlg(true);
                break;
            default:
                break;

        }

    }
}
