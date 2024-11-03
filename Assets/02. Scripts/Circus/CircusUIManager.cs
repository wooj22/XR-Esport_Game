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
    [SerializeField] Image levelUpImage;
    [SerializeField] Image gameSuccessImage;
    [SerializeField] Image gameOverImage;
    [SerializeField] GameObject infoImages;

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
        // 설명 on
        adviceLabel.text = "서커스장의 레이저를 피해라!";
        adviceBackImage.gameObject.SetActive(true);
        adviceLabel.gameObject.SetActive(true);
        infoImages.SetActive(true);

        // 설명 off
        yield return new WaitForSeconds(5f);
        infoImages.SetActive(false);
        adviceLabel.rectTransform.anchoredPosition = Vector3.zero;

        // 카운트다운
        adviceLabel.text = "";
        for (int i = startCount; i > 0; i--)
        {
            adviceLabel.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // 게임 시작
        adviceLabel.text = "Game Start!";
        yield return new WaitForSeconds(2f);
        adviceLabel.gameObject.SetActive(false);
        adviceBackImage.gameObject.SetActive(false);
        timerLabel.gameObject.SetActive(true);
        gaugeBar.gameObject.SetActive(true);
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
        if(gaugeBar.value >= gaugeBar.maxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// 레벨업 UI
    public void LevelUpUI()
    {
        levelUpImage.gameObject.SetActive(true);
        StartCoroutine(FlyingImage(levelUpImage.GetComponent<RectTransform>()));
    }

    IEnumerator FlyingImage(RectTransform rectTransform)
    {
        float downY = -500f;
        float highY = 0f;
        float duration = 0.6f;
        float elapsedTime = 0f;

        // 초기 위치
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, downY);

        // 위로 이동
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(downY, highY, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // 아래로 이동
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(highY, downY, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
            yield return null;
        }

        // 최종 위치
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, downY);
        levelUpImage.gameObject.SetActive(false);
    }

    /// 게임 종료 UI
    public void GameSuccessUI()
    {
        timerLabel.gameObject.SetActive(false);
        gaugeBar.gameObject.SetActive(false);
        gameSuccessImage.gameObject.SetActive(true);

        StartCoroutine(MoveImage(gameSuccessImage.GetComponent<RectTransform>()));
    }

    public void GameOverUI()
    {
        timerLabel.gameObject.SetActive(false);
        gaugeBar.gameObject.SetActive(false);
        gameOverImage.gameObject.SetActive(true);

        StartCoroutine(MoveImage(gameOverImage.GetComponent<RectTransform>()));
    }

    private IEnumerator MoveImage(RectTransform rectTransform)
    {
        float startY = -300f;
        float endY = 0f;
        float duration = 1.5f;
        float elapsedTime = 0f;

        // 초기 위치
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);

        // 이동
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);

            yield return null;
        }

        // 최종 위치
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, endY);
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