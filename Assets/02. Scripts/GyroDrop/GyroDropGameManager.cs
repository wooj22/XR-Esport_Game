using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropGameManager : MonoBehaviour
{
    public GameObject disk; // 원판 오브젝트
    public GameObject cameraObject; // 단순한 카메라 오브젝트
    private float rotationSpeed = 10f; // 회전 속도
    private float riseSpeed; // 상승 속도
    private float targetYPosition = 525f; // 카메라의 목표 Y 위치
    private float diskCameraOffset = 53f; // 원판과 카메라의 Y축 오프셋
    private float initialRiseAmount = 20f; // 초기 상승 Y값
    private float pauseDuration = 5f; // 상승 멈추는 시간
    private float totalRiseDuration = 40f; // 최종 상승 시간

    private bool isRising = false; // 상승 상태 플래그
    private bool isPaused = false; // 상승 멈춤 상태 플래그

    void Start()
    {
        Debug.Log("게임이 시작되니 원판 위로 올라와주세요");
        Invoke("StartRising", 5f); // 5초 후 상승 시작
        riseSpeed = (targetYPosition - initialRiseAmount) / totalRiseDuration; // 상승 속도 계산
    }

    void Update()
    {
        if (isRising)
        {
            // 원판 회전
            disk.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // 카메라와 원판의 Y 위치 업데이트
            if (!isPaused)
            {
                float currentY = cameraObject.transform.position.y;

                // 카메라와 원판이 상승하는 로직
                if (currentY < 50f)
                {
                    // 처음 상승
                    float newY = currentY + riseSpeed * Time.deltaTime;
                    cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
                    disk.transform.position = new Vector3(disk.transform.position.x, newY - diskCameraOffset, disk.transform.position.z);
                }
                else if (currentY >= 50f && !isPaused)
                {
                    // Y가 50에 도달했을 때 상승 멈춤
                    isPaused = true;
                    Invoke("ResumeRising", pauseDuration); // 5초 후 다시 상승 시작
                }
            }
        }
    }

    void StartRising()
    {
        isRising = true; // 상승 시작
    }

    void ResumeRising()
    {
        isPaused = false; // 상승 재개
        StartCoroutine(RiseCoroutine());
    }

    private System.Collections.IEnumerator RiseCoroutine()
    {
        while (cameraObject.transform.position.y < targetYPosition)
        {
            // 카메라와 원판 Y 위치 업데이트
            float currentY = cameraObject.transform.position.y;
            float newY = Mathf.Min(currentY + riseSpeed * Time.deltaTime, targetYPosition);
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
            disk.transform.position = new Vector3(disk.transform.position.x, newY - diskCameraOffset, disk.transform.position.z);
            yield return null;
        }

        Debug.Log("최고 높이에 도달함.");
        isRising = false; // 상승 상태 종료
    }
}
