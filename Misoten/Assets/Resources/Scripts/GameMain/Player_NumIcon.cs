using UnityEngine;
using System.Collections;

public class Player_NumIcon : MonoBehaviour {

	private Vector3 defPos;
	private Transform parent;
	private float distance;

	void Awake()
	{
		defPos = this.transform.localPosition;
		parent = this.transform.parent;
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnWillRenderObject()
	{
		if (parent.position.y < Camera.current.transform.position.y) return;

		distance = Vector3.Distance(Camera.current.transform.position, parent.position);

		if (Mathf.Asin((parent.position.y - Camera.current.transform.position.y) / distance) * 180 / Mathf.PI < 30.0f)
			this.transform.localPosition = defPos;
		else
			this.transform.localPosition = -defPos;
	
	
	}

}
