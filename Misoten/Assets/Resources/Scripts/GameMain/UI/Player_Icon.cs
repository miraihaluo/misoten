using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Icon : MonoBehaviour {

	[SerializeField]
	Camera camera = null;

	[SerializeField]
	Transform target = null;

	RectTransform rectTransform = null;
	RawImage rawImageObj;
	Rect rect;
	Vector2 size;

	/// <summary>
	/// 表示される最大サイズ
	/// </summary>
	[SerializeField]
	private Vector2 MAX_SIZE;

	/// <summary>
	/// 表示される最小サイズ
	/// </summary>
	[SerializeField]
	private Vector2 MIN_SIZE;

	/// <summary>
	/// 表示する遠距離限界
	/// </summary>
	[SerializeField]
	private float MAX_LENGTH = 100.0f;

	/// <summary>
	/// 表示する近距離限界
	/// </summary>
	[SerializeField]
	private float MIN_LENGTH = 20.0f;

	/// <summary>
	/// 最大サイズになる距離
	/// </summary>
	[SerializeField]
	private float MAX_SIZE_LENGTH = 30.0f;

	/// <summary>
	/// 最小サイズになる距離
	/// </summary>
	[SerializeField]
	private float MIN_SIZE_LENGTH = 80.0f;

	private float distance;

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		rawImageObj = GetComponent<RawImage>();
	
	}

	// Use this for initialization
	void Start () {
		rect.width = rawImageObj.uvRect.width;
		rect.height = rawImageObj.uvRect.height;
		rect.x = (int.Parse(target.parent.name) - 1) * rawImageObj.uvRect.width;
		rect.y = rawImageObj.uvRect.y;

		rawImageObj.uvRect = rect;

	}
	
	// Update is called once per frame
	void LateUpdate () {
		rawImageObj.color = Color.clear;
		distance = Vector3.Distance(camera.transform.position, target.position);

		if (distance > MAX_LENGTH) return;
		if (distance < MIN_LENGTH) return;
		if (Vector3.Angle(camera.transform.forward, target.position - camera.transform.position) > 90.0f) return;

		SizeFunction();

		rectTransform.position = RectTransformUtility.WorldToScreenPoint(camera, target.position);

		if (rectTransform.position.x < 0 + rectTransform.rect.width / 2) return;
		if (rectTransform.position.x > 960 - rectTransform.rect.width / 2) return;
		if (rectTransform.position.y < 0 + rectTransform.rect.height / 2) return;
		if (rectTransform.position.y > 540 - rectTransform.rect.height / 2) return;

		rawImageObj.color = Color.white;

		rectTransform.localPosition = rectTransform.position;

	}

	void SizeFunction()
	{
		if (distance > MIN_SIZE_LENGTH)
		{
			rectTransform.sizeDelta = MIN_SIZE;
			return;
		}

		if (distance < MAX_SIZE_LENGTH)
		{
			rectTransform.sizeDelta = MAX_SIZE;
			return;
		}

		size.x = size.y = MAX_SIZE.x * (1.00f - (distance - MAX_SIZE_LENGTH) / (MIN_SIZE_LENGTH - MAX_SIZE_LENGTH));

		if (size.x < MIN_SIZE.x)
			size.x = size.y = MIN_SIZE.x;

		rectTransform.sizeDelta = size;

	}

}
