using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    BalloonMapManager balloonMapManager;

    public bool isEventBalloon = false;  // 이벤트 풍선인지 여부
    // public float timer = 0f;             // 이벤트 지속 시간 추적
    public Material eventMaterial;       // 이벤트 풍선으로 바꿀 머터리얼
    private Material originalMaterial;   // 원래의 머터리얼
    private Renderer balloonRenderer;    // 풍선의 Renderer 컴포넌트


    void Start()
    {
        balloonRenderer = GetComponent<Renderer>();
        originalMaterial = balloonRenderer.material; // 시작 시 원래 머터리얼 저장
                                                     // BalloonMapManager를 찾아서 참조
        balloonMapManager = FindObjectOfType<BalloonMapManager>();
    }



    // -------------------------------------------------------------------------------------
    // ★ [ 이벤트 풍선 관련 메소드 ] ★ ---------------------------------------------------


    // [ 이벤트 풍선으로 변경 ]
    public void ChangeToEventBalloonAppearance()
    {
        // 이벤트 풍선으로 외형을 변경하는 로직
        // 예: 색상 변경, 크기 변경 등
        if (balloonRenderer != null && eventMaterial != null)
        {
            balloonRenderer.material = eventMaterial; // 이벤트 풍선 머터리얼 적용
        }
    }

    // [ 원래 모습으로 변경 ]
    public void ResetAppearance()
    {
        // 원래 풍선의 외형으로 복구하는 로직

        if (balloonRenderer != null && originalMaterial != null)
        {
            balloonRenderer.material = originalMaterial; // 원래 머터리얼로 복구
        }
        
    }


    // -------------------------------------------------------------------------------------
    // ★ [ 충돌 관련 메소드 ] ★ ----------------------------------------------------------


    // [ 풍선 - 플레이어 콜라이더 충돌 ]
    //
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("플레이어와 충돌했습니다.");

            StartCoroutine(PopBalloon());
        }
    }


    // [ 풍선 터뜨리는 애니메이션 재생 -> 일정 시간 후 풍선 삭제 ]
    //
    //  Animator에 'Pop' 트리거를 걸어 풍선 터뜨리는 애니메이션 재생
    //  
    IEnumerator PopBalloon()
    {
        print("터지는 애니메이션이 실행됩니다.");
        // GetComponent<Animator>().SetTrigger("Pop"); // 애니메이션 구현 시, 할당 필요

        balloonMapManager.OnBalloonPopped(this);

        yield return new WaitForSeconds(0.7f); 
        Destroy(gameObject);
    }
}
