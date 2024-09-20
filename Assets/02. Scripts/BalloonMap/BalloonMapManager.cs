using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMapManager : MonoBehaviour
{

    // [ 이벤트 풍선 관련 변수 ]

    public GameObject[] balloonScreens;  // 5개의 벽 오브젝트 배열

    List<GameObject> availableScreens;   // 이벤트 발생하지 않은 벽 리스트

    string[] balloonTags = { "red", "yellow", "green", "blue", "pink", "purple" }; // 풍선 색깔에 해당하는 태그 리스트

    Balloon eventBalloon = null;        // 현재 이벤트 풍선 
    float eventBalloonTime = 30f;       // 30초마다 이벤트 발생
    float eventDuration = 20f;          // 5초 안에 이벤트 풍선을 터트려야 함
    float timer = 0f;


    void Start()
    {
        // 처음에 모든 화면을 사용 가능하도록 설정
        availableScreens = new List<GameObject>(balloonScreens);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 30초마다 이벤트 풍선을 선택
        if (timer >= eventBalloonTime)
        {
            SelectRandomEventBalloon();
            timer = 0f; // 타이머 리셋
        }

        // 이벤트 풍선의 시간 제한 확인
        if (eventBalloon != null && eventBalloon.isEventBalloon)
        {
            eventBalloon.timer += Time.deltaTime;
            if (eventBalloon.timer >= eventDuration)
            {
                ResetBalloon(eventBalloon); // 시간 초과 시 원래 모습으로 돌아가게 함
            }
        }
    }


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

        print("이벤트 풍선을 선택 합니다.");

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

            Debug.Log("이벤트 풍선이 활성화 되었습니다.");
        }
        else
        {
            Debug.Log("해당 벽에 풍선이 없습니다."); // 벽에 풍선이 없다면 => 거의 다 없애 간다는 거니까 이벤트 풍선 발생 X
        }
    }



    // ★ [ 이벤트 풍선 파괴 시 : 랜덤 색상의 풍선 파괴 ] ★
    // 
    // 1. 랜덤 색깔 태그 선택
    // 2. 이벤트 풍선이 속한 벽에서 해당 태그의 풍선들을 찾아 파괴
    //
    void DestroyRandomColorBalloons(Balloon eventBalloon)
    {
        string randomTag = balloonTags[Random.Range(0, balloonTags.Length)];

        foreach (Transform balloon in eventBalloon.transform.parent)
        {
            if (balloon.CompareTag(randomTag))
            {
                Destroy(balloon.gameObject); 
                Debug.Log("이벤트 풍선의 효과로 "+ randomTag + "색의 풍선이 터졌다!!");
            }
        }

        // 이벤트 풍선 리셋
        ResetBalloon(eventBalloon);
    }


    // ★ [ 이벤트 풍선을 터치했을 때 처리 ]
    // 
    public void OnBalloonPopped(Balloon balloon)
    {
        if (balloon.isEventBalloon)
        {
            // 이벤트 풍선일 때는 랜덤 색깔 풍선 파괴
            DestroyRandomColorBalloons(balloon);
        }
        else
        {
            // 일반 풍선일 때는 단순 파괴 -> Balloon 스크립트에서 해줄거임 
            // Destroy(balloon.gameObject);
        }
    }


    // ★ [ 시간 초과 시 또는 풍선을 터트린 후 : 이벤트 풍선을 원래 상태로 리셋 ]
    // 
    void ResetBalloon(Balloon balloon)
    {
        balloon.isEventBalloon = false;
        balloon.ResetAppearance(); // 원래 모습으로 되돌림

        // 해당 게임 오브젝트를 다시 사용 가능하게 리스트에 추가 => ?? 왜 추가함?
        // availableScreens.Add(balloon.transform.parent.gameObject);

        eventBalloon = null; // 이벤트 풍선 리셋
    }
}
