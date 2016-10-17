using UnityEngine;
using System.Collections;

public class MissionController : MonoBehaviour {

	// 残り時間を持っているGameMainSceneControllerオブジェの入れ子
	private GameMainSceneController sceneObj;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		// GameMainSceneControllerのゲームオブジェクトを取得
		sceneObj = GameObject.FindObjectOfType<GameMainSceneController>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
