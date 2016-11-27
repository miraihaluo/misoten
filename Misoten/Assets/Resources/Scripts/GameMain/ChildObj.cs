using UnityEngine;
using System.Collections;

public class ChildObj : MonoBehaviour {

	private Vector3 forward;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnWillRenderObject()
	{
//		forward = -Camera.current.transform.forward;
		forward = Vector3.Normalize(Camera.current.transform.position - this.transform.position);
		forward.y = 0.0f;
		this.transform.up = forward;

	}

}
