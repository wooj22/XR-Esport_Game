using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonUIManager : MonoBehaviour
{
    [SerializeField] Image adviceBackImage;
    [SerializeField] GameObject adviceImage;  // 게임 설명
    [SerializeField] Image[] countDownImages; // 카운트다운 숫자 (4,3,2,1)
    [SerializeField] Image clockImage;

    [SerializeField] Image gameSuccessImage;
    [SerializeField] Image gameOverImage;
    [SerializeField] Image levelUpImage;

    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;
    [SerializeField] Image fade_down_sub;


    // 게임시작 전 카운트다운
    public void StartCountDown()
    {
        StartCoroutine(StartCountDownCoroutine());
    }

    IEnumerator StartCountDownCoroutine()
    {
        adviceBackImage.gameObject.SetActive(true);
        adviceImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        adviceImage.gameObject.SetActive(false);

        for (int i = countDownImages.Length - 1; i >= 0; i--)
        {
            countDownImages[i].gameObject.SetActive(true); 
            yield return new WaitForSeconds(1f);           
            countDownImages[i].gameObject.SetActive(false); 
        }
        adviceBackImage.gameObject.SetActive(false);
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

        yield return new WaitForSeconds(1f);

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
    public void GameClearUI()
    {
        gameSuccessImage.gameObject.SetActive(true);

        StartCoroutine(MoveImage(gameSuccessImage.GetComponent<RectTransform>()));
    }

    public void GameOverUI()
    {
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

    // 페이드인
    public void FadeInImage()
    {
        StartCoroutine(FadeIn());
    }

    // 페이드아웃
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
            fade_down_sub.color = new Color(0, 0, 0, fadeCount);
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
            fade_down_sub.color = new Color(0, 0, 0, fadeCount);
        }
    }

    public void ActiveClock()
    {
        clockImage.gameObject.SetActive(true);
    }

    /*
    public void DeactiveClock()
    {
        clockImage.gameObject.SetActive(false);
    }
    */
}
