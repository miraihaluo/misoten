using UnityEngine;
using System.Collections;

public class child_all : MonoBehaviour {
     
    public GameObject[] Childs;

    void Awake() {
        Childs = GameObject.FindGameObjectsWithTag("Child");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
