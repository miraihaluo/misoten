using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeMessageFunc : MonoBehaviour {

    //メッセージ送り先
    public TextMesh TextMeshObj;        
    
    //変更メッセージ用配列
    public string[] MessageStr;

    //メッセージ数用変数
    [SerializeField]
    private int MessageLength;

    // Use this for initialization
    void Start () {
        
    }
    void Awake ()
    {
        //メッセージの数
        MessageLength = MessageStr.Length;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    //外部から表示メッセージを変更(引数:メッセージ番号)
    public bool ChangeMessageText(int messaNum)
    {
        //メッセージ番号が存在しないとき(messaNumが0~MessageLengthまで)
        if (messaNum  <  0 || messaNum  > MessageLength - 1)
        {
            return false;   //何もせず0返す
        }
        //メッセージ番号が存在するとき
        else
        {
            //指定された番号のメッセージに変更
            SetMessageText(MessageStr[messaNum]);
            return true;
        }
    }

    //メッセージセット関数
    private void SetMessageText(string setMessa)
    {
        TextMeshObj.GetComponent<TextMesh>().text = setMessa;
    }
}
