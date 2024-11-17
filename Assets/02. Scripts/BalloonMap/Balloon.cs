using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    BalloonMapManager balloonMapManager;
    public bool isEventBalloon = false;  // �̺�Ʈ ǳ�� ����
    private Animator animator;
    private GameObject confetti; // �ɰ��� 

    void Start()
    {
        balloonMapManager = FindObjectOfType<BalloonMapManager>();
        animator = GetComponent<Animator>();

        confetti = transform.GetChild(1).gameObject;

        // �ִϸ��̼��� ������ �ð���ŭ ���� �� ����
        float randomDelay = Random.Range(0f, 2f); // 0~2�� ����
        animator.StartPlayback();                 // �Ͻ����� ���·� ����
        Invoke(nameof(StartAnimation), randomDelay);
    }

    void StartAnimation()
    {
        animator.StopPlayback(); // ��� ����
    }

    // -------------------------------------------------------------------------------------
    // �� [ �浹 ���� �޼ҵ� ] �� ----------------------------------------------------------


    // [ ǳ�� - �÷��̾� �ݶ��̴� �浹 ]
    //
    // �ִϸ��̼� ��� �ð� ���� �ߺ� �浹 ���� : �ݶ��̴� ��Ȱ��ȭ
    // 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;

            StartCoroutine(PopBalloon());
        }
    }


    // [ ǳ�� �Ͷ߸��� �ִϸ��̼� ��� -> ���� �ð� �� ǳ�� ���� ]
    //
    //  Animator�� Ʈ���Ÿ� �ɾ� ǳ�� ���� �ִϸ��̼� ���
    //  
    IEnumerator PopBalloon()
    {
        // �̺�Ʈ ǳ�� ���ŵǴ� �ִϸ��̼� ����
        if (isEventBalloon)
        {
            Debug.Log("�̺�Ʈ ǳ���� �浹�߽��ϴ�.");
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
