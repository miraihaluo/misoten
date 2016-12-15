using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Frame : MonoBehaviour {

	private enum E_STATUS
	{
		NONE,
		MISSION,
		FEVER,
	
		MAX
	}

	private E_STATUS eStatus = E_STATUS.NONE;

	/// <summary>
	/// 自身のRawImageオブジェ
	/// </summary>
	private RawImage myRawImageObj;

	void Awake()
	{
		myRawImageObj = this.GetComponent<RawImage>();
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (eStatus)
		{
			case E_STATUS.MISSION:
				MissionUpdate();
				break;
		
			case E_STATUS.FEVER:
				FeverUpdate();
				break;

		}
	
	}

	private void ChangeStatus(E_STATUS changeStatus)
	{
		switch (eStatus)
		{
			case E_STATUS.MISSION:
				MissionFinalize();
				break;

			case E_STATUS.FEVER:
				FeverFinalize();
				break;
		
		}

		eStatus = changeStatus;

		switch (eStatus)
		{
			case E_STATUS.MISSION:
				MissionInitialize();
				break;

			case E_STATUS.FEVER:
				FeverInitialize();
				break;
		
		}

	}

	/// <summary>
	/// 明滅する時間
	/// </summary>
	private const float FLASH_TIME = 5.5f;
	private float flashNowTime = 0;

	/// <summary>
	/// 明滅する感覚
	/// </summary>
	private const float FLASH_KANKAKU = 0.5f;
	private float flashNowKankaku = 0;
	private bool orikaesiFlag = true;

	/// <summary>
	/// 明滅する色（二色）
	/// </summary>
	private Color[] FLASH_COLOR = { Color.white, Color.green };

	private void MissionInitialize()
	{
		flashNowTime = 0;
		flashNowKankaku = 0;
		orikaesiFlag = true;
		myRawImageObj.color = FLASH_COLOR[0];
	
	}

	private void MissionUpdate()
	{
		flashNowTime += Time.deltaTime;
		flashNowKankaku += Time.deltaTime;

		if (orikaesiFlag)
		{
			myRawImageObj.color = Color.Lerp(FLASH_COLOR[0], FLASH_COLOR[1], flashNowKankaku / FLASH_KANKAKU);
			if (flashNowKankaku >= FLASH_KANKAKU)
			{
				orikaesiFlag = !orikaesiFlag;
				flashNowKankaku = 0;

			}

		}
		else
		{
			myRawImageObj.color = Color.Lerp(FLASH_COLOR[1], FLASH_COLOR[0], flashNowKankaku / FLASH_KANKAKU);
			if (flashNowKankaku >= FLASH_KANKAKU)
			{
				orikaesiFlag = !orikaesiFlag;
				flashNowKankaku = 0;

			}

		}

		if (flashNowTime >= FLASH_TIME)
		{
			ChangeStatus(E_STATUS.NONE);
		
		}
	
	}

	private void MissionFinalize()
	{
		myRawImageObj.color = Color.white;

	}

	/// <summary>
	/// フィーバー時の明滅間隔。
	/// ランダムで可変にする
	/// </summary>
	private float feverFlashKankaku;
	private float feverFlashNowKankaku;

	private float[] feverFlashKankakuRnage = {0.25f, 1.0f};

	/// <summary>
	/// フィーバー時の明滅する色（二色）
	/// ランダムで選出する
	/// </summary>
	private Color[] feverFlashColor = new Color[2];

	/// <summary>
	/// 明滅に使用するランダムに選出する色
	/// </summary>
	private Color[] selectFeverFlashColor = {
								Color.white,
								Color.grey,
								Color.red,
								Color.blue,
								Color.green,
								Color.yellow
								};

	private int newColorID = 0;
	private int oldColorID = 0;

	private void FeverInitialize()
	{
		feverFlashColor[0] = myRawImageObj.color;
		feverFlashColor[1] = selectFeverFlashColor[RnadomSelectColor()];
		feverFlashKankaku = RandomFloatKOKU(feverFlashKankakuRnage[0], feverFlashKankakuRnage[1]);
		feverFlashNowKankaku = 0;

	}

	private void FeverUpdate()
	{
		feverFlashNowKankaku += Time.deltaTime;

		myRawImageObj.color = Color.Lerp(feverFlashColor[0], feverFlashColor[1], feverFlashNowKankaku / feverFlashKankaku);
		if (feverFlashNowKankaku >= feverFlashKankaku)
		{
			feverFlashNowKankaku = 0;
			feverFlashColor[0] = feverFlashColor[1];
			feverFlashColor[1] = selectFeverFlashColor[RnadomSelectColor()];
			feverFlashKankaku = RandomFloatKOKU(feverFlashKankakuRnage[0], feverFlashKankakuRnage[1]);

		}
	
	}

	private void FeverFinalize()
	{ }

	/// <summary>
	/// ランダムカラー用の要素数を返す
	/// </summary>
	/// <returns>要素番号</returns>
	private int RnadomSelectColor()
	{
		while(oldColorID == newColorID)
			newColorID = Random.Range(0, selectFeverFlashColor.Length);

		oldColorID = newColorID;

		return newColorID;

	}

	/// <summary>
	/// 乱数にコクを出す
	/// </summary>
	/// <param name="min">最小の範囲</param>
	/// <param name="max">最大の範囲</param>
	/// <returns>乱数値</returns>
	private float RandomFloatKOKU(float min, float max)
	{
		return (Random.Range(min, max) + Random.Range(min, max) + Random.Range(min, max) + Random.Range(min, max) + Random.Range(min, max)) / 5;
	}

	public void SetFrameStatus(MissionController.E_MISSION_TYPE missionType)
	{
		switch (missionType)
		{
			case MissionController.E_MISSION_TYPE.SPECIAL_CHILDREN:
			case MissionController.E_MISSION_TYPE.ENEMY_DRONE:
				ChangeStatus(E_STATUS.MISSION);
				break;

			case MissionController.E_MISSION_TYPE.FEVER:
				ChangeStatus(E_STATUS.FEVER);
				break;
		
		}
	
	}

}
