using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BGMove : MonoBehaviour {

	private RawImage BGObj;

	private Rect UVRect;

	void Awake()
	{
		BGObj = this.GetComponent<RawImage>();
		UVRect = BGObj.uvRect;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UVRect.x += Time.deltaTime;
		UVRect.y -= Time.deltaTime;

		BGObj.uvRect = UVRect;
	
	}
}
