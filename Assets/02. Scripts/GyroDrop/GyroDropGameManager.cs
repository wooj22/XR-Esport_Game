using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroDropGameManager : MonoBehaviour
{
    [SerializeField] GyroDropSceneManager _gyrodropSceneManager;
    [SerializeField] GyroDropUIManager _gyrodropUIManager;
    [SerializeField] GyroDropSoundManager _gyrodropSoundManager;

    // [ 게임 오브젝트 참조 ]
    public GameObject disk;                
    private GameObject cameraObject;        
    public GameObject XRoom;
    public GameObject[] platformPieces;    
    public GameObject Player;
    public GameObject Firework;
    public GameObject[] Lights;

    // [ UI ]
    public Text TimerText;                 
    public Slider HeightSlider;             
    public GameObject ArrowObject1; public GameObject ArrowObject2;
    public Sprite Arrow;            // 화살표 : 기본 시계방향 
    public Sprite Arrow_reverse;    // 시계반대방향 

    private float remainingTime;

    public Material material_gray;                 
    public Material material_green;                 
    public Material material_blue;                 
    public Material material_orange;
    public Material material_red;

    // [ 설정 ]
    private const float TargetYPosition = 525f;  // 카메라 목표 Y 위치
    private const float DiskCameraOffset = 41f;  // 원판과 카메라 간 오프셋
    private const float TotalRiseDuration = 60f; // 전체 상승에 걸리는 시간

    private const float LowerPercentage = 0.05f; // 하강 비율 (5%)
    private const float ClearTimeLimit = 90f;   // 게임 제한 시간 (1분30초)

    // [ 상태 플래그 ]
    private bool isRising = false;
    public bool gameEnded = false;           // 게임이 종료되었는지 여부
    public bool pausedOnce = false;          // 50에서 한 번만 멈추기 위한 플래그
    public bool isCollisionDetected = false;  // 충돌 발생 여부
    public bool isCollisionOngoing = false;
    private bool isCountdown;
    public bool isTrigger_center = false;

    // [ 회전 및 속도 ]
    private float RotationSpeed = 20f;     // 원판 회전 속도
    private int RotationDirection = 1;     // 1: 시계 방향, -1: 반시계 방향
    private float riseSpeed;               // 카메라 상승 속도


    // 4개의 구간에 대한 실행 플래그 배열
    private bool[] levelUpExecuted = new bool[4];
    private readonly float[] thresholds = { 0.2f, 0.4f, 0.6f, 0.8f };



    void Start()
    {
        // ★ 시연 시, 필요 
        cameraObject = GameObject.Find("SpoutCamera"); 
        //cameraObject = GameObject.Find("SpoutCamera_2new"); // 테스트용 

        riseSpeed = (TargetYPosition - 10f) / TotalRiseDuration; // 상승 속도 계산 (목표 위치까지 일정 시간에 맞게)
        print("상승 속도 = " + riseSpeed);

        remainingTime = ClearTimeLimit; 

        _gyrodropSoundManager.PlayBGM(); 
        _gyrodropUIManager.FadeInImage(); 

        StartCoroutine(StartGameGuide());

        StartCoroutine(PlatformHoleRoutine());

        StartCoroutine(ChangeRotationDirectionRoutine()); // Y 좌표가 50 이상일 때부터 회전 방향 변경 루틴 시작

    }


    // ★ 게임 시작
    IEnumerator StartGameGuide()
    {
        yield return new WaitForSeconds(5f);

        _gyrodropUIManager.StartCountDown( );
        yield return new WaitForSeconds(7f);

        _gyrodropUIManager.ShowStartArrows( );
        yield return new WaitForSeconds(4f);

        isRising = true;
        _gyrodropUIManager.HideStartArrows();

        StartCoroutine(RiseCoroutine()); // 상승 : 1차 멈춤 있음 
    }


    void Update()
    {
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * RotationDirection * Time.deltaTime);

            UpdateDisk(); 
            UpdateLevel();

            HeightSlider.value = (cameraObject.transform.position.y / TargetYPosition); 
            UpdateTimerText();                                                          
        }

        if (cameraObject.transform.position.y >= TargetYPosition) 
        {
            RestoreAllPlatformPieces();  
        }

    }

    // 타이머 관리 
    private void UpdateTimerText()
    {
        if (pausedOnce)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;  
                StartCoroutine(GameOver());
            }
            else if (remainingTime <= 10 && !isCountdown)
            {
                isCountdown = true;
                _gyrodropSoundManager.Play_CountDown();
            }
            else
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                TimerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        
    }


    // 상승 관리 
    private IEnumerator RiseCoroutine()
    {
        while (!gameEnded && cameraObject.transform.position.y < TargetYPosition)
        {
            float currentY = cameraObject.transform.position.y;


            if (currentY>=30f && !pausedOnce)
            {
                _gyrodropUIManager.StartCountDown2();

                yield return new WaitForSeconds(3f);

                Player.SetActive(true); Debug.Log("플레이어가 활성화 됩니다.");
                pausedOnce = true; 
            }

            MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
            yield return null;
        }

        // 목표 높이에 도달하면 게임 클리어 처리
        if (!gameEnded) StartCoroutine(GameClear()); 
    }



    // ★ [ 카메라 - 원판 위치 ]  ------------------------------------------------------------------------------------------------
    // 
    private void MoveCameraAndDisk(float newY)
    {
        newY = Mathf.Min(newY, TargetYPosition); // 목표 높이 초과 방지
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
        disk.transform.position = new Vector3(disk.transform.position.x, newY - DiskCameraOffset, disk.transform.position.z);
        XRoom.transform.position = new Vector3(XRoom.transform.position.x, newY, XRoom.transform.position.z); 
    }



    // ----------------------------------------------------------------------------------------------------------------
    // ★ [ 원판 관련 ] -------------------------------------------------------------------------------------

    private IEnumerator PlatformHoleRoutine()
    {
        isCollisionDetected = false; // 루틴 시작 시 충돌 플래그 초기화

        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 40f)  // Y 좌표가 40 이상일 때만 구멍 생성
            {
                int numHoles = GetNumberOfHolesBasedOnHeight(); // 높이에 따라 구멍 개수 결정

                List<GameObject> selectedPieces = new List<GameObject>();
                for (int i = 0; i < numHoles; i++)
                {
                    GameObject piece;
                    do
                    {
                        piece = platformPieces[Random.Range(0, platformPieces.Length)];
                    } while (selectedPieces.Contains(piece)); // 중복 방지

                    selectedPieces.Add(piece);
                }

                foreach (GameObject piece in selectedPieces)
                {
                    StartCoroutine(BlinkPlatform(piece));
                }
            }
            yield return new WaitForSeconds(10f);

            isCollisionDetected = false; // 충돌 플래그 초기화
        }
    }


    // 높이에 따라 구멍 개수 결정
    private int GetNumberOfHolesBasedOnHeight()
    {
        float height = cameraObject.transform.position.y;

        if      (height >= 300f) { return Random.Range(3, 5);  } // 3개 또는 4개
        else if (height >= 100f) { return Random.Range(2, 4); } // 2개 또는 3개
        else                     { return 2; } // 기본 2개
    }


    // 원판 깜빡임 
    private IEnumerator BlinkPlatform(GameObject piece)
    {
        PlatformPiece platformPiece = piece.GetComponent<PlatformPiece>();
        if (platformPiece != null)
        {
            _gyrodropSoundManager.HoleWarining_SFX();
            platformPiece.StartBlinking(0.5f, 5); 
        }

        yield return new WaitForSeconds(8f); 

        piece.GetComponent<Renderer>().enabled = true;   // 원판 복구 
        piece.GetComponent<Collider>().enabled = false;

        if (platformPiece != null)
        {
            platformPiece.DeactivateWarningUI(); // 원판 복구 후 WarningUI 비활성화
        }

    }


    // 원판 색상/속도 변경 
    private void UpdateDisk()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        Material newMaterial = null;
        if (heightPercentage >= 0.8f) { newMaterial = material_red; RotationSpeed = 10f * 2.5f;  }        // 속도 2.5배
        else if (heightPercentage >= 0.6f) { newMaterial = material_orange; RotationSpeed = 10f * 2f; }   // 속도 2배
        else if (heightPercentage >= 0.4f) { newMaterial = material_blue; RotationSpeed = 10f * 1.7f; }   // 속도 1.7배
        else if (heightPercentage >= 0.2f) { newMaterial = material_green; RotationSpeed = 10f * 1.3f; }  // 속도 1.3배
        else { newMaterial = material_gray; }

        if (newMaterial != null)
        {
            foreach (GameObject piece in platformPieces)
            {
                piece.GetComponent<Renderer>().material = newMaterial;
            }
        }

    }


    // 회전 방향 변경 (8~12초 랜덤 간격)
    private IEnumerator ChangeRotationDirectionRoutine()
    {
        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 50f)
            {
                _gyrodropUIManager.UpdateArrowSprites(RotationDirection); 

                _gyrodropUIManager.ShowArrows( );

                yield return new WaitForSeconds(3f); 

                RotationDirection *= -1; // 방향 변경 : 시계 방향 ↔ 시계 반대 방향 전환

                _gyrodropUIManager.HideArrows();
            }

            yield return new WaitForSeconds(Random.Range(8f, 12f));

        }
    }
    

    // ----------------------------------------------------------------------------------------------------------
    // ★ [ 충돌 발생 시 호출되는 함수 ] ★ ---------------------------------------------------------------------

    public void HandleCollision()
    {
        if (!isCollisionDetected)
        {
            isCollisionDetected = true;    // 첫 충돌만 인식되도록 
            Debug.Log("충돌 발생: 하강 시작!");

            _gyrodropSoundManager.Hole_SFX();

            StartCoroutine(LowerHeight()); // 하강 실행
        }
    }


    // ★ 충돌 시, 하강 
    // 
    private IEnumerator LowerHeight()
    {
        float currentY = cameraObject.transform.position.y;
        float targetY = currentY - (TargetYPosition * LowerPercentage);
        targetY = Mathf.Max(targetY, 0); // 최소 높이 제한

        float duration = 1f; // 하강에 걸릴 시간 (1초)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(currentY, targetY, elapsedTime / duration);
            MoveCameraAndDisk(newY);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        MoveCameraAndDisk(targetY); // 마지막 위치로 정확히 이동

        _gyrodropUIManager.FinishWarning(); // PlatformPiece에서 Start 실행 
    }


    // [ 원판 중간에 서 있을 때 "center" 충돌 ] --------------------------------------------------------------------------

    #region 임시 Center 충돌 로직 : 비활성화 감지 X 
    /*
    public void OnPlayerCollidedWithCenter()
    {
        if (!warningDisplayed && !gameEnded)
        {
            warningDisplayed = true;
            isCollisionOngoing = true;
            _gyrodropSoundManager.Hole_SFX();
            _gyrodropUIManager.StartWarning();

            StartCoroutine(WaitAndCheckCollision(5f));
        }
    }

    // 현재 충돌 중인 플레이어가 남아있는지 확인
    public void CheckCollisionStatus(int remainingCollisions)
    {
        if (remainingCollisions == 0)
        {
            isCollisionOngoing = false;
            warningDisplayed = false; // 모든 충돌이 종료되면 초기화
            _gyrodropUIManager.FinishWarning();
        }
    }

    // 5초 대기 후 충돌 여부 확인
    private IEnumerator WaitAndCheckCollision(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (isCollisionOngoing)
        {
            StartCoroutine(ContinuousLowering());
        }

    }

    // 충돌 중일 때 카메라와 원판 하강
    public IEnumerator ContinuousLowering()
    {
        while (isCollisionOngoing && !gameEnded)
        {
            float newY = cameraObject.transform.position.y - (TargetYPosition * LowerPercentage * Time.deltaTime);
            MoveCameraAndDisk(Mathf.Max(newY, 0));

            if (cameraObject.transform.position.y <= 30)
            {
                StartCoroutine(GameOver());
                break;
            }

            yield return null;
        }
    }
    */
    #endregion


    public void CenterDrop()
    {
        if (isCollisionOngoing && !gameEnded)
        {
            StartCoroutine(DropLowering());
        }

    }

    public IEnumerator DropLowering()
    {
        while (isCollisionOngoing && !gameEnded)
        {
            float newY = cameraObject.transform.position.y - (TargetYPosition * LowerPercentage * Time.deltaTime);
            MoveCameraAndDisk(Mathf.Max(newY, 0));

            if (cameraObject.transform.position.y <= 30)
            {
                StartCoroutine(GameOver());
                break;
            }
            yield return null;
        }
    }



    // [ UI ] -------------------------------------------------------------------------------------------------------

    void UpdateLevel()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (heightPercentage >= thresholds[i] && !levelUpExecuted[i])
            {
                levelUpExecuted[i] = true; // 해당 구간 플래그 설정
                // _gyrodropUIManager.LevelUpUI();
                // _gyrodropSoundManager.Play_LevelUp();
            }
        }
    }


    // --------------------------------------------------------------------------------------------------------------
    // ★ [ 게임 클리어/오버 ] ★ -----------------------------------------------------------------------------------

    IEnumerator GameClear() 
    {
        gameEnded = true;
        Firework.SetActive(true);

        _gyrodropSoundManager.Play_GameClear();
        _gyrodropUIManager.FinishWarning();
        _gyrodropUIManager.HideArrows();
        _gyrodropUIManager.GameClearUI();

        yield return new WaitForSeconds(5f);
        _gyrodropSoundManager.Play_DropWarning();

        yield return new WaitForSeconds(8f);
        StartCoroutine(Drop(60));

    }

    IEnumerator GameOver()
    {
        gameEnded = true;
        _gyrodropSoundManager.Play_GameOver();
        _gyrodropUIManager.FinishWarning();
        _gyrodropUIManager.HideArrows();
        _gyrodropUIManager.GameOverUI();

        yield return new WaitForSeconds(5f);
        _gyrodropSoundManager.Play_DropWarning();
        StartCoroutine(TurnOffLight());

        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(6));

    }

    private IEnumerator Drop(float speedMultiplier)
    {
        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * speedMultiplier * Time.deltaTime);
            yield return null;
        }
        Debug.Log("하강 완료.");

        StartCoroutine(ReturnMainMap());
    }

    // 모든 조각 원상복구(보이도록)
    private void RestoreAllPlatformPieces()
    {
        foreach (GameObject piece in platformPieces)
        {
            piece.GetComponent<Renderer>().enabled = true; // 조각을 보이게 함 
        }
    }


    // 메인 맵 복귀
    IEnumerator ReturnMainMap()
    {
        yield return new WaitForSeconds(3f);
        _gyrodropUIManager.FadeOutImage();

        yield return new WaitForSeconds(5f);
        _gyrodropSceneManager.LoadMainMenuMap();
    }

    IEnumerator TurnOffLight()
    {
        foreach (GameObject light in Lights)
        {
            if (light != null) 
            {
                light.SetActive(false);
            }
            yield return new WaitForSeconds(1f); // 1초 간격
        }
    }
}
