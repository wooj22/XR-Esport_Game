using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollerUIManager : MonoBehaviour
{
    [SerializeField] Text timerLabel;
    [SerializeField] Slider gaugeBar;
    [SerializeField] Text adviceLabel;
    [SerializeField] Image gameSuccessImage;
    [SerializeField] Image gameOverImage;

    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;

    /// 게임시작 전 카운트다운
    public void StartCountDown(int minute)
    {
        StartCoroutine(StartCountDownCoroutine(minute));
    }

    IEnumerator StartCountDownCoroutine(int startCount)
    {
        // 설명
        adviceLabel.text = "아이템을 밟아라!";
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

    public bool GaugeValueCheck()
    {
        if (gaugeBar.value >= gaugeBar.maxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// 게임 종료 UI
    public void GameSuccessUI()
    {
        timerLabel.gameObject.SetActive(false);
        gaugeBar.gameObject.SetActive(false);
        gameSuccessImage.gameObject.SetActive(true);
    }

    public void GameOverUI()
    {
        timerLabel.gameObject.SetActive(false);
        gaugeBar.gameObject.SetActive(false);
        gameOverImage.gameObject.SetActive(true);
    }

    /// 페이드인
    public void FadeInImage()
    {
        StartCoroutine(FadeIn());
    }

    /// 페이드아웃
    public void FadeOutImage()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float fadeCount = 1;

        while (fadeCount > 0.001f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);

            fade_front.color = new Color(0, 0, 0, fadeCount);
            fade_right.color = new Color(0, 0, 0, fadeCount);
            fade_left.color = new Color(0, 0, 0, fadeCount);
            fade_down.color = new Color(0, 0, 0, fadeCount);
        }
    }

    IEnumerator FadeOut()
    {
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);

            fade_front.color = new Color(0, 0, 0, fadeCount);
            fade_right.color = new Color(0, 0, 0, fadeCount);
            fade_left.color = new Color(0, 0, 0, fadeCount);
            fade_down.color = new Color(0, 0, 0, fadeCount);
        }
    }
}
