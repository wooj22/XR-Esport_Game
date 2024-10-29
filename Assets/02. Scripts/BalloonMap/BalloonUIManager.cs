using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonUIManager : MonoBehaviour
{
    [SerializeField] Text adviceLabel;
    [SerializeField] Image adviceBackImage;
    [SerializeField] Image gameSuccessImage;
    [SerializeField] Image gameOverImage;

    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;
    [SerializeField] Image fade_down_sub;


    // ���ӽ��� �� ī��Ʈ�ٿ�
    public void StartCountDown(int minute)
    {
        StartCoroutine(StartCountDownCoroutine(minute));
    }

    IEnumerator StartCountDownCoroutine(int startCount)
    {
        // ����
        adviceLabel.text = "ǳ���� ��Ʈ����!";
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

    /// ���� ���� UI
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
}
