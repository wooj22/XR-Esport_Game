using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroDropGameManager : MonoBehaviour
{
    // [ 게임 오브젝트 참조 ]
    public GameObject disk;                // 원판
    public GameObject cameraObject;        // 카메라 
    public GameObject XRoom;
    public GameObject[] platformPieces;    // 발판 조각들

    // [ UI 요소 ]
    public Text TimerText;                 // 타이머 텍스트
    public Slider HeightSlider;             // 높이 슬라이더
    public Text WarningText;       // 경고 메시지 UI 텍스트

    // 타이머 변수
    private float remainingTime;

    // [ 메터리얼 설정 ]
    public Material material_gray;                 
    public Material material_green;                 
    public Material material_blue;                 
    public Material material_orange;
    public Material material_red;

    // [ 설정 상수 ]
    private const float TargetYPosition = 525f;  // 카메라 목표 Y 위치
    private const float DiskCameraOffset = 53f;  // 원판과 카메라 간 오프셋
    private const float PauseDuration = 5f;      // 상승 멈춤 시간
    private const float TotalRiseDuration = 90f; // 전체 상승에 걸리는 시간

    private const float LowerPercentage = 0.05f; // 하강 비율 (5%)
    private const float ClearTimeLimit = 120f;   // 게임 제한 시간 (2분)

    // [ 상태 플래그 ]
    private bool isRising = false;
    private bool gameEnded = false;           // 게임이 종료되었는지 여부
    private bool pausedOnce = false;          // 50에서 한 번만 멈추기 위한 플래그
    public bool isCollisionDetected = false;  // 충돌 발생 여부
    private bool warningDisplayed = false;  // 경고 메시지 1회 출력 플래그
    private bool isCollisionOngoing = false;


    // [ 회전 및 속도 ]
    private float RotationSpeed = 10f;     // 원판 회전 속도
    private int RotationDirection = 1;     // 1: 시계 방향, -1: 반시계 방향
    private float riseSpeed;               // 카메라 상승 속도

    // [ 사운드 ]
    public AudioSource warningSound; // 경고음 사운드

    void Start()
    {
        Debug.Log("게임 시작! 원판 위로 올라오세요.");

        riseSpeed = (TargetYPosition - 10f) / TotalRiseDuration; // 상승 속도 계산 (목표 위치까지 일정 시간에 맞게)
        print("상승 속도 = " + riseSpeed);

        remainingTime = ClearTimeLimit; // 남은 시간 초기화

        Invoke("StartRising", 5f);    // 5초 후 카메라 1차 상승 시작
        
        StartCoroutine(PlatformHoleRoutine());

        StartCoroutine(ChangeRotationDirectionRoutine()); // Y 좌표가 50 이상일 때부터 회전 방향 변경 루틴 시작

    }

    void Update()
    {
        // 게임 진행 중 회전 처리
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * RotationDirection * Time.deltaTime);

            UpdateDiskMaterial(); // 높이에 따라 메터리얼과 속도 변경

            HeightSlider.value = (cameraObject.transform.position.y / TargetYPosition); // 슬라이더 업데이트 (0에서 1로 비율 계산)
            UpdateTimerText();                                                          // 타이머 텍스트 업데이트
        }

        if (cameraObject.transform.position.y >= TargetYPosition) 
        {
            RestoreAllPlatformPieces();  // 최고 높이에 도달했을 때 : 원판 메우기 
        }

    }


    private void UpdateTimerText()
    {
        if (pausedOnce)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;  // 타이머가 음수로 내려가지 않도록 0으로 고정
                StartCoroutine(GameOver());
            }
            else
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                TimerText.text = $"{minutes:00}:{seconds:00}"; // "MM:SS" 형식으로 텍스트 업데이트
            }
        }
        
    }


    private void StartRising()
    {
        isRising = true; // 상승 시작
        StartCoroutine(RiseCoroutine()); // 카메라 상승 루틴 실행
    }


    private IEnumerator RiseCoroutine()
    {
        while (!gameEnded && cameraObject.transform.position.y < TargetYPosition)
        {
            float currentY = cameraObject.transform.position.y;

            // Y 좌표가 30일 때 멈추고 5초 대기
            if (currentY>=30f && !pausedOnce)
            {
                Debug.Log("카메라 멈춤! 5초 대기 후 재상승.");
                yield return new WaitForSeconds(PauseDuration);

                pausedOnce = true; 
            }

            // 카메라와 원판을 계속 이동
            MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
            yield return null;
        }

        // 목표 높이에 도달하면 게임 클리어 처리
        if (!gameEnded) StartCoroutine(GameClear()); Debug.Log("최고 높이에 도달 =" + cameraObject.transform.position.y);
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


    // ★ 높이에 따라 구멍 개수 결정
    private int GetNumberOfHolesBasedOnHeight()
    {
        float height = cameraObject.transform.position.y;

        if      (height >= 300f) { return Random.Range(2, 4);  } // 2개 또는 3개
        else if (height >= 100f) { return Random.Range(1, 3); } // 1개 또는 2개
        else                     { return 1; } // 기본 1개
    }


    // ★ 원판 깜빡임 함수 호출 (5초 간격)
    private IEnumerator BlinkPlatform(GameObject piece)
    {
        PlatformPiece platformPiece = piece.GetComponent<PlatformPiece>();
        if (platformPiece != null)
        {
            platformPiece.StartBlinking(0.5f, 5); 
        }

        yield return new WaitForSeconds(5f); 

        piece.GetComponent<Renderer>().enabled = true;   // 다시 발판을 보이게 하고 충돌 가능하게 함
        piece.GetComponent<Collider>().enabled = false; 

    }


    // ★ 원판 색상 변경 
    private void UpdateDiskMaterial()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        Material newMaterial = null;
        if (heightPercentage >= 0.8f) { newMaterial = material_red; RotationSpeed = 10f * 2.5f; } // 속도 2.5배
        else if (heightPercentage >= 0.6f) { newMaterial = material_orange; RotationSpeed = 10f * 2f; } // 속도 2배
        else if (heightPercentage >= 0.4f) { newMaterial = material_blue; RotationSpeed = 10f * 1.7f; } // 속도 1.7배
        else if (heightPercentage >= 0.2f) { newMaterial = material_green; RotationSpeed = 10f * 1.3f; } // 속도 1.3배
        else { newMaterial = material_gray; }

        if (newMaterial != null)
        {
            foreach (GameObject piece in platformPieces)
            {
                piece.GetComponent<Renderer>().material = newMaterial;
            }
        }
    }


    // ★ 회전 방향을 8~12초 사이 랜덤 간격으로 변경하는 루틴
    private IEnumerator ChangeRotationDirectionRoutine()
    {
        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 50f)
            {
                RotationDirection *= -1;  
                Debug.Log("회전 방향 변경! 현재 방향: " + (RotationDirection == 1 ? "시계" : "반시계"));
            }

            float randomWaitTime = Random.Range(8f, 12f);  
            yield return new WaitForSeconds(randomWaitTime);
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
    }


    // [ 원판 중간에 서 있을 때 "center" 충돌 ] --------------------------------------------------------------------------

    // 플레이어와 첫 충돌 시 호출
    public void OnPlayerCollidedWithCenter()
    {
        if (!warningDisplayed)
        {
            warningDisplayed = true;
            isCollisionOngoing = true;
            // warningSound.Play();
            // WarningText.text = "5초 내로 원판으로 올라와주세요!";
            print("5초 내로 원판으로 올라와주세요!");
            // WarningText.gameObject.SetActive(true);

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

        // WarningText.gameObject.SetActive(false);
    }

    // 충돌 중일 때 카메라와 원판 하강
    private IEnumerator ContinuousLowering()
    {
        while (isCollisionOngoing && !gameEnded)
        {
            print("센터에 서있으므로 하강합니다.");
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



    // --------------------------------------------------------------------------------------------------------------
    // ★ [ 게임 클리어/오버 ] ★ -----------------------------------------------------------------------------------

    IEnumerator GameClear() 
    {
        gameEnded = true;
        Debug.Log("게임 클리어! 5초 후 빠르게 하강합니다.");

        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(25)); 
    }

    IEnumerator GameOver()
    {
        gameEnded = true; 
        Debug.Log("게임 오버! 5초 후 천천히 하강합니다.");

        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(5)); 
    }

    private IEnumerator Drop(float speedMultiplier)
    {
        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * speedMultiplier * Time.deltaTime);
            yield return null;
        }
        Debug.Log("하강 완료.");
    }

    // ★ 모든 조각 원상복구(보이도록)
    private void RestoreAllPlatformPieces()
    {
        foreach (GameObject piece in platformPieces)
        {
            piece.GetComponent<Renderer>().enabled = true; // 조각을 보이게 함 
        }
    }
}
