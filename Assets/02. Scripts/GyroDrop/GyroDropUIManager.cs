using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroDropUIManager : MonoBehaviour
{
    [SerializeField] Text adviceLabel;
    [SerializeField] Image adviceBackImage;
    [SerializeField] Image gameClearImage;
    [SerializeField] Image gameOverImage;
    [SerializeField] Image levelUpImage;

    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;


    // ���ӽ��� �� ī��Ʈ�ٿ�
    public void StartCountDown(int minute)
    {
        StartCoroutine(StartCountDownCoroutine(minute));
    }

    IEnumerator StartCountDownCoroutine(int startCount)
    {
        // ����
        adviceLabel.text = "������ ����� ������ ���ض�!";
        adviceBackImage.gameObject.SetActive(true);
        adviceLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        // ī��Ʈ�ٿ�
        adviceLabel.text = "";
        for (int i = startCount; i > 0; i--)
        {
            adviceLabel.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        adviceLabel.gameObject.SetActive(false);
        adviceBackImage.gameObject.SetActive(false);
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
    public void GameClearUI()
    {
        gameClearImage.gameObject.SetActive(true);

        StartCoroutine(MoveImage(gameClearImage.GetComponent<RectTransform>()));
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

        StartCoroutine(DeleteUI());
    }

    IEnumerator DeleteUI()
    {
        yield return new WaitForSeconds(3f);

        float fadeCount = 1;

        while (fadeCount > 0.001f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);

            gameClearImage.color = new Color(0, 0, 0, fadeCount);
            gameOverImage.color = new Color(0, 0, 0, fadeCount);

        }
    }


    // ���̵���
    public void FadeInImage()
    {
        StartCoroutine(FadeIn());
    }

    // ���̵�ƿ�
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

    public void StartWarning()
    {
        adviceLabel.text = "��� ������ ���� ������!";
        adviceBackImage.gameObject.SetActive(true);
        adviceLabel.gameObject.SetActive(true);
    }

    public void FinishWarning()
    {
        adviceLabel.gameObject.SetActive(false);
        adviceBackImage.gameObject.SetActive(false);
    }


    public void StartCountDown2()
    {
        StartCoroutine(StartCountDownCoroutine2());
    }

    IEnumerator StartCountDownCoroutine2()
    {
        // ����
        adviceLabel.text = "�����մϴ�!";
        adviceBackImage.gameObject.SetActive(true);
        adviceLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        adviceLabel.gameObject.SetActive(false);
        adviceBackImage.gameObject.SetActive(false);
    }
}

