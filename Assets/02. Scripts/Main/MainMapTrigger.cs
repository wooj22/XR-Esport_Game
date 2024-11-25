using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapTrigger : MonoBehaviour
{
    [SerializeField] MainManager _mainManager;
    [SerializeField] EasterEgg easterEgg;
    [SerializeField] Image guage;
    [SerializeField] float fillSpeed = 0.2f; 
    private Coroutine fillCoroutine;         // �ڷ�ƾ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //easterEgg.AddNameList(this.gameObject.name);    // �̽��Ϳ���
            easterEgg.TimeInitialization();                   // �̽��Ϳ��� Ÿ��üũ �ʱ�ȭ
            //fillCoroutine = StartCoroutine(FillGauge());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
                fillCoroutine = null;
            }

            guage.fillAmount = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (guage.fillAmount < 1f)
            {
                guage.fillAmount += fillSpeed * Time.deltaTime;
            }

            if (guage.fillAmount >= 1f)
            {
                MaxGuage();
            }
        }
    }

    /// �� �̵� ������ Max, �� ��ȯ ȣ��
    private void MaxGuage()
    {
        _mainManager.SwitchMap(this.gameObject.name);  // �� ��ȯ
    }
}
