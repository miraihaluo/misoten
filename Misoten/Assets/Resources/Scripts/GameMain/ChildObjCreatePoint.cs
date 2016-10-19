using UnityEngine;
using System.Collections;

public class ChildObjCreatePoint : MonoBehaviour {

	private GameObject prefabChildObj;
	private GameObject instansChildObj;

	[SerializeField]
	private Vector3 pos;

	private float createChildCoolTime;

	/// <summary>
	/// 存在する最大子供の人数
	/// </summary>
	private const int MAX_CHILDREN = 15;

	// Use this for initialization
	void Start () {
		pos.y = 25;
		createChildCoolTime = Random.Range(15, 30);
	
	}

	void Awake()
	{
		prefabChildObj = (GameObject)Resources.Load("Prefabs/GameMain/ChildObj");

		// 最初にいくつか生成
		for (int i = 0; i < 9; i++)
		{
			Instantiate(prefabChildObj).transform.parent = this.transform;
			pos.x = Random.Range(-150, 150);
			pos.z = Random.Range(-150, 150);
			transform.GetChild(i).transform.position = pos;
		
		}

	}

	// Update is called once per frame
	void Update () {

		// テキトーに再生成
		if (transform.childCount < 15)
		{
			createChildCoolTime -= Time.deltaTime * 60;

			if (createChildCoolTime <= 0)
			{
				pos.x = Random.Range(-150, 150);
				pos.z = Random.Range(-150, 150);

				instansChildObj = Instantiate(prefabChildObj);
				instansChildObj.transform.position = pos;
				instansChildObj.transform.parent = this.transform;

				createChildCoolTime = Random.Range(15, 30);
			
			}
		
		}


	}
}
