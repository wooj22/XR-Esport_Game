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


    /// ���ӽ��� �� ī��Ʈ�ٿ�
    public void StartCountDown(int minute)
    {
        StartCoroutine(StartCountDownCoroutine(minute));
    }

    IEnumerator StartCountDownCoroutine(int startCount)
    {
        // ���� on
        adviceLabel.text = "��Ŀ������ �������� ���ض�!";
        adviceBackImage.gameObject.SetActive(true);
        adviceLabel.gameObject.SetActive(true);
        infoImages.SetActive(true);

        // ���� off
        yield return new WaitForSeconds(5f);
        infoImages.SetActive(false);
        adviceLabel.rectTransform.anchoredPosition = Vector3.zero;

        // ī��Ʈ�ٿ�
        adviceLabel.text = "";
        for (int i = startCount; i > 0; i--)
        {
            adviceLabel.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // ���� ����
        adviceLabel.text = "Game Start!";
        yield return new WaitForSeconds(2f);
        adviceLabel.gameObject.SetActive(false);
        adviceBackImage.gameObject.SetActive(false);
        timerLabel.gameObject.SetActive(true);
        gaugeBar.gameObject.SetActive(true);
    }

    /// Ÿ�̸�
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


    /// ������
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

    /// ������ UI
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

        // �ʱ� ��ġ
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, downY);

        // ���� �̵�
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(downY, highY, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // �Ʒ��� �̵�
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(highY, downY, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
            yield return null;
        }

        // ���� ��ġ
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, downY);
        levelUpImage.gameObject.SetActive(false);
    }

    /// ���� ���� UI
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

        // �ʱ� ��ġ
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);

        // �̵�
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);

            yield return null;
        }

        // ���� ��ġ
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, endY);
    }

    /// ���̵���
    public void FadeInImage()
    {
        StartCoroutine(FadeIn());
    }

    /// ���̵�ƿ�
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