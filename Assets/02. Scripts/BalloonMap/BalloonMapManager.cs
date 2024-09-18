using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonMapManager : MonoBehaviour
{
    /*
    // [ 풍선 생성 및 배치 ]
    public GameObject[] balloonPrefabs;  // 6개 색상의 풍선 프리팹 배열 (각 색에 맞는 태그를 가짐)
    private int totalBalloons = 120;     // 전체 풍선의 개수
    public int balloonsPerScreen = 30;   // 각 화면당 생성할 풍선 수
    public Plane[] planes;               // 4개의 화면을 나타내는 Plane 오브젝트 배열

    // [ 제한 시간, 게임 클리어 조건 ]
    public float timeLimit = 90f;        // 제한 시간 (1분 30초)
    private float timeRemaining;         // 남은 시간
    private bool gameEnded = false;      // 게임이 끝났는지 여부 체크
    public GameObject gameClearUI;       // 게임 클리어 시 보여줄 UI

    // [ 게이지 시스템 ]
    public Slider progressBar;   // 게이지를 나타내는 슬라이더 UI
    // private int totalBalloons;   // 전체 풍선의 개수
    private int poppedBalloons;  // 터트린 풍선의 개수


    void Start()
    {
        // SpawnBalloons();

        timeRemaining = timeLimit;  // 남은 시간을 제한 시간으로 설정

        // totalBalloons = GameObject.FindGameObjectsWithTag("Balloon").Length; // 전체 풍선 개수 설정
        poppedBalloons = 0;   // 터트린 풍선의 개수 초기화
        UpdateProgressBar();  // 초기 게이지 상태 업데이트
    }


    // [ 남은 시간 체크 ]
    //  - 시간 감소 (초 단위)
    //  - 게임 종료 관리: 시간 초과 시 게임 종료 / 모든 풍선을 터트리면 게임 클리어
    //
    void Update()
    {
        if (!gameEnded)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                EndGame(false); 
            }

            if (GameObject.FindGameObjectsWithTag("Balloon").Length == 0)
            {
                EndGame(true); 
            }
        }
    }


    // ★ [ 풍선 -> 각 화면의 Plane 위에 랜덤하게 배치 ] 
    // 
    
    void SpawnBalloons()
    {
        for (int i = 0; i < planes.Length; i++)
        {  // 각 화면 (Plane)마다 반복
            for (int j = 0; j < balloonsPerScreen; j++)
            {  
                // 각 화면당 30개의 풍선 생성
                // 랜덤한 위치를 Plane의 영역 내에서 선택
                Vector3 randomPosition = GetRandomPositionOnPlane(planes[i]);

                // 랜덤한 풍선 프리팹을 선택하여 해당 위치에 생성
                GameObject balloon = Instantiate(balloonPrefabs[Random.Range(0, balloonPrefabs.Length)], randomPosition, Quaternion.identity);
            }
        }
    }
    

    // Plane의 영역 내에서 랜덤한 위치를 반환하는 함수
    
    Vector3 GetRandomPositionOnPlane(Plane plane)
    {
        // Plane의 범위를 기준으로 X, Z 좌표를 무작위로 선택
        float randomX = Random.Range(plane.bounds.min.x, plane.bounds.max.x);
        float randomZ = Random.Range(plane.bounds.min.z, plane.bounds.max.z);

        // Plane의 높이(Y 값)는 일정하므로, 그 값을 유지한 채로 반환
        return new Vector3(randomX, plane.transform.position.y, randomZ);
    }

    Vector3 GetObjectBounds(GameObject obj)
    {
        // 오브젝트에서 Collider 컴포넌트 가져오기
        Collider collider = obj.GetComponent<Collider>();

        if (collider != null)
        {
            // bounds로 Collider의 경계 상자를 얻음
            Bounds bounds = collider.bounds;

            // 경계 상자의 최소, 최대 좌표 출력
            Vector3 min = bounds.min;  // 최소 좌표
            Vector3 max = bounds.max;  // 최대 좌표
            Vector3 center = bounds.center;  // 중심 좌표

            Debug.Log("Min Bounds: " + min);
            Debug.Log("Max Bounds: " + max);
            Debug.Log("Center Bounds: " + center);

            // 경계 상자의 중심 좌표를 반환
            return center;
        }
        else
        {
            Debug.LogError("Collider가 없습니다.");
            return Vector3.zero;  // Collider가 없을 경우 기본값 반환
        }
    }

    void EndGame(bool cleared)
    {
        gameEnded = true;

        if (cleared)
        { 
            gameClearUI.SetActive(true); // 클리어 UI 표시
        }
        else
        {
            // 게임 실패 처리
        }
    }



    // ★ [ 게이지 업데이트 ]
    void UpdateProgressBar()
    {
        progressBar.value = (float)poppedBalloons / totalBalloons; // 주의! (0 ~ 1 사이 값)
    }

    // ★ [ 풍선 터졌을 때 호출 ]
    void OnBalloonPopped()
    {
        poppedBalloons++;
        UpdateProgressBar();
    }

    */
}
