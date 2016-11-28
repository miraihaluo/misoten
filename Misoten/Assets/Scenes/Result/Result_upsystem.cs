using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Result_upsystem : MonoBehaviour
{

    public ResultSystem system;
    private float scale;
    public RectTransform myRT;
    public int p_no;
    private float init_height;
    private bool stop = false;
    public Text text;


    public float top_scale;
    public float speed;
    public float acc;

    [SerializeField]
    private AudioSource Dram_SE;
    [SerializeField]
    private AudioSource Finish_SE;

    private int[] rank = new int[4];
    private bool flg = false;

    // Use this for initialization
    void Start()
    {
        p_no -= 1;
        scale = myRT.sizeDelta.y;
        init_height = scale;
    }


    // Update is called once per frame
    void Update()
    {
        if (!flg)
        {
            ResultSystem result = GameObject.Find("result").GetComponent<ResultSystem>();
            rank = result.GetRank();
            if (rank[p_no] == 1)
            {
                Dram_SE.Play();
                flg = true;
            }
        }

        if (!stop)
        {


            if (system.GetScore()[p_no] * top_scale + init_height < scale)
            {
                myRT.sizeDelta = new Vector2(myRT.sizeDelta.x, init_height + system.GetScore()[p_no] * top_scale);
                stop = true;
                text.text = system.GetRank()[p_no].ToString();
                text.transform.gameObject.SetActive(true);


                if (rank[p_no] == 1)
                {
                    Dram_SE.Stop();
                    Finish_SE.Play();
                }
            }
            else
            {
                myRT.sizeDelta = new Vector2(myRT.sizeDelta.x, scale);

            }
            scale += Time.deltaTime * speed;
            speed += speed * acc * Time.deltaTime;
        }
    }
}
