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

	}

	void Awake()
	{
		rawImageObj = GetComponent<RawImage>();

		rect = rawImageObj.uvRect;

	}
	
	// Update is called once per frame
	void Update () {

		rect.x = (targetPlayer.Rank - 1) * rect.width;
		rawImageObj.uvRect = rect;

	}
}
