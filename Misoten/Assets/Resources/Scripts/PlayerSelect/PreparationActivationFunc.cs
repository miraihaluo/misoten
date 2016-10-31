using UnityEngine;
using System.Collections;

//オブジェクトの表示非表示
public class PreparationActivationFunc : MonoBehaviour {

    //参加済かどうか
    [SerializeField]
    private bool ActivateFlg;   //(true:参加/false:不参加)



    // Use this for initialization
    void Start () {
        ActivateFlg = false;
	}

    void Awake()
    {


    }
	
	// Update is called once per frame
	void Update () {

        //参加してる時
	    if(ActivateFlg == true)
        {
            //メッセージ1に変える(参加済)
            this.GetComponent<ChangeMessageFunc>().ChangeMessageText(1);

        }
        //参加してない時
        else
        {
            //メッセージ0に変える(未参加)
            this.GetComponent<ChangeMessageFunc>().ChangeMessageText(0);
        }
	}

    //現在の参加不参加状態を取得
    bool GetActivateFlg()
    {
        return ActivateFlg;
    }

    //参加不参加の状態を設定
    public void SetActivateFlg(bool in_flg)
    {
        ActivateFlg = in_flg;
    }
}
