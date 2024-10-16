using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropGameManager : MonoBehaviour
{
    public GameObject disk;                // 원판
    public GameObject cameraObject;        // 카메라 
    public GameObject[] platformPieces;    // 발판 조각들

    private float rotationSpeed = 10f;     // 회전 속도
    private float riseSpeed;               // 상승 속도

    private float targetYPosition = 525f;  // 카메라의 목표 Y 위치
    private float diskCameraOffset = 53f;  // 원판-카메라 Y축 오프셋

    private float initialRiseAmount = 20f; // 초기 상승 Y값
    private float pauseDuration = 5f;      // 상승 멈추는 시간
    private float totalRiseDuration = 90f; // 최종 상승 시간

    private float lowerPercentage = 0.05f; // 하강 비율 (5%)
    private float clearTimeLimit = 120f;   // 게임 시간 2분 제한

    private bool isRising = false;         // 상승 상태 플래그
    private bool isPaused = false;         // 상승 멈춤 상태 플래그
    private bool gameEnded = false;        // 게임 종료 상태 플래그


    void Start()
    {
        Debug.Log("게임이 시작되니 원판 위로 올라와주세요");
        Invoke("StartRising", 5f); 

        riseSpeed = (targetYPosition - initialRiseAmount) / totalRiseDuration; // 상승 속도 계산

        StartCoroutine(GameTimer());           // 게임 타이머 시작
        StartCoroutine(PlatformHoleRoutine()); // 구멍 생성 루틴 시작
    }


    // [ 상황에 따른 플레이 설정 ]
    // 
    void Update()
    {
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            if (!isPaused)
            {
                float currentY = cameraObject.transform.position.y;
                if (currentY < 50f)
                {
                    MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
                }
                else if (currentY >= 50f && !isPaused)
                {
                    isPaused = true;
                    Invoke("ResumeRising", pauseDuration);
                }
            }
        }
    }

    private void MoveCameraAndDisk(float newY)
    {
        newY = Mathf.Min(newY, targetYPosition);
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
        disk.transform.position = new Vector3(disk.transform.position.x, newY - diskCameraOffset, disk.transform.position.z);
    }

    private void StartRising() => isRising = true;

    private void ResumeRising()
    {
        isPaused = false;
        StartCoroutine(RiseCoroutine());
    }

    private IEnumerator RiseCoroutine()
    {
        while (cameraObject.transform.position.y < targetYPosition && !gameEnded)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y + riseSpeed * Time.deltaTime);
            yield return null;
        }

        if (!gameEnded)
        {
            GameClear(); // 목표 높이에 도달 시 클리어 처리
        }
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

        // 깜빡임
        for (int i = 0; i < 5; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.5f);
        }

        // 구멍 뚫림
        renderer.enabled = false;
        collider.enabled = false;

        yield return new WaitForSeconds(5f); // 5초 유지

        // 구멍 메움
        renderer.enabled = true;
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("구멍을 밟았습니다! 5% 하강합니다.");
            LowerHeight();
        }
    }

    private void LowerHeight()
    {
        float currentY = cameraObject.transform.position.y;
        float lowerY = currentY - (targetYPosition * lowerPercentage);
        MoveCameraAndDisk(Mathf.Max(lowerY, 0)); // 최소 높이 0으로 제한
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(clearTimeLimit);

        if (!gameEnded)
        {
            GameOver(); // 시간 초과 시 게임 오버 처리
        }
    }

    private void GameClear()
    {
        gameEnded = true;
        Debug.Log("게임 클리어! 빠르게 하강합니다.");
        StartCoroutine(FastDrop());
    }

    private IEnumerator FastDrop()
    {
        yield return new WaitForSeconds(5f); // 메시지 출력 시간

        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * 5 * Time.deltaTime); // 빠르게 하강
            yield return null;
        }

        Debug.Log("하강 완료! 축하합니다.");
    }

    private void GameOver()
    {
        gameEnded = true;
        Debug.Log("게임 오버! 현재 높이에서 천천히 하강합니다.");
        StartCoroutine(SlowDrop());
    }

    private IEnumerator SlowDrop()
    {
        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * Time.deltaTime); // 천천히 하강
            yield return null;
        }

        Debug.Log("하강 완료.");
    }

}
