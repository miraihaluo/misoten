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
		this.gameObject.SetActive(true);

		foreach (GameObject obj in miniMapIcon)
			obj.SetActive(true);

	}

	public void ActiveOff()
	{
		foreach (GameObject obj in miniMapIcon)
			obj.SetActive(false);

		this.gameObject.SetActive(false);
	
	}

	void OnWillRenderObject()
	{
//		forward = -Camera.current.transform.forward;
		forward = Vector3.Normalize(Camera.current.transform.position - this.transform.position);
		forward.y = 0.0f;
		this.transform.forward = forward;

	}

}
