using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] public int currentLevel;              // 현재 레벨
    [SerializeField] public float currentLaserCycle;       // 현재 레이저 발생 주기
    [SerializeField] public float currentLaserSpeed;       // 현재 레이저 속도

    [Header("MapData")]
    [SerializeField] float gamePlayTime;                   // 전체 게임 진행 시간 120초
    [SerializeField] int maxLaserCount;                    // 전체 레이저 개수 29개
    [SerializeField] Transform laserParent;                // 레이저 생성 위치
    [SerializeField] List<GameObject> laserPrefabList;     // 겹 레이저 프리팹 3개
    [SerializeField] List<float> levelUpTime;              // 레벨 분기 단위
    [SerializeField] List<float> laserCycleList;           // 3레벨 사이클주기 데이터
    [SerializeField] List<float> laserSpeedList;           // 3레벨 스피드 데이터

    [Header("Managers")]
    [SerializeField] CircusUIManager _circusUIManager;
    [SerializeField] CircusSoundManager _circusSoundManager;
    [SerializeField] CircusSceneManager _circusSceneManager;


    private void Start()
    {
        CircusMapStartSetting();
        StartCoroutine(CircusGame());
    }

    /*-------------------- Gaming ----------------------*/
    /// 맵 초기 셋팅
    private void CircusMapStartSetting()
    {
        // 레벨 초기화
        currentLevel = 1;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];

        // BGM 시작
        _circusSoundManager.PlayBGM();

        // 페이드인
        _circusUIManager.FadeInImage();

        // UI 게이지 초기화
        _circusUIManager.GaugeSetting(maxLaserCount * 0.9f);
    }

    /// 게임 진행
    IEnumerator CircusGame()
    {
        // 맵 셋팅 대기
        yield return new WaitForSeconds(5f);

        // UI 5초 카운트다운 후 Timer 시작
        _circusUIManager.StartCountDown(5);
        yield return new WaitForSeconds(8f);
        _circusUIManager.StartTimer(gamePlayTime);
        StartCoroutine(EndCountDownSound(gamePlayTime));

        // 게임 시작
        Coroutine Level = StartCoroutine(LevelControl());
        Coroutine Laser = StartCoroutine(GenerateLasers());
        yield return new WaitForSeconds(gamePlayTime + 2f);

        // 게임 종료
        StopCoroutine(Level);
        StopCoroutine(Laser);

        // 게임 결과 확인
        CheckGameResult();
        yield return new WaitForSeconds(5f);

        // 메인맵 복귀
        StartCoroutine(ReturnMainMap());
    }

    /// 종료 10초 전 카운트다운
    IEnumerator EndCountDownSound(float playTime)
    {
        yield return new WaitForSeconds(playTime - 8f);
        _circusSoundManager.PlaySFX("SFX_10Count");
        yield return new WaitForSeconds(10f);
        _circusSoundManager.StopSFX();
    }

    /// 레벨 컨트롤
    IEnumerator LevelControl()
    {
        // 30초
        yield return new WaitForSeconds(levelUpTime[0]);
        currentLevel++;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];

        // 50초
        yield return new WaitForSeconds(levelUpTime[1]-levelUpTime[0]);
        currentLevel++;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];

        // 40초
        yield return new WaitForSeconds(levelUpTime[2] - levelUpTime[1]);
        currentLevel++;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];
    }

    /// 레이저 생성
    IEnumerator GenerateLasers()
    {
        while (true)
        {
            SpawnLaser();
            yield return new WaitForSeconds(currentLaserCycle);
        }
    }

    private void SpawnLaser()
    {
        if(currentLevel == 1)
        {
            // 1 level
            GameObject Laser = Instantiate(laserPrefabList[0], laserParent.position, laserPrefabList[0].transform.rotation, laserParent);
        }
        else
        {
            // 2, 3 level
            GameObject laserPrefab = laserPrefabList[Random.Range(0, laserPrefabList.Count)];
            GameObject Laser = Instantiate(laserPrefab, laserParent.position, laserPrefab.transform.rotation, laserParent);
        }
    }

    /// 게임 결과 확인
    private void CheckGameResult()
    {
        if (_circusUIManager.GaugeValueCheck())
        {
            // 게임성공
            this.GetComponent<CircusDirector>().PlayFirecracker();
            _circusUIManager.GameSuccessUI();
            _circusSoundManager.PlaySFX("SFX_Circus_GameClear");
            PlayCheerAnimation();
        }
        else
        {
            // 게임실패
            _circusUIManager.GameOverUI();
            _circusSoundManager.PlaySFX("SFX_Circus_GameOver");
        }
    }

    /// 관객 환호 애니메이션
    private void PlayCheerAnimation()
    {
        GameObject[] audiences = GameObject.FindGameObjectsWithTag("Audience");
        foreach (GameObject audience in audiences)
        {
            AudienceController audienceController = audience.GetComponent<AudienceController>();
            audienceController.PlayCheerAnimation();
        }
    }


    /// 메인 맵 복귀
    IEnumerator ReturnMainMap()
    {
        _circusUIManager.FadeOutImage();
        _circusSoundManager.StopBGM();

        yield return new WaitForSeconds(5f);
        _circusSceneManager.LoadMainMenuMap();
    }

    /*-------------------- Event ----------------------*/
    public void OnLaserHitPlayer()
    {
        _circusUIManager.GaugeDown();
        _circusSoundManager.PlaySFX("SFX_Circus_laserOver");
    }

    public void OnLaserReachBorder()
    {
        _circusUIManager.GaugeUp();
    }
}
