using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerryManager : MonoBehaviour
{
    [Header("Map Data")]
    [SerializeField] float runningtime;

    [Header("Managers")]    
    [SerializeField] MerryUIManager _merryUIManager;
    [SerializeField] MerrySoundManager _merrySoundManager;
    [SerializeField] MerrySceneManager _merrySceneManager;

    private Coroutine merryop;
    private Coroutine merrytermi;

    private void Start()
    {
        MerryMapStartSetting();
        merryop = StartCoroutine(MerryOeration());
        merrytermi = StartCoroutine(MerryTermination());
    }

    /// 시작 셋팅
    private void MerryMapStartSetting()
    {
        _merrySoundManager.PlayBGM();
        _merryUIManager.FadeInImage();
    }

    /// 운행
    IEnumerator MerryOeration()
    {
        yield return null;
    }

    /// 운행 종료
    IEnumerator MerryTermination()
    {
        yield return new WaitForSeconds(runningtime);
        _merrySoundManager.StopBGM();
        _merryUIManager.FadeOutImage();
        yield return new WaitForSeconds(5f);
        _merrySceneManager.LoadMainMenuMap();
    }

    /// 중간 강제 이동
    public void MerryStop()
    {
        StopCoroutine(merryop);
        StopCoroutine(merrytermi);
        StartCoroutine(MerryStopEnging());
    }

    IEnumerator MerryStopEnging()
    {
        _merrySoundManager.StopBGM();
        _merrySoundManager.PlaySFX("SFX_Open");
        _merryUIManager.FadeOutImage();
        yield return new WaitForSeconds(5f);
        _merrySceneManager.LoadMainMenuMap();
    }
}
