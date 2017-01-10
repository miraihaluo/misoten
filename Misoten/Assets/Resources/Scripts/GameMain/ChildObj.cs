using UnityEngine;
using System.Collections;

public class ChildObj : MonoBehaviour {

	private Vector3 forward;

	/// <summary>
	/// 対応したミニマップの子供アイコンのゲームオブジェ
	/// </summary>
	public GameObject[] miniMapIcon = new GameObject[4];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActiveOn()
	{
		foreach (GameObject obj in miniMapIcon)
			obj.SetActive(true);
		
	}

	public void ActiveOff()
	{/*
		foreach (GameObject obj in miniMapIcon)
			obj.SetActive(false);*/

		this.gameObject.SetActive(false);
	
	}

}
