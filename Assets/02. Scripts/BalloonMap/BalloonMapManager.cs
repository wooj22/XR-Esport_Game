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

    // [ 게임 오버 시, 풍선 떨어짐 관련 ] 
    private string BalloonTag = "Balloon";
    private string eventBalloonTag = "EventBalloon"; 
    public GameObject Barricade1;  
    public GameObject Barricade2;

    // [ 게임 클리어 시, 인형 떨어짐 관련 ]
    public GameObject Doll;
    private List<GameObject> dollObjects = new List<GameObject>(); // 자식 오브젝트 리스트

    // ------------------------------------------------------------------------------------------------------------


    void Start()
    {
        // 게임 시작 시 전체 풍선 개수 계산
        foreach (GameObject screen in balloonScreens)
        {
            totalBalloons += screen.GetComponentsInChildren<Balloon>().Length;
            
        }
        print("전체 풍선의 개수 = " + totalBalloons);

        // 슬라이더 초기화
        balloonSlider.maxValue = totalBalloons;
        balloonSlider.value = 0;

        // BGM 재생 및 안내 음성 실행
        _balloonSoundManager.PlayBGMWithGuide(StartGameAfterGuide); // 안내 음성 기능 추가

        // 게임 클리어 시, 인형들 할당 
        int childCount = Doll.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = Doll.transform.GetChild(i).gameObject;
            dollObjects.Add(child); // 리스트에 추가
        }

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
        if (gameStarted && !gameEnded) // ★ 게임이 시작되고 종료되지 않은 상태에서만
        {
            UpdateGameTimer();
            CheckEventBalloonTimer();
        }

    }

    void UpdateGameTimer()
    {
        gameDuration -= Time.deltaTime;
        UpdateTimerUI();

        if (gameDuration <= 0)
        {
            gameDuration = 0; // 타이머가 음수로 내려가지 않도록 0으로 고정
            GameOver();
        }
    }

    void CheckEventBalloonTimer()
    {
        timer += Time.deltaTime;
        if (timer >= eventBalloonTime)
        {
            SelectRandomEventBalloon();
            timer = 0f;
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

        DropDall();

        Invoke("ReturnToMainScene", 5f); 
    }

    // ★ 게임 오버 처리 (제한시간 내에 실패했을 때)
    void GameOver()
    {
        gameEnded = true;
        UpdateTimerUI();  // 타이머를 00:00으로 설정
        Debug.Log("게임 오버! 제한 시간 내에 모든 풍선을 터뜨리지 못했습니다.");

        DropBalloon();

        Invoke("ReturnToMainScene", 5f); 
    }

    // 게임 종료 후 메인 씬으로 돌아가기 (씬 매니저에서 관리)
    void ReturnToMainScene()
    {
        _balloonSceneManager.LoadMainMenuMap();
    }




    // -----------------------------------------------------------------------------------------------
    // ★ 이벤트 풍선 관련 메소드 --------------------------------------------------------------------


    // ★ [ 이벤트 풍선 랜덤 선택 ] ★ : 특정 벽에 풍선이 없다면 다른 벽을 시도하도록 수정됨 
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
        /*
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
        */

        List<Balloon> availableBalloons = new List<Balloon>();

        foreach (GameObject screen in balloonScreens)
        {
            availableBalloons.AddRange(screen.GetComponentsInChildren<Balloon>());
        }

        if (availableBalloons.Count > 0)
        {
            int randomIndex = Random.Range(0, availableBalloons.Count);
            Balloon selectedBalloon = availableBalloons[randomIndex];

            Vector3 balloonPosition = selectedBalloon.transform.position;
            Quaternion balloonRotation = selectedBalloon.transform.rotation;

            Destroy(selectedBalloon.gameObject);

            GameObject eventBalloonObject = Instantiate(eventBalloonPrefab, balloonPosition, balloonRotation);
            Balloon eventBalloon = eventBalloonObject.transform.GetChild(0).GetComponent<Balloon>();
            eventBalloon.isEventBalloon = true;

            Debug.Log("이벤트 풍선이 생성되었습니다.");
        }
        else
        {
            Debug.Log("모든 벽에 풍선이 없습니다.");
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



    // [ 게임 오버 시 : 풍선이 떨어짐 ]
    //
    // 1. 이벤트 풍선은 모두 제거 
    // 2. 중력 할당 : 초기 회전력과 랜덤 힘 추가 
    // 3. 콜라이더 is trigger 해제 
    // 4. 풍선이 화면 벗어남 방지 위해, 바리게이트 활성화 
    // 
    public void DropBalloon()
    {
        RemoveEventBalloons();

        GameObject[] objects = GameObject.FindGameObjectsWithTag(BalloonTag);
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = obj.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 0.5f;
                rb.drag = 0.2f;
                rb.angularDrag = 0.1f;

                Vector3 randomTorque = new Vector3( Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f) );
                rb.AddTorque(randomTorque, ForceMode.Impulse);

                Vector3 randomForce = new Vector3( Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f) );
                rb.AddForce(randomForce, ForceMode.Impulse);

                // rb.constraints = RigidbodyConstraints.None; // 필요 시 회전 제약 설정
            }

            Collider[] colliders = obj.GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.isTrigger = false;  // isTrigger 해제
            }
        }

        if (Barricade1 != null) Barricade1.SetActive(true);
        if (Barricade2 != null) Barricade2.SetActive(true);
    }

    // 'EventBalloon' 태그를 가진 모든 오브젝트 제거
    public void RemoveEventBalloons()
    {
        GameObject[] eventBalloons = GameObject.FindGameObjectsWithTag(eventBalloonTag);

        foreach (GameObject balloon in eventBalloons)
        {
            Destroy(balloon);  
        }
    }


    // [ 게임 클리어 시 : 인형이 떨어짐 ]
    // 
    public void DropDall()
    {
        // 혹시 모르니 모든 풍선을 끌까...?

        // 중력 강화 (주의, 모든 Rigidbody에 영향)
        // Physics.gravity = new Vector3(0, -20f, 0);  // 기본 중력은 -9.81f

        if (Barricade1 != null) Barricade1.SetActive(true);
        if (Barricade2 != null) Barricade2.SetActive(true);

        // 오브젝트 활성화 및 랜덤한 힘과 회전 적용
        foreach (GameObject obj in dollObjects)
        {
            obj.SetActive(true);

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // 중력 활성화
                rb.useGravity = true;
                rb.mass = 0.2f;
                rb.drag = 0.05f;
                rb.angularDrag = 0.05f;

                Vector3 randomTorque = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                rb.AddTorque(randomTorque, ForceMode.Impulse);

                Vector3 randomForce = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-6f, -10f), Random.Range(-0.5f, 0.5f));
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
        }
    }
}
