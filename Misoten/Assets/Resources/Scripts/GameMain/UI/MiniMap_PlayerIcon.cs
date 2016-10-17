using UnityEngine;
using System.Collections;
using UnityEngine.UI;	// UI弄るのに必要

public class MiniMap_PlayerIcon : MonoBehaviour {

	private Image imageObj;
	private Vector3 pos = Vector3.one;

	[SerializeField]
	private GameObject targetPlayer;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		imageObj = this.GetComponent<Image>();
	
	}


	// Update is called once per frame
	void Update () {
		pos.x = targetPlayer.transform.position.x * (300 / 200);

	
	}
}
