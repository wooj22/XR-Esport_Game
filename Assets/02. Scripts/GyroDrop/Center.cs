using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public GyroDropGameManager gameManager;

    // �浹 ���� �÷��̾� �±� ������Ʈ�� ������ ����Ʈ
    private List<GameObject> collidingPlayers = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collidingPlayers.Contains(other.gameObject))
        {
            collidingPlayers.Add(other.gameObject);
            gameManager.OnPlayerCollidedWithCenter();  // �浹 ���� �˸�
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && collidingPlayers.Contains(other.gameObject))
        {
            collidingPlayers.Remove(other.gameObject);
            gameManager.CheckCollisionStatus(collidingPlayers.Count);  // �浹 ���� ���� Ȯ��

        }
    }

}
