using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;  // シーン遷移に使う関数がある名前空間

public class PlayerSelectSceneController : MonoBehaviour
{
	// スペースキーで画面遷移
	public string sceneChangeVirtualKeyName = "Jump";

	[SerializeField]    // 変数をインスペクターから変更できるようにする
	private string nextSceneName;

	private PlayerData playerData;

	// Use this for initialization
	void Start()
	{
	
	}

	void Awake()
	{
		playerData = Resources.Load<PlayerData>("Assets/PlayerData");
	
	}

	// Update is called once per frame
	void Update () {

		// プレイヤー参加人数の操作　仮
		int addPlyaerNum = 0;

		if (Input.GetButtonDown("Vertical"))
		{
			if (Input.GetAxis("Vertical") > 0)
				addPlyaerNum = 1;

			if (Input.GetAxis("Vertical") < 0)
				addPlyaerNum = -1;

			playerData.AddPlayerNum(addPlyaerNum);

		}

		// とりあえず参加人数が0より多かったら遷移出来る
		if (playerData.GetPlayerNum() > 0)
			CheckChengeScene();
	
	}

	private void CheckChengeScene()
	{
		if (Input.GetButtonDown(sceneChangeVirtualKeyName))
		{
			playerData.ScoreArrayInstance();
			SceneManager.LoadScene(nextSceneName);

		}
	}

}
