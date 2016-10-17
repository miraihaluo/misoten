using UnityEngine;
using System.Collections;
using UnityEngine.UI;	// UI弄るのに必要

public class Rank : MonoBehaviour {

	[SerializeField]
	private PlayerControl targetPlayer;

	private RawImage rawImageObj;

	private Rect rect;

	// Use this for initialization
	void Start () {
		rect.y = 0.0f;
		rect.width = 0.25f;
		rect.height = 1.0f;

	}

	void Awake()
	{
		rawImageObj = GetComponent<RawImage>();
	
	}
	
	// Update is called once per frame
	void Update () {

		rect.x = (targetPlayer.Rank - 1) * rect.width;
		rawImageObj.uvRect = rect;

	}
}
