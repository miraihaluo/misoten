using UnityEngine;
using System.Collections;

public class ChildObjCreatePoint : MonoBehaviour {

	private GameObject prefabChildObj;

	[SerializeField]
	private Vector3 pos;

	private const int createChildCoolTime = 5;
	private float nowCreateChildTime = 0;

	// Use this for initialization
	void Start () {
		prefabChildObj = (GameObject)Resources.Load("Prefabs/GameMain/ChildObj");
	
	}
	
	// Update is called once per frame
	void Update () {

		// テキトーに再生成
		if ((int)nowCreateChildTime <= 0 && transform.childCount == 0)
		{
			Instantiate(prefabChildObj).transform.parent = this.transform;
			transform.GetChild(0).transform.position = pos;
			nowCreateChildTime = createChildCoolTime;

		}

		if (transform.childCount == 0)
		{
			nowCreateChildTime -= Time.deltaTime * 1;
		
		}


	}
}
