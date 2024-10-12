using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class BalloonMapManager : MonoBehaviour
{
    public GameObject[] balloonScreens;  // 벽 오브젝트 배열
    public GameObject Player;            // 플레이어 오브젝트
    public GameObject eventBalloonPrefab; // 이벤트 풍선 오브젝트 

    [SerializeField] BalloonSceneManager _balloonSceneManager;      // 풍선맵 씬 매니저 

    // [ 이벤트 풍선 ]
    float eventBalloonTime = 12f;       // 12초마다 이벤트 발생
    float timer = 0f;


    // [ 게임 진행상황 표시 ]
    public Slider balloonSlider;        // 풍선 개수 표시할 슬라이더
    public Text timerText;              // 남은 시간을 표시할 텍스트 UI

    private int totalBalloons = 0;      // 전체 풍선 개수
    private int poppedBalloons = 0;     // 터진 풍선 개수

    private float gameDuration = 30f;   // 30초 타이머
    private bool gameStarted = false;   // 게임이 시작되었는지 여부
    private bool gameEnded = false;     // 게임 종료 여부

    // [ 사운드 관련 ]
    [SerializeField] BalloonSoundManager _balloonSoundManager;


    // ------------------------------------------------------------------------------------------------------------


    void Start()
    {
        // 게임 시작 시 전체 풍선 개수 계산
        foreach (GameObject screen in balloonScreens)
        {
            totalBalloons += screen.GetComponentsInChildren<Balloon>().Length;
            print("전체 풍선의 개수 = " + totalBalloons);
        }

        // 슬라이더 초기화
        balloonSlider.maxValue = totalBalloons;
        balloonSlider.value = 0;

        // BGM 재생 및 안내 음성 실행
        _balloonSoundManager.PlayBGMWithGuide(StartGameAfterGuide); // 안내 음성 기능 추가

    }


    // ★ 안내 음성이 끝난 후 호출될 함수: 게임을 시작
    void StartGameAfterGuide()
    {
        Debug.Log("안내 음성이 끝났습니다. 게임을 시작합니다.");
        gameStarted = true; 

        Player.SetActive(true); Debug.Log("플레이어가 활성화 됩니다.");
    }


    // ★ --------------------------------------- ★
    //
    // 1. [ 타이머 업데이트 ] 
    // 2. [ 이벤트 풍선 처리 ]
    // 2-1. 일정 시간마다 이벤트 풍선 선택
    //     - 타이머 리셋
    //
    void Update()
    {
        if (gameStarted && !gameEnded) // ★ 게임이 시작되고 종료되지 않은 상태에서만 실행
        {
            gameDuration -= Time.deltaTime;
            UpdateTimerUI();

            if (gameDuration <= 0)
            {
                gameDuration = 0; // 타이머가 음수로 내려가지 않도록 0으로 고정
                GameOver();
            }

            timer += Time.deltaTime;

            if (timer >= eventBalloonTime)
            {
                SelectRandomEventBalloon();
                timer = 0f;
            }

        }
    }



    // --------------------------------------------------------------------------------------------
    // ★ 게임 진행 사항 관리     -----------------------------------------------------------------


    // [ 타이머 UI 업데이트 ]
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(gameDuration / 60f);
        int seconds = Mathf.FloorToInt(gameDuration % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // ★ 게임 클리어 처리 (모든 풍선을 터뜨렸을 때)
    void GameClear()
    {
        gameEnded = true;
        Debug.Log("게임 클리어! 모든 풍선을 터뜨렸습니다.");
        Invoke("ReturnToMainScene", 3f); 
    }

    // ★ 게임 오버 처리 (제한시간 내에 실패했을 때)
    void GameOver()
    {
        gameEnded = true;
        UpdateTimerUI();  // 타이머를 00:00으로 설정
        Debug.Log("게임 오버! 제한 시간 내에 모든 풍선을 터뜨리지 못했습니다.");
        Invoke("ReturnToMainScene", 3f); 
    }

    // 게임 종료 후 메인 씬으로 돌아가기 (씬 매니저에서 관리)
    void ReturnToMainScene()
    {
        _balloonSceneManager.LoadMainMenuMap();
    }




    // -----------------------------------------------------------------------------------------------
    // ★ 이벤트 풍선 관련 메소드 --------------------------------------------------------------------


    // ★ [ 이벤트 풍선 랜덤 선택 ] ★
    // 
    // 1. 랜덤으로 사용 가능한 벽 선택
    //   - 선택된 벽 (오브젝트)의 자식 풍선들 가져옴 
    // 2. 랜덤 풍선 선택
    // 2-1. 선택된 풍선을 이벤트 풍선으로 설정 (오브젝트 자체를 이벤트 풍선 프리팹으로 대체)
    //      - 기존 풍선 위치 및 회전값 저장
    //      - 기존 풍선 제거
    //      - 이벤트 풍선 프리팹 생성
    //      - 새로 생성된 이벤트 풍선에 추가 작업이 필요한 경우, Balloon 클래스를 참조
    //
    void SelectRandomEventBalloon()
    {
        int randomScreenIndex = Random.Range(0, balloonScreens.Length);
        Balloon[] balloons = balloonScreens[randomScreenIndex].GetComponentsInChildren<Balloon>();

        if (balloons.Length > 0)
        {
            int randomBalloonIndex = Random.Range(0, balloons.Length);
            Balloon selectedBalloon = balloons[randomBalloonIndex];

            Vector3 balloonPosition = selectedBalloon.transform.position;
            Quaternion balloonRotation = selectedBalloon.transform.rotation;

            Destroy(selectedBalloon.gameObject);

            GameObject eventBalloonObject = Instantiate(eventBalloonPrefab, balloonPosition, balloonRotation);

            // Balloon eventBalloon = eventBalloonObject.GetComponent<Balloon>(); // 이벤트 풍선에 라이트 없을 때 코드 
            Balloon eventBalloon = eventBalloonObject.transform.GetChild(0).GetComponent<Balloon>();
            eventBalloon.isEventBalloon = true;

            Debug.Log("이벤트 풍선이 생성되었습니다.");
        }
        else
        {
            Debug.Log("해당 벽에 풍선이 없습니다.");
        }
    }



    // ★ [ 풍선을 터치했을 때 처리 ]
    // 
    // 1. 풍선 파괴 
    //   - 이벤트 풍선일 때 : 시간 8초 추가
    //   - 일반 풍선일 때 : 단순 파괴 -> 현재는 Balloon 스크립트에서 해줄거임 
    // 2. 슬라이더 업데이트
    //   - 모든 풍선을 터뜨렸는지 확인
    public void OnBalloonPopped(Balloon balloon)
    {
        if (balloon.isEventBalloon)
        {
            AddTime();

            _balloonSoundManager.PlaySFX();

            // 이벤트 풍선 SFX 넣기 
        }
        else
        {
            // Destroy(balloon.gameObject);
            _balloonSoundManager.PlaySFX();
        }


        poppedBalloons++;
        balloonSlider.value = poppedBalloons;

        print("남은 풍선 개수 : " + (totalBalloons - poppedBalloons));

        if (poppedBalloons >= totalBalloons)
        {
            GameClear();
        }
    }


    // ★ [ 이벤트 풍선 파괴 시 : 시간 6초 추가 ] ★
    //
    void AddTime()
    {
        gameDuration += 6f;
        print("시간이 6초 추가되었습니다");
    }

}
