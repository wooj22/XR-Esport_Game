using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public GyroDropGameManager gameManager;

    // 충돌 중인 플레이어 태그 오브젝트를 저장할 리스트
    private List<GameObject> collidingPlayers = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collidingPlayers.Contains(other.gameObject))
        {
            collidingPlayers.Add(other.gameObject);
            gameManager.OnPlayerCollidedWithCenter();  // 충돌 감지 알림
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && collidingPlayers.Contains(other.gameObject))
        {
            collidingPlayers.Remove(other.gameObject);
            gameManager.CheckCollisionStatus(collidingPlayers.Count);  // 충돌 종료 여부 확인
        }
    }
}
