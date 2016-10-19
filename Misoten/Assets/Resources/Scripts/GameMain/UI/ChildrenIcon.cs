using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChildrenIcon : MonoBehaviour {

	/// <summary>
	/// プレイヤー毎のミニマップオブジェを取得
	/// </summary>>
	private GameObject[] childPlayerMiniMapArray;

	/// <summary>
	/// ミニマップ用アイコンのプレハブデータ
	/// </summary>
	private GameObject prefabChildrenIcon;
	private GameObject instansChildrenIcon;


	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		childPlayerMiniMapArray = new GameObject[transform.childCount];

		for (int i = 0; i < transform.childCount; i++)
			childPlayerMiniMapArray[i] = transform.GetChild(i).gameObject;

			prefabChildrenIcon = (GameObject)Resources.Load("Prefabs/GameMain/UIs/MiniMap_Child");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateChildIcon(Vector3 pos, uint childID)
	{
		// (ミニマップの大きさ / 2) * 子供オブジェの座標 / (ステージの広さ / 2)
		pos.x = (200 / 2) * pos.x / (300 / 2);
		pos.y = (200 / 2) * pos.z / (300 / 2);

		foreach (GameObject playerNo in childPlayerMiniMapArray)
		{
			instansChildrenIcon = Instantiate(prefabChildrenIcon);
			instansChildrenIcon.transform.position = pos;
			instansChildrenIcon.transform.SetParent(playerNo.transform, false);
			instansChildrenIcon.name = childID.ToString();
		
		}
	
	
	}

	public void DestroyChildIcon(uint childID)
	{

		foreach (GameObject playerNo in childPlayerMiniMapArray)
		{
			foreach (Transform childIcon in playerNo.transform)
			{
				if (uint.Parse(childIcon.name) == childID)
				{
					Destroy(childIcon.gameObject);
					break;
				}
			
			}
		
		}
	
	}

}
