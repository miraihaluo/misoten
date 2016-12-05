using UnityEngine;
using System.Collections;

public class TitlePlayer : MonoBehaviour
{

	//ひとつ前のプレイヤー座標
	[SerializeField]
	private Vector3 oldPos;

	/// <summary>
	/// ベジェ曲線
	/// </summary>
	[SerializeField]
	private Vector3[] rootPos = new Vector3[6];

	//速さ
	[SerializeField]
	private float speed;

	//ベジェ
	private Bezier moveBezier;

	private float time = 0.0f;

	//プレイヤーのルートリセットフラグ
	private bool bezierResetFlg = false;

	// Use this for initialization
	void Awake()
	{
		oldPos = transform.localPosition;
		moveBezier = new Bezier(rootPos[0], rootPos[1], rootPos[2], rootPos[3]);
	}

	// Update is called once per frame
	void Update()
	{

		transform.localPosition = moveBezier.GetPointAtTime(time);

		//今と前との位置の差
		Vector3 posDif = transform.localPosition - oldPos;
		oldPos = transform.localPosition;

		//プレイヤーの向き調整
		if (posDif.magnitude > 0.01)
		{
			transform.localRotation = Quaternion.LookRotation(posDif);
		}

		time += speed;

		if (time > 1f)
		{
			time = 0f;
			//ルート１
			if (bezierResetFlg == true)
			{
				moveBezier = new Bezier(rootPos[0], rootPos[1], rootPos[2], rootPos[3]);
				bezierResetFlg = false;
			}
			else//ルート2
			{
				moveBezier = new Bezier(rootPos[3], rootPos[4], rootPos[5], rootPos[0]);
				bezierResetFlg = true;
			}

		}


	}
}
