using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    MainManager _mainManager;

    private void Start()
    {
        _mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    // 플레이어와 충돌한 콜라이더의 name씬으로 이동
    // 충돌중일때 게이지를 채우고, 게이지가 다 차면 swithMap()을 호출하는 방식으로 변경해야함
    private void OnTriggerEnter(Collider other)
    {
        string sceneName = other.gameObject.name;
        _mainManager.SwitchMap(sceneName);
    }
}
