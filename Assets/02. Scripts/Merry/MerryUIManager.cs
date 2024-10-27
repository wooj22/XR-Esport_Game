using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerryUIManager : MonoBehaviour
{
    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;

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