using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;  // BGM 오디오 소스
    [SerializeField] AudioSource sfxSource;  // SFX 오디오 소스
    [SerializeField] AudioSource plusSource; // 게임오버, 게임클리어, 10초 카운트 

    [SerializeField] AudioClip bgmClip;          // BGM 클립
    [SerializeField] AudioClip BalloonClip;      // SFX 클립 : 일반풍선
    [SerializeField] AudioClip EventBalloonClip; // 이벤트 풍선 
    [SerializeField] AudioClip GameClearClip;
    [SerializeField] AudioClip GameOverClip;
    [SerializeField] AudioClip CountDownClip;

    private float originalBgmVolume;          // BGM 원래 볼륨 저장 변수
    
     
    // [SerializeField] AudioClip guideClip;     // 안내 음성 클립 
    // private System.Action onGuideComplete;    // 안내 음성이 끝난 후 호출될 콜백 함수


    private void Start()
    {
        originalBgmVolume = bgmSource.volume; // BGM의 원래 볼륨 저장
        // PlayBGM();
    }


    // BGM 재생
    public void PlayBGM()
    {
        Debug.Log("BGM 재생합니다.");
        bgmSource.clip = bgmClip;  bgmSource.loop = true;
        bgmSource.Play();
    }


    // SFX 재생 : 일반풍선 터짐 
    public void Balloon_SFX() { sfxSource.PlayOneShot(BalloonClip); }
    public void EventBalloon_SFX() { sfxSource.PlayOneShot(EventBalloonClip); }


    // [ 게임 클리어, 오버, 10초 전 -> 메인 볼륨 줄이고 재생 ]
    public void Play_GameClear()
    {
        bgmSource.volume = originalBgmVolume * 0.2f; // BGM 볼륨 줄이기
        plusSource.clip = GameClearClip;
        plusSource.Play();
    }

    public void Play_GameOver()
    {
        bgmSource.volume = originalBgmVolume * 0.2f; // BGM 볼륨 줄이기
        plusSource.clip = GameOverClip;
        plusSource.Play();
    }

    public void Play_CountDown()
    {
        bgmSource.volume = originalBgmVolume * 0.3f; // BGM 볼륨 줄이기
        plusSource.clip = CountDownClip;
        plusSource.Play();
    }


}
