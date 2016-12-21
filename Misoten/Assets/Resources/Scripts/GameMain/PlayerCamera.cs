using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	/// <summary>
	/// ゲームの進行状態
	/// </summary>
/*	private enum E_STATUS
	{
		MIWATASI,	// 開始のデモムービー
		SYUTUGEN,	// プレイヤーが降りてくる
		START,// UIの出現、開始のカウントダウン
		GAME,	// ゲームメイン
		END,	// 終了動作

		MAX
	};
*/
	private GameMainSceneController.E_STATUS eStatus = GameMainSceneController.E_STATUS.MIWATASI;

	/// <summary>
	/// シーンコントローラーオブジェ
	/// </summary>
	[SerializeField, Header("シーンコントローラーオブジェ")]
	private GameMainSceneController sceneObj;

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

	/// <summary>
	/// 見渡している経過時間
	/// カットタイミング等を計る用
	/// </summary>
	private float miwatasiTimer = 0;

	/// <summary>
	/// 見渡し状態の時の移動スピード
	/// </summary>
	[SerializeField, Header("見渡し状態の時の移動スピード")]
	private float miwatasiSpeed = 5;

	/// <summary>
	/// カメラの通貨するポイント
	/// </summary>
	[SerializeField, Header("カメラの通過するポイント")]
	private Vector3[] movePoint;
	private int movePointCount = 0;

	/// <summary>
	/// カメラの向く方向
	/// </summary>
	[SerializeField, Header("カメラの向く方向")]
	private Vector3[] forwardVec;
	private int forwardVecCount = 0;

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
		MiwatasiInitialize();

    }

    void LateUpdate()
	{
		switch (eStatus)
		{
			case GameMainSceneController.E_STATUS.MIWATASI:
				MiwatasiUpdate();
				break;

			case GameMainSceneController.E_STATUS.SYUTUGEN:
				SyutugenUpdate();
				break;

			case GameMainSceneController.E_STATUS.START:
				StartUpdate();
				break;

			case GameMainSceneController.E_STATUS.GAME:
				GameUpdate();
				break;

			case GameMainSceneController.E_STATUS.END:
				EndUpdate();
				break;

		}

    }

	public void ChangeStatus(GameMainSceneController.E_STATUS changeStatus)
	{
		// 終了処理
		switch (eStatus)
		{
			case GameMainSceneController.E_STATUS.MIWATASI:
				MiwatasiFinalize();
				break;

			case GameMainSceneController.E_STATUS.SYUTUGEN:
				SyutugenFinalize();
				break;

			case GameMainSceneController.E_STATUS.START:
				StartFinalize();
				break;

			case GameMainSceneController.E_STATUS.GAME:
				GameFinalize();
				break;

			case GameMainSceneController.E_STATUS.END:
				EndFinalize();
				break;

		}

		eStatus = changeStatus;

		switch (eStatus)
		{
			case GameMainSceneController.E_STATUS.MIWATASI:
				MiwatasiInitialize();
				break;

			case GameMainSceneController.E_STATUS.SYUTUGEN:
				SyutugenInitialize();
				break;

			case GameMainSceneController.E_STATUS.START:
				StartInitialize();
				break;

			case GameMainSceneController.E_STATUS.GAME:
				GameInitialize();
				break;

			case GameMainSceneController.E_STATUS.END:
				EndInitialize();
				break;

		}

	}

	private void MiwatasiInitialize()
	{
		movePointCount = 0;
		forwardVecCount = 0;

		this.transform.position = movePoint[movePointCount];
		movePointCount++;
	
		this.transform.forward = forwardVec[forwardVecCount];
		forwardVecCount++;

	}

	private void MiwatasiUpdate()
	{
		miwatasiTimer += Time.deltaTime;

		this.transform.position = Vector3.MoveTowards(this.transform.position, movePoint[movePointCount], miwatasiSpeed * Time.deltaTime);
	
		if(Vector3.Distance(this.transform.position, movePoint[movePointCount]) <= 0)
		{
			movePointCount++;

			if (movePointCount >= movePoint.Length)
			{
				sceneObj.SetStatus(GameMainSceneController.E_STATUS.SYUTUGEN);
				return;

			}

			this.transform.position = movePoint[movePointCount];
			movePointCount++;

			this.transform.forward = forwardVec[forwardVecCount];
			forwardVecCount++;

		}

	}

	private void MiwatasiFinalize()
	{ }

	private void SyutugenInitialize()
	{ }

	private void SyutugenUpdate()
	{ }

	private void SyutugenFinalize()
	{ }

	private void StartInitialize()
	{ }

	private void StartUpdate()
	{ }

	private void StartFinalize()
	{ }

	private void GameInitialize()
	{
		//プレイヤーの初期位置
		playerStartPosition = targetPlayer.position;
		//カメラの初期位置
		cameraStartPosition = targetPlayer.position + posOffset;

		//カメラの初期情報
		transform.position = cameraStartPosition;
		transform.rotation = targetPlayer.rotation;

	}

	private void GameUpdate()
	{
		//カメラ更新
		CameraUpdate();

	}

	private void GameFinalize()
	{ }

	private void EndInitialize()
	{ }

	private void EndUpdate()
	{

	}

	private void EndFinalize()
	{ }

}
