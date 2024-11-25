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
    private Coroutine fillCoroutine;         // 코루틴 제어

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //easterEgg.AddNameList(this.gameObject.name);    // 이스터에그
            easterEgg.TimeInitialization();                   // 이스터에그 타임체크 초기화
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

    /// 맵 이동 게이지 Max, 맵 전환 호출
    private void MaxGuage()
    {
        _mainManager.SwitchMap(this.gameObject.name);  // 맵 전환
    }
}
