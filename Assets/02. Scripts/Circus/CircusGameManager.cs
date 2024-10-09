using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] float currentLaserCycle;      // 현재의 레이저 발생 주기
    [SerializeField] float currentLaserSpeed;      // 현재의 레이저 속도

    [Header("MapData")]
    [SerializeField] float gamePlayTime;           // 전체 게임 진행 시간 120초
    [SerializeField] int maxLaserCount;            // 전체 레이저 개수 29개
    [SerializeField] List<GameObject> laserPrefabList;

    [Header("Managers")]
    [SerializeField] CircusUIManager _circusUIManager;
    [SerializeField] CircusSoundManager _circusSoundManager;
    [SerializeField] CircusSceneManager _circusSceneManager;

    private void Start()
    {
        _circusSoundManager.PlayBGM();
        _circusUIManager.StartCountDown(5);
        _circusUIManager.StartTimer(gamePlayTime);
        _circusUIManager.GaugeSetting(5);
    }
}
