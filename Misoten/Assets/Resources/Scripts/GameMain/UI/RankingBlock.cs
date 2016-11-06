using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankingBlock : MonoBehaviour {

	/// <summary>
	/// Demo用
	/// </summary>
	private enum E_DEMO
	{
		RIGHT,
		LEFT,
	
		MAX
	}

	/// <summary>
	/// Demo用
	/// </summary>
	E_DEMO eDemoStatus = E_DEMO.RIGHT;

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
	private int newRanking = 1;

	/// <summary>
	/// 変化前の順位
	/// </summary>
	private int oldRanking = 1;

	/// <summary>
	/// 初期位置
	/// </summary>
	private Vector2 defPos;

	/// <summary>
	/// 動かす値
	/// </summary>
	private Vector2 movePos = Vector2.zero;

	/// <summary>
	/// 移動スピード
	/// </summary>
	[SerializeField]
	private float moveSpeed = 60;

	/// <summary>
	/// Demo用
	/// </summary>
	private bool testFlag = false;

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
			testFlag = true;

		if (testFlag)
			Demo();
	
	}

	/// <summary>
	/// デモ
	/// </summary>
	private void Demo()
	{
		switch (eDemoStatus)
		{
			case E_DEMO.RIGHT:
				movePos.x += moveSpeed * Time.deltaTime;
				if (movePos.x >= myRectTransform.sizeDelta.x)
					eDemoStatus = E_DEMO.LEFT;

				break;

			case E_DEMO.LEFT:
				movePos.x -= moveSpeed * Time.deltaTime;
				if (movePos.x < 0)
				{
					movePos.x = 0;
					eDemoStatus = E_DEMO.RIGHT;
					testFlag = false;

				}
				break;
		
		}

		myRectTransform.anchoredPosition = defPos + movePos;

	}

	/// <summary>
	/// 順位の更新
	/// </summary>
	/// <param name="ranking">変化後の順位</param>
	public void UpdateRanking(int ranking)
	{
		newRanking = ranking;

		testFlag = true;

	}

}
