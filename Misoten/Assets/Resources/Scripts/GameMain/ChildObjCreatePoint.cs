using UnityEngine;
using System.Collections;

public class ChildObjCreatePoint : MonoBehaviour {

	private GameObject prefabChildObj;
	private GameObject instansChildObj;

	/// <summary>
	/// ミニマップ用のアイコンを生成するオブジェを取得
	/// </summary>
	private ChildrenIcon chidrenIconObj;

	private Vector3 pos;
	public Vector3 Pos { get { return pos; } }

	private float createChildCoolTime;

	private uint childrenID = 0;

	/// <summary>
	/// 存在する最大子供の人数
	/// </summary>
	private const int MAX_CHILDREN = 15;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		chidrenIconObj = FindObjectOfType<ChildrenIcon>();
		pos.y = 25;
		createChildCoolTime = Random.Range(15, 30);

		prefabChildObj = (GameObject)Resources.Load("Prefabs/GameMain/ChildObj");

		// 最初にいくつか生成
		for (int i = 0; i < 9; i++)
		{
			pos.x = Random.Range(-150, 150);
			pos.z = Random.Range(-150, 150);

			CreateChild(pos);
		
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

				CreateChild(pos);

				createChildCoolTime = Random.Range(15, 30);
			
			}
		
		}

	}

	private void CreateChild(Vector3 createPos)
	{
		instansChildObj = Instantiate(prefabChildObj);
		instansChildObj.transform.position = pos;
		instansChildObj.name = childrenID.ToString();
		instansChildObj.transform.parent = this.transform;

		// ミニマップアイコン生成
		chidrenIconObj.CreateChildIcon(pos, childrenID);

		childrenID++;

	}

	public void DestroyChild(uint childID)
	{
		chidrenIconObj.DestroyChildIcon(childID);

		foreach (Transform child in transform)
		{
			if (uint.Parse(child.name) == childID)
			{
				Destroy(child.gameObject);
				break;
			}
		
		}
	
	}

}
