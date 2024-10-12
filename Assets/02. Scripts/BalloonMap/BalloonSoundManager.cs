using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;  // BGM 오디오 소스
    [SerializeField] AudioSource sfxSource;  // SFX 오디오 소스
    [SerializeField] AudioSource guideSource; // 안내 음성 오디오 소스 

    [SerializeField] AudioClip bgmClip;      // BGM 클립
    [SerializeField] AudioClip sfxClip;      // SFX 클립
    [SerializeField] AudioClip guideClip;    // 안내 음성 클립 

    private float originalBgmVolume;         // BGM 원래 볼륨 저장 변수
    private System.Action onGuideComplete;   // 안내 음성이 끝난 후 호출될 콜백 함수


    // BGM 재생 및 안내 음성 시작
    public void PlayBGMWithGuide(System.Action guideCompleteCallback)
    {
        originalBgmVolume = bgmSource.volume; // BGM의 원래 볼륨 저장
        PlayBGM();
        onGuideComplete = guideCompleteCallback; // 안내 음성이 끝난 후 실행할 콜백 함수 저장
        Invoke("PlayGuide", 3f); // 3초 후 안내 음성 재생
    }

    // BGM 재생
    public void PlayBGM()
    {
        Debug.Log("BGM 재생합니다.");
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // 안내 음성 재생
    public void PlayGuide()
    {
        Debug.Log("안내 음성 시작.");
        bgmSource.volume = originalBgmVolume * 0.2f; // BGM 볼륨 줄이기
        guideSource.clip = guideClip;
        guideSource.Play();
        Invoke("RestoreBGMVolume", guideClip.length); // 안내 음성이 끝난 후 BGM 볼륨 복구
    }

    // 안내 음성이 끝난 후 BGM 볼륨 복구
    public void RestoreBGMVolume()
    {
        bgmSource.volume = originalBgmVolume;
        Debug.Log("안내 음성 종료. BGM 볼륨 복구.");

        // 안내 음성이 끝난 후 콜백 함수 실행
        if (onGuideComplete != null)
        {
            onGuideComplete();
        }
    }

    // SFX 재생
    public void PlaySFX()
    {
        sfxSource.PlayOneShot(sfxClip);
    }

}
