using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;  // シーン遷移に使う関数がある名前空間

public class TitleSceneController : MonoBehaviour
{

	[SerializeField]    // 変数をインスペクターから変更できるようにする
	private string nextSceneName;

	//音楽データ
	[SerializeField]
	private AudioSource sound01;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

		// キーボード、マウスの何かしらのボタンのトリガーをtrueで返す
		if (Input.anyKeyDown)
		{
			sound01.Play();
			FadeManager.Instance.LoadLevel(nextSceneName, 1.0f);

			//SceneManager.LoadScene(nextSceneName);
		}

	}

}
