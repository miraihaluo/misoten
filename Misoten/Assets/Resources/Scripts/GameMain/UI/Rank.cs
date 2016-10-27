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


	}

	public void UpdateNowRank()
	{
		// U:0が1位なので、-1して合わせる
		rect.x = (targetPlayer.Rank - 1) * rect.width;
		rawImageObj.uvRect = rect;
	
	}

}
