using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    [SerializeField] GyroDropGameManager _gyrodropGameManager;
    [SerializeField] GyroDropUIManager _gyrodropUIManager;
    [SerializeField] GyroDropSoundManager _gyrodropSoundManager;

    private HashSet<GameObject> activeColliders = new HashSet<GameObject>();  // 충돌 중인 Player 오브젝트들
    private float firstCollisionTime = -1f;                                   // 첫 충돌 발생 시간 (-1은 타이머가 비활성화된 상태)



    // 1. 타이머가 활성화된 경우 5초 경과 시 Drop() 실행 -> // Drop 후 타이머 초기화
    // 2. 매 프레임마다 비활성화된 오브젝트 제거
    private void Update()
    {
        if (firstCollisionTime >= 0 && Time.time - firstCollisionTime >= 5f)
        {
            Drop();  ResetTimer();  
        }

        CheckInactiveColliders();
    }

    // --------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _gyrodropGameManager.pausedOnce && !_gyrodropGameManager.gameEnded)  
        {
            // print("플레이어 센터 충돌발생");
            _gyrodropGameManager.isCollisionOngoing = true;
            activeColliders.Add(other.gameObject);
            CheckAndActivateObjectC();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _gyrodropGameManager.pausedOnce && !_gyrodropGameManager.gameEnded)  
        {
            activeColliders.Remove(other.gameObject);
            CheckAndDeactivateObjectCIfNeeded();
        }
    }

    // --------------------------------------------------------------------------------------------------------

    // 1. 비활성화된 B 오브젝트를 제거
    // 2. 모든 충돌이 종료된 경우 타이머 리셋
    private void CheckInactiveColliders()
    {
        activeColliders.RemoveWhere(collider => collider == null || !collider.activeSelf);

        if (activeColliders.Count == 0 && _gyrodropGameManager.pausedOnce && !_gyrodropGameManager.gameEnded)
        {
            // print("모든 충돌 사라짐");
            _gyrodropGameManager.isCollisionOngoing = false;
            ResetTimer();
            CheckAndDeactivateObjectCIfNeeded();
        }
    }

    // 첫 충돌 발생 시점 기록 (이미 기록된 경우 덮어쓰지 않음)
    private void CheckAndActivateObjectC()
    {
        if (firstCollisionTime < 0)
        {
            firstCollisionTime = Time.time;
        }

        _gyrodropSoundManager.Hole_SFX();
        _gyrodropUIManager.StartWarning();
    }

    private void CheckAndDeactivateObjectCIfNeeded()
    {
        if (activeColliders.Count == 0)
        {
            _gyrodropUIManager.FinishWarning();
        }
    }

    // --------------------------------------------------------------------------------------------------------

    private void ResetTimer()
    {
        firstCollisionTime = -1f; 
    }

    private void Drop()
    {
        _gyrodropGameManager.CenterDrop();
        Debug.Log("하강 시작합니다. 센터에서 벗어나세요");
    }

}
