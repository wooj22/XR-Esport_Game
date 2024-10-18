using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropGameManager : MonoBehaviour
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
    private bool isRising_1 = false;         // 첫번째 상승 중인지 여부
    private bool isRising_2 = false;
    private bool isPaused = false;           // 상승이 일시 중지 상태인지 여부
    private bool gameEnded = false;          // 게임이 종료되었는지 여부
    private bool hasPlayerEntered = false;   // 플레이어가 구멍에 들어갔는지 확인

    // [ 속도 계산 ]
    private float riseSpeed;  // 카메라 상승 속도


    // 1. 상승 속도 계산 (목표 위치까지 일정 시간에 맞게)
    void Start()
    {
        riseSpeed = (TargetYPosition - InitialRiseAmount) / TotalRiseDuration;
        print("상승 속도 = " + riseSpeed);

        StartCoroutine(GameStart_1());
    }

    void Update()
    {
        
    }

    IEnumerator GameStart_1()
    {
        Debug.Log("게임 시작! 원판 위로 올라오세요.");

        yield return new WaitForSeconds(5f);
        isRising_1 = true; 
    }

}
