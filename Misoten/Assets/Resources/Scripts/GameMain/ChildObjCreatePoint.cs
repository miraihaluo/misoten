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

	/// <summary>
	/// 固定の子供オブジェの参照を格納している配列
	/// </summary>
	private GameObject[] childrenObjArray;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		chidrenIconObj = FindObjectOfType<ChildrenIcon>();
		pos.y = 0;
		createChildCoolTime = Random.Range(15, 30);

		prefabChildObj = (GameObject)Resources.Load("Prefabs/GameMain/ChildObj");

		// 固定の子供オブジェクトの参照を受け取り、ミニマップアイコンを生成する
		Transform special = this.transform.GetChild(0);
		childrenObjArray = new GameObject[special.childCount];
		for (int i = 0; i < childrenObjArray.Length; i++)
		{
			childrenObjArray[i] = special.GetChild(i).gameObject;
			CreateChild(childrenObjArray[i].transform.position, (uint)i);

		}

		childrenID = (uint)childrenObjArray.Length;

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

				for (int i = 0; i < 2; i++)
				{
					int rand = Random.Range(0, childrenObjArray.Length - 1);
					childrenObjArray[rand].SetActive(true);
					CreateChild(childrenObjArray[rand].transform.position, (uint)rand);
				
				}


					createChildCoolTime = Random.Range(15, 30);
			
			}
		
		}

	}

	private void CreateChild(Vector3 createPos)
	{
		instansChildObj = Instantiate(prefabChildObj);
		instansChildObj.transform.position = createPos;
		instansChildObj.name = childrenID.ToString();
		instansChildObj.transform.parent = this.transform;

		// ミニマップアイコン生成
		chidrenIconObj.CreateChildIcon(createPos, childrenID);

		childrenID++;

	}

	private void CreateChild(Vector3 createPos, uint childID)
	{
		// ミニマップアイコン生成
		chidrenIconObj.CreateChildIcon(createPos, childID);

	}

	public void DestroyChild(uint childID)
	{
		chidrenIconObj.DestroyChildIcon(childID);

		if (childID < childrenObjArray.Length)
		{
			childrenObjArray[childID].SetActive(false);
			return;

		}

		foreach (Transform child in transform)
		{
			if (child.name == "Special") continue;
			if (uint.Parse(child.name) == childID)
			{
				Destroy(child.gameObject);
				break;
			}
		
		}
	
	}

}
