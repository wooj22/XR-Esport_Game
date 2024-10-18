using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerGameManager : MonoBehaviour
{
    [Header("Levels")]
    [Header("MapData")]
    [Header("Managers")]
    [SerializeField] RollerUIManager _rollerUIManager;
    [SerializeField] RollerSoundManager _rollerSoundManager;
    [SerializeField] RollerSceneManager _rollerSceneManager;

    private void Start()
    {
        RollerMapStartSetting();

        // 맵 개발 전 전체씬 로직 테스트
        StartCoroutine(test());
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(5f);
        _rollerSoundManager.StopBGM();
        _rollerUIManager.FadeOutImage();

        yield return new WaitForSeconds(8f);
        _rollerSceneManager.LoadMainMenuMap();
    }

    /*-------------- Game -------------------*/
    /// 맵 초기 셋팅
    private void RollerMapStartSetting()
    {
        // BGM
        _rollerSoundManager.PlayBGM();

        // 페이드인
        _rollerUIManager.FadeInImage();
    }
}
