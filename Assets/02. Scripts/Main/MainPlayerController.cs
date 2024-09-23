using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    MainSceneManager _mainSceneManager;

    private void Start()
    {
        _mainSceneManager = GameObject.Find("SceneManager").GetComponent<MainSceneManager>();
    }


    // 플레이어와 충돌한 콜라이더의 name씬으로 이동
    private void OnTriggerEnter(Collider other)
    {
        string sceneName = other.gameObject.name;
        _mainSceneManager.OnLoadSceneByName(sceneName);
    }
}
