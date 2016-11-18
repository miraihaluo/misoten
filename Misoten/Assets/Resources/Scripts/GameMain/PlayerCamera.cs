using UnityEngine;
using System.Collections;


public class PlayerCamera : MonoBehaviour {

    // 追従するGameObject（プレイヤー）
    // インスペクターで指定する
    [SerializeField]
    private Transform targetPlayer;
    

    // 追従するプレイヤーからの相対位置で注視点を指定
    // (0, 0, 0)でtargetPlayerの原点を注視
    private Vector3 lockAtOffset = new Vector3(0, -3, 0);
    
    //カメラのディレイスピード
    private float delaySpeed = 10.0f;
    

    /*****************************************************************
        カメラ更新処理
    *****************************************************************/
    void LookAt(bool slerp)
    {
        Vector3 posOffset;          //カメラ座標のオフセット
        Vector3 newPosition;        //新カメラ座標
        Quaternion newRotation;     //新カメラ角度

		//カメラ座標の計算
		/*        cameraPos.Set(	targetPlayer.position.x + (targetPlayer.forward.x * posOffset.x),
								targetPlayer.position.y + posOffset.y,
								targetPlayer.position.z + (targetPlayer.forward.z * posOffset.z)
								);

				// 注視点の計算
				lockAtPos.Set(	targetPlayer.position.x + targetPlayer.forward.x * lockAtOffset.x,
								targetPlayer.position.y + lockAtOffset.y,
								targetPlayer.position.z + targetPlayer.forward.z * lockAtOffset.z);

				transform.LookAt(lockAtPos);
		*/

        //カメラ座標のオフセット計算
        posOffset = new Vector3(-targetPlayer.forward.x * 10, 5.0f, -targetPlayer.forward.z * 10);

        //カメラ座標計算
        newPosition = targetPlayer.position + posOffset ;

        //カメラ角度計算
        newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position - lockAtOffset);

        switch (slerp)
        {
            case true:  //ディレイありTPS視点
                transform.position = Vector3.Lerp(transform.position, newPosition, delaySpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, delaySpeed * Time.deltaTime);
                break;
            case false: //ディレイ無しTPS視点
                transform.position = newPosition;
                transform.rotation = newRotation;
                break;
        }
    }


	void Start () {
        //ディレイ無しでTPS視点
        LookAt(false);

    }


    void Update()
    {
        //ディレイありでTPS視点
        LookAt(false);
    }

}
