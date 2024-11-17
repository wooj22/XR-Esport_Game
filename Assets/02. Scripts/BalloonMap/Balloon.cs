using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    BalloonMapManager balloonMapManager;
    public bool isEventBalloon = false;  // 이벤트 풍선 여부
    private Animator animator;
    private GameObject confetti; // 꽃가루 

    void Start()
    {
        balloonMapManager = FindObjectOfType<BalloonMapManager>();
        animator = GetComponent<Animator>();

        confetti = transform.GetChild(1).gameObject;

        // 애니메이션을 랜덤한 시간만큼 지연 후 시작
        float randomDelay = Random.Range(0f, 2f); // 0~2초 지연
        animator.StartPlayback();                 // 일시정지 상태로 시작
        Invoke(nameof(StartAnimation), randomDelay);
    }

    void StartAnimation()
    {
        animator.StopPlayback(); // 재생 시작
    }

    // -------------------------------------------------------------------------------------
    // ★ [ 충돌 관련 메소드 ] ★ ----------------------------------------------------------


    // [ 풍선 - 플레이어 콜라이더 충돌 ]
    //
    // 애니메이션 재생 시간 동안 중복 충돌 방지 : 콜라이더 비활성화
    // 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;

            StartCoroutine(PopBalloon());
        }
    }


    // [ 풍선 터뜨리는 애니메이션 재생 -> 일정 시간 후 풍선 삭제 ]
    //
    //  Animator에 트리거를 걸어 풍선 제거 애니메이션 재생
    //  
    IEnumerator PopBalloon()
    {
        // 이벤트 풍선 제거되는 애니메이션 실행
        if (isEventBalloon)
        {
            Debug.Log("이벤트 풍선과 충돌했습니다.");
            animator.SetTrigger("Destroy"); 
        }
        else
        {
            animator.SetTrigger("Pop");
            confetti.SetActive(true);
        }

        balloonMapManager.OnBalloonPopped(this);

        yield return new WaitForSeconds(0.4f); 
        Destroy(gameObject);
    }
}
