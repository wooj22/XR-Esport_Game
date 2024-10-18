using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDrop_unused : MonoBehaviour
{
    // [ 게임 오브젝트 참조 ]
    public GameObject disk;                // 원판
    public GameObject cameraObject;        // 카메라 
    public GameObject[] platformPieces;    // 발판 조각들

    // [ 설정 상수 ]
    private const float RotationSpeed = 10f;     // 원판 회전 속도
    private const float TargetYPosition = 525f;  // 카메라 목표 Y 위치
    private const float DiskCameraOffset = 53f;  // 원판과 카메라 간 오프셋
    private const float InitialRiseAmount = 10f; // 초기 카메라 상승 Y값
    private const float PauseDuration = 5f;      // 상승 멈춤 시간
    private const float TotalRiseDuration = 90f; // 전체 상승에 걸리는 시간
    private const float LowerPercentage = 0.05f; // 하강 비율 (5%)
    private const float ClearTimeLimit = 120f;   // 게임 제한 시간 (2분)

    // [ 상태 플래그 ]
    private bool isRising = false;           // 카메라가 상승 중인지 여부
    private bool isPaused = false;           // 상승이 일시 중지 상태인지 여부
    private bool gameEnded = false;          // 게임이 종료되었는지 여부
    private bool hasPlayerEntered = false;   // 플레이어가 구멍에 들어갔는지 확인

    // [ 속도 계산 ]
    private float riseSpeed;  // 카메라 상승 속도


    void Start()
    {
        Debug.Log("게임 시작! 원판 위로 올라오세요.");

        // 상승 속도 계산 (목표 위치까지 일정 시간에 맞게)
        riseSpeed = (TargetYPosition - InitialRiseAmount) / TotalRiseDuration;
        print("상승 속도 = " + riseSpeed);

        // 5초 후 카메라 상승 시작
        Invoke("StartRising", 5f); // 1

        // 타이머와 발판 구멍 생성 루틴 시작
        StartCoroutine(GameTimer()); // 2

        StartCoroutine(PlatformHoleRoutine()); // 3
    }

    void Update()
    {
        // 원판 회전 (게임이 끝나지 않고 상승 중일 때)
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
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
            if (!isPaused) // 멈추지 않은 경우에만 상승
            {
                float currentY = cameraObject.transform.position.y;
                MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);

                // 50 높이에 도달하면 잠시 멈춤
                if (currentY >= 50f && !isPaused)
                {
                    isPaused = true; // 일시 멈춤 플래그
                    Debug.Log("카메라 멈춤! 5초 대기 후 재상승.");

                    yield return new WaitForSeconds(PauseDuration); // 멈춘 후 대기
                    isPaused = false; // 다시 상승 시작
                }
            }
            yield return null;
        }

        // 목표 높이에 도달하면 게임 클리어 처리
        if (!gameEnded) GameClear();
    }

    private void MoveCameraAndDisk(float newY)
    {
        // 카메라와 원판 위치 조정
        newY = Mathf.Min(newY, TargetYPosition); // 목표 높이 초과 방지
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
        disk.transform.position = new Vector3(disk.transform.position.x, newY - DiskCameraOffset, disk.transform.position.z);
    }

    private IEnumerator PlatformHoleRoutine()
    {
        while (!gameEnded)
        {
            GameObject selectedPiece = platformPieces[Random.Range(0, platformPieces.Length)];
            StartCoroutine(BlinkPlatform(selectedPiece));
            yield return new WaitForSeconds(10f); // 다음 구멍까지 대기
        }
    }

    private IEnumerator BlinkPlatform(GameObject piece)
    {
        Renderer renderer = piece.GetComponent<Renderer>();
        Collider collider = piece.GetComponent<Collider>();

        // 발판 깜빡임 (5회 반복)
        for (int i = 0; i < 5; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.5f);
        }

        // 구멍 생성
        renderer.enabled = false;
        collider.enabled = false;

        // 5초 후 복구
        yield return new WaitForSeconds(5f);
        renderer.enabled = true;
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayerEntered)
        {
            hasPlayerEntered = true; // 한 번만 처리
            Debug.Log("구멍 밟음! 5% 하강합니다.");
            LowerHeight();
        }
    }

    private void LowerHeight()
    {
        float currentY = cameraObject.transform.position.y;
        float lowerY = currentY - (TargetYPosition * LowerPercentage);
        MoveCameraAndDisk(Mathf.Max(lowerY, 0)); // 최소 높이 제한
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(ClearTimeLimit);

        if (!gameEnded) GameOver(); // 시간 초과 시 게임 오버
    }

    private void GameClear()
    {
        gameEnded = true; // 게임 종료
        Debug.Log("게임 클리어! 빠르게 하강합니다.");
        StartCoroutine(Drop(20)); // 빠르게 하강
    }

    private void GameOver()
    {
        gameEnded = true; // 게임 종료
        Debug.Log("게임 오버! 천천히 하강합니다.");
        StartCoroutine(Drop(1)); // 천천히 하강
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
}
