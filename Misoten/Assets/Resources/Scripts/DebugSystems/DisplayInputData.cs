using UnityEngine;
using System.Collections;

public class DisplayInputData : MonoBehaviour
{
	private enum E_DISPLAY_MODE
	{
		KEYBOARD = 0,
		PLAYER_1,
		PLAYER_2,
		PLAYER_3,
		PLAYER_4,
	
		MAX
	}

	/// <summary>
	/// 表示する入力情報を決めるenum
	/// </summary>
	E_DISPLAY_MODE eDisplayMode = E_DISPLAY_MODE.KEYBOARD;

	private IndicationAxisJoyStick joystickDisplay;
	private IndicationInputKey keyboardDisplay;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		joystickDisplay = GetComponentInChildren<IndicationAxisJoyStick>();
		keyboardDisplay  = GetComponentInChildren<IndicationInputKey>();

		joystickDisplay.gameObject.SetActive(false);
		keyboardDisplay.gameObject.SetActive(true);

	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.I))
		{
			eDisplayMode = (E_DISPLAY_MODE)(((int)eDisplayMode + 1) % (int)E_DISPLAY_MODE.MAX);

			switch (eDisplayMode)
			{
				case E_DISPLAY_MODE.KEYBOARD:
					joystickDisplay.gameObject.SetActive(false);
					keyboardDisplay.gameObject.SetActive(true);

					break;

				case E_DISPLAY_MODE.PLAYER_1:
					joystickDisplay.gameObject.SetActive(true);
					keyboardDisplay.gameObject.SetActive(false);

					joystickDisplay.playerID = 1;

					break;

				case E_DISPLAY_MODE.PLAYER_2:
					joystickDisplay.playerID = 2;

					break;

				case E_DISPLAY_MODE.PLAYER_3:
					joystickDisplay.playerID = 3;

					break;

				case E_DISPLAY_MODE.PLAYER_4:
					joystickDisplay.playerID = 4;

					break;

			}

		}

	
	}
}
