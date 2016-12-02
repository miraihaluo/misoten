using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Result_upsystem : MonoBehaviour {

    public ResultSystem system;
    private float scale;
    public RectTransform myRT;
    public int p_no;
    private float init_height;
    private bool stop=false;
    public Text text;


    public float top_scale;
    public float speed;
    public float acc;
    

	// Use this for initialization
	void Start () {
        p_no -= 1;
        scale = myRT.sizeDelta.y;
        init_height = scale;
	}

	
	// Update is called once per frame
	void Update () {

        if (!stop)
        {
            

            if (system.GetScore()[p_no] * top_scale + init_height < scale)
            {
                myRT.sizeDelta = new Vector2(myRT.sizeDelta.x, init_height + system.GetScore()[p_no] * top_scale);
                stop = true;
                text.text = system.GetRank()[p_no].ToString();
                text.transform.gameObject.SetActive(true);

            }
            else
            {
                myRT.sizeDelta = new Vector2(myRT.sizeDelta.x, scale);

            }
            scale += Time.deltaTime * speed;
            speed += speed * acc * Time.deltaTime ; 
        }
    }
}
