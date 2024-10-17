using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] MainSoundManager _mainSoundManager;
    [SerializeField] MainSceneManager _mainSceneManager;

    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;

    public void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void SwitchMap(string sceneName)
    {
        StartCoroutine(FadeOutEndSwitchMap(sceneName));
        Debug.Log("맵 전환 " + sceneName);
    }

    /// 페이드인
    IEnumerator FadeIn()
    {
        _mainSoundManager.PlayBGM();
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

    /// 페이드아웃 후 맵 전환
    IEnumerator FadeOutEndSwitchMap(string sceneName)
    {
        _mainSoundManager.PlaySFX("SFX_Main_OpenMap");
        _mainSoundManager.StopBGM();

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

        yield return new WaitForSeconds(3f);

        _mainSceneManager.OnLoadSceneByName(sceneName);
    }
}
