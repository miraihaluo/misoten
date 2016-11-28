﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;  // シーン遷移に使う関数がある名前空間

public class TutorialSceneControllar : MonoBehaviour
{

    // スペースキーで画面遷移
    public string sceneChangeVirtualKeyName = "Jump";

    [SerializeField]    // 変数をインスペクターから変更できるようにする
    private string nextSceneName;

    [SerializeField]
    private AudioSource sound01;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(sceneChangeVirtualKeyName))
        {
            sound01.Play();
            FadeManager.Instance.LoadLevel(nextSceneName, 1.0f);
        }

    }
}
