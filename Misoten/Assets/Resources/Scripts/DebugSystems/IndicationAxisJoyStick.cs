using UnityEngine;
using System.Collections;
using UnityEngine.UI;	// UI系をいじるのに必要

public class IndicationAxisJoyStick : MonoBehaviour {

	private Image axisBorder_ImageObj;
	private Image axisVal_ImageObj;
	private Text inputKey_TextObj;

	[SerializeField]
	private float pointSize = 10;
	private float borderHalfSize;

	private Vector3 axisVal_Vec3 = Vector3.one;

	/// <summary>
	/// 表示するプレイヤー番号 1～4
	/// </summary>
	public int playerID = 0;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		Image[] childrenComponentsArray = GetComponentsInChildren<Image>();

		foreach (Image work in childrenComponentsArray)
		{
			if (work.name == "AxisBorder")
				axisBorder_ImageObj = work;

			if (work.name == "AxisVal")
				axisVal_ImageObj = work;

		}

		inputKey_TextObj = GetComponentInChildren<Text>();

		axisVal_ImageObj.rectTransform.sizeDelta = new Vector2(pointSize , pointSize);
		borderHalfSize = axisBorder_ImageObj.rectTransform.rect.width / 2;

	}

	// Update is called once per frame
	void Update () {
		axisVal_Vec3.x = borderHalfSize * Input.GetAxis("Horizontal" + playerID);
		axisVal_Vec3.y = borderHalfSize * Input.GetAxis("Vertical" + playerID);

		axisVal_ImageObj.rectTransform.localPosition = axisVal_Vec3;

		inputKey_TextObj.text = "プレイヤー" + playerID + "\n";
		inputKey_TextObj.text += "ATTACK：";

		if (Input.GetButtonDown("Attack" + playerID))
		{
			inputKey_TextObj.text += "押した";

		}
		else
		{
			inputKey_TextObj.text += "押してない";

		}


	}
}
