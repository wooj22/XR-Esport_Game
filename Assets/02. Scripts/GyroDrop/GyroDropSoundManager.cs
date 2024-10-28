using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;  // BGM 
    [SerializeField] AudioSource sfxSource;  // SFX : 구멍에 빠졌을 때, 게임오버, 게임클리어, 10초 카운트 
    [SerializeField] AudioSource plusSource; // 레벨 업, 경고 음성 ( 소리가 줄지 않는 사운드 )

    [SerializeField] AudioClip bgmClip;          // BGM 클립
    [SerializeField] AudioClip HoleClip;         // SFX 클립 : 구멍 밟았을 때 

    [SerializeField] AudioClip WarningClip;  // 자이로드롭 떨어지기 전 경고 음성 

    [SerializeField] AudioClip GameClearClip; 
    [SerializeField] AudioClip GameOverClip;
    [SerializeField] AudioClip CountDownClip;
    [SerializeField] AudioClip LevelUpClip;

    private float originalBgmVolume;         
    private float originalSfxVolume;          
    private float originalPlusVolume;


    private void Start()
    {
        originalBgmVolume = bgmSource.volume; // 원래 볼륨 저장
        originalSfxVolume = sfxSource.volume;
        originalPlusVolume = plusSource.volume;
    }


    // BGM 재생
    public void PlayBGM()
    {
        Debug.Log("BGM 재생합니다.");
        bgmSource.clip = bgmClip; bgmSource.loop = true;
        bgmSource.Play();
    }


    // SFX 재생 : 구멍 밟았을 때 
    public void Hole_SFX() { sfxSource.PlayOneShot(HoleClip); sfxSource.volume = originalSfxVolume * 1.5f; Invoke("RestoreVolume", HoleClip.length); }


    // [ 게임 클리어, 오버, 10초 전 -> 메인 볼륨 줄이고 재생 ]
    public void Play_GameClear()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM 볼륨 줄이기
        sfxSource.clip = GameClearClip;
        sfxSource.Play();
    }


    public void Play_GameOver()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM 볼륨 줄이기
        sfxSource.clip = GameOverClip;
        sfxSource.Play();
    }


    public void Play_CountDown()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM 볼륨 줄이기
        sfxSource.clip = CountDownClip;
        sfxSource.Play();
    }

    // 레벨 업
    public void Play_LevelUp()
    {
        plusSource.clip = LevelUpClip;
        plusSource.Play();
    }


    // 떨어짐 경고 음성 
    public void Play_DropWarning()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM 볼륨 줄이기
        sfxSource.volume = originalSfxVolume * 0.5f; // SFX 볼륨 줄이기
        plusSource.volume = originalPlusVolume * 3f;

        plusSource.clip = WarningClip;
        plusSource.Play();

        Invoke("RestoreVolume", WarningClip.length); // 안내 음성이 끝난 후 BGM 볼륨 복구
    }


    // 볼륨 복구
    public void RestoreVolume()
    {
        bgmSource.volume = originalBgmVolume;
        sfxSource.volume = originalSfxVolume;
        plusSource.volume = originalPlusVolume;
    }


    /*
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

    // BGM 재생 및 안내 음성 시작
    
    public void PlayBGMWithGuide(System.Action guideCompleteCallback)
    {
        // originalBgmVolume = bgmSource.volume; // BGM의 원래 볼륨 저장
        PlayBGM();
        onGuideComplete = guideCompleteCallback; // 안내 음성이 끝난 후 실행할 콜백 함수 저장
        Invoke("PlayGuide", 3f); // 3초 후 안내 음성 재생
    }
    
    public void PlayGuide()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM 볼륨 줄이기
        guideSource.clip = guideClip;
        guideSource.Play();
        Invoke("RestoreBGMVolume", guideClip.length); // 안내 음성이 끝난 후 BGM 볼륨 복구
    }
    */
}
