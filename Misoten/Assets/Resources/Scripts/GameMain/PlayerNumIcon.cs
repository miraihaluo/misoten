using UnityEngine;
using System.Collections;

public class PlayerNumIcon : MonoBehaviour {

	void Awake()
	{
		Material myMaterial = GetComponent<Renderer>().material;

		// 各プレイヤーアイコンのUVを設定する
		myMaterial.SetTextureOffset("_MainTex", new Vector2( myMaterial.GetTextureScale("_MainTex").x * (int.Parse(this.transform.parent.name) - 1), myMaterial.GetTextureOffset("_MainTex").y));
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnWillRenderObject()
	{
		this.transform.forward = Camera.current.transform.forward;
	
	}

}
