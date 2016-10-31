using UnityEngine;
using System.Collections;

public class AttackObj : MonoBehaviour {

	private enum E_STATUS {
		FORWARD,
		BACK
	
	}

	private E_STATUS eStatus = E_STATUS.FORWARD;

	/// <summary>
	/// 当たり判定が生きているかどうか
	/// </summary>
	private bool alive_f = true;

	/// <summary>
	/// 移動スピード
	/// </summary>
	[SerializeField]
	private float moveSpeed = 2.0f * 60.0f;

	[SerializeField]
	private float maxMove = 60.0f;

	/// <summary>
	/// 移動量
	/// </summary>
	private Vector3 moveVec;

	/// <summary>
	/// 進行方向
	/// </summary>
	private Vector3 forwardVec;
	public Vector3 ForwardVec { set { forwardVec = value; } }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		moveVec = forwardVec * Time.deltaTime * moveSpeed;

		switch (eStatus)
		{
			case E_STATUS.FORWARD:
				this.transform.Translate(moveVec, Space.Self);

				if (Vector3.Distance(this.transform.parent.transform.position, this.transform.position) > maxMove)
					eStatus = E_STATUS.BACK;

				break;

			case E_STATUS.BACK:
				this.transform.Translate(-moveVec, Space.Self);

				if (Vector3.Distance(this.transform.parent.transform.position, this.transform.position) < 10)
					this.transform.parent.SendMessage("DestroyAttackObj");

				break;
		
		}

	
	}

	void OnTriggerEnter(Collider other)
	{
		if (!alive_f) return;
		if (other.tag != "Player") return;
		if (other.transform.gameObject == this.transform.parent.gameObject) return;

		if (other.GetComponent<PlayerControl>().Score > 0)
			this.transform.parent.GetComponent<PlayerControl>().AttackSuccess();

		other.GetComponent<PlayerControl>().AttackDamage();

		alive_f = false;

	}
	
}
