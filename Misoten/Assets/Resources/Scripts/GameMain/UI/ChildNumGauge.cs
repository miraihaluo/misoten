using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChildNumGauge : MonoBehaviour {

	[SerializeField]
	private PlayerControl targetPlayer;

	private Slider sliderObj;

	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		sliderObj = GetComponent<Slider>();
	
	}
	
	// Update is called once per frame
	void Update () {
		sliderObj.value = targetPlayer.Score;
	
	}
}
