using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class BalloonMapManager : MonoBehaviour
{
    public GameObject[] balloonScreens;  // 5개의 벽 오브젝트 배열

    List<GameObject> availableScreens;   // 이벤트 발생하지 않은 벽 리스트


    // [ 이벤트 풍선 ]
    string[] balloonTags = { "red", "yellow", "green", "blue", "pink", "purple" }; // 풍선 색깔에 해당하는 태그 리스트
    Balloon eventBalloon = null;        // 현재 이벤트 풍선 
    float eventBalloonTime = 30f;       // 30초마다 이벤트 발생
    float eventDuration = 5f;           // 5초 안에 이벤트 풍선을 터트려야 함 => 현재 시연 위해 변경되어있음
    float timer = 0f;


    // [ 게임 진행상황 표시 ]
    public Slider balloonSlider;        // 풍선 개수 표시할 슬라이더
    public Text timerText;              // 남은 시간을 표시할 텍스트 UI

    private int totalBalloons = 0;      // 전체 풍선 개수
    private int poppedBalloons = 0;     // 터진 풍선 개수

    private float gameDuration = 90f;   // 1분 30초(90초) 타이머
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
        }

        // 슬라이더 초기화
        balloonSlider.maxValue = totalBalloons;
        balloonSlider.value = 0;


        // 처음에 모든 화면을 사용 가능하도록 설정
        availableScreens = new List<GameObject>(balloonScreens);


        // BGM 재생
        // _balloonSoundManager.PlayBGM();

        // BGM 재생 및 안내 음성 실행
        _balloonSoundManager.PlayBGMWithGuide(); // 안내 음성 기능 추가

    }




    // ★ --------------------------------------- ★
    //
    // 1. [ 타이머 업데이트 ]
    // 2. [ 이벤트 풍선 처리 ]
    // 2-1. 30초마다 이벤트 풍선 선택
    //     - 타이머 리셋
    // 2-2. 이벤트 풍선의 시간 제한 확인
    //     - 시간 초과 시 원래 모습으로 돌아가게 함
    //
    void Update()
    {
        if (!gameEnded)
        {
            gameDuration -= Time.deltaTime;
            UpdateTimerUI();

            if (gameDuration <= 0)
            {
                gameDuration = 0; // 타이머가 음수로 내려가지 않도록 0으로 고정
                GameOver();
            }
            UpdateTimerUI(); // 타이머 UI는 계속 업데이트하여 00:00을 포함한 정확한 시간 표시
        }

        
        timer += Time.deltaTime;

        if (timer >= eventBalloonTime)
        {
            SelectRandomEventBalloon();
            timer = 0f; 
        }

        if (eventBalloon != null && eventBalloon.isEventBalloon)
        {
            eventBalloon.timer += Time.deltaTime;
            if (eventBalloon.timer >= eventDuration)
            {
                ResetBalloon(eventBalloon); 
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

    // ★ 게임 종료 후 메인 씬으로 돌아오는 함수 추가
    void ReturnToMainScene()
    {
        SceneManager.LoadScene("Main"); // 메인 씬 이름을 적절히 변경해주세요
    }

    // ★ 게임 오버 처리 (1분 30초 내에 실패했을 때)
    void GameOver()
    {
        gameEnded = true;
        UpdateTimerUI();  // 타이머를 00:00으로 설정
        Debug.Log("게임 오버! 제한 시간 내에 모든 풍선을 터뜨리지 못했습니다.");
        Invoke("ReturnToMainScene", 3f); // 3초 후 메인 씬으로 이동
    }

    // ★ 게임 클리어 처리 (모든 풍선을 터뜨렸을 때)
    void GameClear()
    {
        gameEnded = true;
        Debug.Log("게임 클리어! 모든 풍선을 터뜨렸습니다.");
        Invoke("ReturnToMainScene", 3f); // 3초 후 메인 씬으로 이동
    }



    // -----------------------------------------------------------------------------------------------
    // ★ 이벤트 풍선 관련 메소드 --------------------------------------------------------------------



    // ★ [ 이벤트 풍선 랜덤 선택 ] ★
    // 
    // 1. 랜덤으로 사용 가능한 벽 선택
    //   - 선택된 벽 (오브젝트)의 자식 풍선들 가져옴 
    // 2. 랜덤 풍선 선택
    //   - 선택된 풍선을 이벤트 풍선으로 설정
    //   - 이벤트 발생한 게임 오브젝트는 리스트에서 제거
    //
    void SelectRandomEventBalloon()
    {
        if (availableScreens.Count == 0)
        {
            Debug.Log("이벤트를 발생시킬 화면이 더 이상 없음.");
            return; 
        }

        int randomScreenIndex = Random.Range(0, availableScreens.Count);
        GameObject selectedScreen = availableScreens[randomScreenIndex];

        Balloon[] balloons = selectedScreen.GetComponentsInChildren<Balloon>();

        if (balloons.Length > 0)
        {
            int randomBalloonIndex = Random.Range(0, balloons.Length);
            eventBalloon = balloons[randomBalloonIndex];

            eventBalloon.isEventBalloon = true;
            eventBalloon.timer = 0f;
            eventBalloon.ChangeToEventBalloonAppearance();

            availableScreens.Remove(selectedScreen);

            Debug.Log("이벤트 풍선이 활성화 되었음.");
        }
        else
        {
            Debug.Log("해당 벽에 풍선이 없음"); // 벽에 풍선이 없다면 => 거의 다 없애 간다는 거니까 이벤트 풍선 발생 X
        }
    }



    // ★ [ 이벤트 풍선 파괴 시 : 랜덤 색상의 풍선 파괴 ] ★
    // 
    // 1. 랜덤 색깔 태그 선택
    // 2. 이벤트 풍선이 속한 벽에서 해당 태그의 풍선들을 찾아 파괴
    // 3. 이벤트 풍선 리셋 적용
    //
    void DestroyRandomColorBalloons(Balloon eventBalloon)
    {
        string randomTag = balloonTags[Random.Range(0, balloonTags.Length)];

        foreach (Transform balloon in eventBalloon.transform.parent)
        {
            if (balloon.CompareTag(randomTag))
            {
                Destroy(balloon.gameObject); 
            }
        }

        Debug.Log("성공 : 이벤트 풍선의 효과로 " + randomTag + "색의 풍선이 터졌다!!");

        ResetBalloon(eventBalloon);
    }


    // ★ [ 이벤트 풍선을 터치했을 때 처리 ]
    // 
    // 1. 풍선 파괴 
    //   - 이벤트 풍선일 때는 랜덤 색깔 풍선 파괴
    //   - 일반 풍선일 때는 단순 파괴 -> 현재는 Balloon 스크립트에서 해줄거임 
    // 2. 슬라이더 업데이트
    //   - 모든 풍선을 터뜨렸는지 확인
    public void OnBalloonPopped(Balloon balloon)
    {
        if (balloon.isEventBalloon)
        {
            DestroyRandomColorBalloons(balloon);
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

        if (poppedBalloons >= totalBalloons)
        {
            GameClear();
        }
    }



    // ★ [ 시간 초과 시 또는 풍선을 터트린 후 : 이벤트 풍선을 원래 상태로 리셋 ]
    // 
    // - 원래 모습으로 되돌림
    // - 해당 게임 오브젝트를 다시 사용 가능하게 리스트에 추가 => 현재는 비활성화 
    // - 이벤트 풍선 리셋
    void ResetBalloon(Balloon balloon)
    {
        Debug.Log("이벤트 풍선이 원래 모습으로 돌아왔음");
        balloon.isEventBalloon = false;
        balloon.ResetAppearance(); 

        // availableScreens.Add(balloon.transform.parent.gameObject);

        eventBalloon = null;
    }
}
