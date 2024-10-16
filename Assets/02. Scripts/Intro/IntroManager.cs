using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntroManager : MonoBehaviour
{
    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;

    [Header("Managers")]
    [SerializeField] IntroSoundManager _introSoundManager;
    [SerializeField] IntroSceneManager _introSceneManager;

    private void Start()
    {
        _introSoundManager.PlayBGM();
    }

    public void StartMidnightCarnival()
    {
        Debug.Log("인트로 시작");
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);

            fade_front.color = new Color(0, 0, 0, fadeCount);
            fade_right.color = new Color(0, 0, 0, fadeCount);
            fade_left.color = new Color(0, 0, 0, fadeCount);
            fade_down.color = new Color(0, 0, 0, fadeCount);
        }

        yield return new WaitForSeconds(3f);
        _introSceneManager.LoadMainMenuMap();
    }
}
