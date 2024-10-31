using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    [SerializeField] GyroDropGameManager _gyrodropGameManager;
    [SerializeField] GyroDropUIManager _gyrodropUIManager;
    [SerializeField] GyroDropSoundManager _gyrodropSoundManager;

    private HashSet<GameObject> activeColliders = new HashSet<GameObject>();  // �浹 ���� Player ������Ʈ��
    private float firstCollisionTime = -1f;                                   // ù �浹 �߻� �ð� (-1�� Ÿ�̸Ӱ� ��Ȱ��ȭ�� ����)



    // 1. Ÿ�̸Ӱ� Ȱ��ȭ�� ��� 5�� ��� �� Drop() ���� -> // Drop �� Ÿ�̸� �ʱ�ȭ
    // 2. �� �����Ӹ��� ��Ȱ��ȭ�� ������Ʈ ����
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
            // print("�÷��̾� ���� �浹�߻�");
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

    // 1. ��Ȱ��ȭ�� B ������Ʈ�� ����
    // 2. ��� �浹�� ����� ��� Ÿ�̸� ����
    private void CheckInactiveColliders()
    {
        activeColliders.RemoveWhere(collider => collider == null || !collider.activeSelf);

        if (activeColliders.Count == 0 && _gyrodropGameManager.pausedOnce && !_gyrodropGameManager.gameEnded)
        {
            // print("��� �浹 �����");
            _gyrodropGameManager.isCollisionOngoing = false;
            ResetTimer();
            CheckAndDeactivateObjectCIfNeeded();
        }
    }

    // ù �浹 �߻� ���� ��� (�̹� ��ϵ� ��� ����� ����)
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
        Debug.Log("�ϰ� �����մϴ�. ���Ϳ��� �������");
    }

}
