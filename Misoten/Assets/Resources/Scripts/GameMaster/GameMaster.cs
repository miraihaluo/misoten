using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	// 終了を実行するヴァーチャルボタン名
	[SerializeField]
	private string quitButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown(quitButton))
		{
			Application.Quit();
		
		}

	}
}
