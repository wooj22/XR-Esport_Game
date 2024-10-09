using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircusUIManager : MonoBehaviour
{
    [SerializeField] Text timerLabel;
    [SerializeField] Slider gaugeBar;
    [SerializeField] Text adviceLabel;
    [SerializeField] Image adviceBackImage;

    /// 게임시작 전 카운트다운
    public void StartCountDown(int minute)
    {
        StartCoroutine(StartCountDownCoroutine(minute));
    }

    IEnumerator StartCountDownCoroutine(int startCount)
    {
        // 설명
        adviceLabel.text = "레이저를 피해라!";
        adviceBackImage.gameObject.SetActive(true);
        adviceLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        // 카운트다운
        adviceLabel.text = "";
        for (int i = startCount; i > 0; i--)
        {
            adviceLabel.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        adviceLabel.gameObject.SetActive(false);
        adviceBackImage.gameObject.SetActive(false);
    }

    /// 타이머
    public void StartTimer(float time)
    {
        StartCoroutine(TimerCountdown(time));
    }

    IEnumerator TimerCountdown(float playTime)
    {
        while (playTime > 0)
        {
            int minutes = Mathf.FloorToInt(playTime / 60);
            int seconds = Mathf.FloorToInt(playTime % 60);

            timerLabel.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);

            playTime--;
        }

        timerLabel.text = "0:00";
    }


    /// 게이지
    public void GaugeUp()
    {
        gaugeBar.value++;
    }

    public void GaugeDown()
    {
        gaugeBar.value--;
    }

    public void GaugeSetting(float maxValue)
    {
        gaugeBar.maxValue = maxValue;
        gaugeBar.value = 0;
    }
}