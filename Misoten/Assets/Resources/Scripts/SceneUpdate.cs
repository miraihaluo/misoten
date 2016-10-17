using UnityEngine;
using System.Collections;

public class SceneUpdate : MonoBehaviour {

	// updateGameObj配列に格納するGameObjectを検索する時に使うタグ名配列
	private string[] tagNameArray =
	{
		"Player",
		"PlayerCamera",
		"UI"

	};

	// ExUpdateを実行するGameObject配列を格納する配列（ジャグ配列）
	private GameObject[][] updateGameObjJaggedArray;

	// Use this for initialization
	void Start () {

		// ExUpdateを実行するジャグ配列の、格納する配列数をタグの数分確保する
		updateGameObjJaggedArray = new GameObject[tagNameArray.Length][];

		// ExUpdateを実行するジャグ配列に、各タグに該当するGameObject配列を格納していく
		for (int i = 0; i < tagNameArray.Length; i++)
		{
			updateGameObjJaggedArray[i] = GameObject.FindGameObjectsWithTag(tagNameArray[i]);

		}
	
	}
	
	// Update is called once per frame
	void Update () {

		// ジャグ配列に格納しているGameObjectのExUpdateを実行していく
		for (int i = 0; i < tagNameArray.Length; i++)
		{
			foreach (GameObject gameObj in updateGameObjJaggedArray[i])
			{
				gameObj.gameObject.SendMessage("ExUpdate");

			}

		}

	}
}
