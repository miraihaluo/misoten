using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChengeTimerType : MonoBehaviour {

	private enum E_TIMERTYPE
	{
		MINUTE = 0,
		SEC,
	
		MAX
	}

	private E_TIMERTYPE eTimerType = E_TIMERTYPE.MINUTE;

	GameMainSceneController sceneObj;

	private GameObject[] timerObjArray;

	void Awake()
	{
		sceneObj = FindObjectOfType<GameMainSceneController>();

		timerObjArray = new GameObject[transform.childCount];

		foreach (Transform child in transform)
		{
			if (child.name == "Minute")
				timerObjArray[(int)E_TIMERTYPE.MINUTE] = child.gameObject;

			if (child.name == "Sec")
				timerObjArray[(int)E_TIMERTYPE.SEC] = child.gameObject;
		
		}

		timerObjArray[(int)E_TIMERTYPE.MINUTE].SetActive(true);
		timerObjArray[(int)E_TIMERTYPE.SEC].SetActive(false);
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (eTimerType == E_TIMERTYPE.SEC) return;

		if (sceneObj.NowTime < 60)
		{
			eTimerType = E_TIMERTYPE.SEC;

			timerObjArray[(int)E_TIMERTYPE.MINUTE].SetActive(false);
			timerObjArray[(int)E_TIMERTYPE.SEC].SetActive(true);
		
		}
	
	}

}
