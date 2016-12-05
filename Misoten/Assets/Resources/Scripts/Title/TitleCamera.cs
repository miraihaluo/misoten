using UnityEngine;
using System.Collections;

public class TitleCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(0.0f, 20.0f * Time.deltaTime, 0.0f);
	
	}
}
