using UnityEngine;
using System.Collections;


public class PlayerCamera : MonoBehaviour {

    // 追従するGameObject（プレイヤー）
    // インスペクターで指定する
    [SerializeField]
    private Transform targetPlayer;

    //カメラ座標のオフセット
    [SerializeField]
    private Vector3 posOffset;

    //カメラ角度のオフセット
    [SerializeField]
    private Vector3 lookAtOffset;

    //ディレイ時間
    [SerializeField]
    private float DelayTime;


    private Vector3 playerStartPosition;    //プレイヤーの初期座標
    private Vector3 cameraStartPosition;    //カメラの初期座標


    /*****************************************************************
        カメラ更新処理
    *****************************************************************/
    void CameraUpdate()
    {

        Vector3 newPosition;        //カメラ座標
        Quaternion newRotation;     //カメラ角度
        float rad;                  //回転角度
          
        //回転角度をラジアンに  
        rad = targetPlayer.eulerAngles.y * Mathf.PI / 180;
        
        //カメラをプレイヤーを中心に回転
        newPosition.x = -(-posOffset.x * Mathf.Cos(rad) - posOffset.z * Mathf.Sin(rad) )+ playerStartPosition.x;
        newPosition.y = targetPlayer.position.y;
        newPosition.z = -posOffset.x * Mathf.Sin(rad) + posOffset.z * Mathf.Cos(rad) + playerStartPosition.z;



        //カメラをプレイヤーの移動分平行移動、高さを合わせる
        newPosition = newPosition + (targetPlayer.position - playerStartPosition);
        newPosition.y = targetPlayer.position.y + posOffset.y;
        //カメラの向きをプレイヤーと同じに
        newRotation = targetPlayer.rotation * Quaternion.Euler(lookAtOffset);

        //ディレイカメラ更新
        transform.position = Vector3.Lerp(transform.position, newPosition, DelayTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, DelayTime * Time.deltaTime);

    }

    void Start () {
   
        //プレイヤーの初期位置
        playerStartPosition = targetPlayer.position;
        //カメラの初期位置
        cameraStartPosition = targetPlayer.position + posOffset;

        //カメラの初期情報
        transform.position = cameraStartPosition;
        transform.rotation = targetPlayer.rotation;
    }

    void Update()
    {
        //カメラ更新
        CameraUpdate();
    }
}
