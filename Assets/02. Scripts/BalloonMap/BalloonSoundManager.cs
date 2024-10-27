using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;  // BGM ����� �ҽ�
    [SerializeField] AudioSource sfxSource;  // SFX ����� �ҽ�
    [SerializeField] AudioSource plusSource; // �ȳ� ���� ����� �ҽ�

    [SerializeField] AudioClip bgmClip;          // BGM Ŭ��
    [SerializeField] AudioClip BalloonClip;      // SFX Ŭ�� : �Ϲ�ǳ��
    [SerializeField] AudioClip EventBalloonClip; // �̺�Ʈ ǳ�� 
    [SerializeField] AudioClip GameClearClip;
    [SerializeField] AudioClip GameOverClip;
    [SerializeField] AudioClip CountDownClip;

    private float originalBgmVolume;          // BGM ���� ���� ���� ����
    
     
    // [SerializeField] AudioClip guideClip;     // �ȳ� ���� Ŭ�� 
    // private System.Action onGuideComplete;    // �ȳ� ������ ���� �� ȣ��� �ݹ� �Լ�


    private void Start()
    {
        originalBgmVolume = bgmSource.volume; // BGM�� ���� ���� ����
        // PlayBGM();
    }


    // BGM ���
    public void PlayBGM()
    {
        Debug.Log("BGM ����մϴ�.");
        bgmSource.clip = bgmClip;  bgmSource.loop = true;
        bgmSource.Play();
    }


    // SFX ��� : �Ϲ�ǳ�� ���� 
    public void Balloon_SFX() { sfxSource.PlayOneShot(BalloonClip); }
    public void EventBalloon_SFX() { sfxSource.PlayOneShot(EventBalloonClip); }


    // [ ���� Ŭ����, ����, 10�� �� -> ���� ���� ���̰� ��� ]
    public void Play_GameClear()
    {
        bgmSource.volume = originalBgmVolume * 0.2f; // BGM ���� ���̱�
        plusSource.clip = GameClearClip;
        plusSource.Play();
    }

    public void Play_GameOver()
    {
        bgmSource.volume = originalBgmVolume * 0.2f; // BGM ���� ���̱�
        plusSource.clip = GameOverClip;
        plusSource.Play();
    }

    public void Play_CountDown()
    {
        bgmSource.volume = originalBgmVolume * 0.3f; // BGM ���� ���̱�
        plusSource.clip = CountDownClip;
        plusSource.Play();
    }



    /*
    // �ȳ� ������ ���� �� BGM ���� ����
    public void RestoreBGMVolume()
    {
        bgmSource.volume = originalBgmVolume;
        Debug.Log("�ȳ� ���� ����. BGM ���� ����.");

        // �ȳ� ������ ���� �� �ݹ� �Լ� ����
        if (onGuideComplete != null)
        {
            onGuideComplete();
        }
    }

    // BGM ��� �� �ȳ� ���� ����
    
    public void PlayBGMWithGuide(System.Action guideCompleteCallback)
    {
        // originalBgmVolume = bgmSource.volume; // BGM�� ���� ���� ����
        PlayBGM();
        onGuideComplete = guideCompleteCallback; // �ȳ� ������ ���� �� ������ �ݹ� �Լ� ����
        Invoke("PlayGuide", 3f); // 3�� �� �ȳ� ���� ���
    }
    
    public void PlayGuide()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM ���� ���̱�
        guideSource.clip = guideClip;
        guideSource.Play();
        Invoke("RestoreBGMVolume", guideClip.length); // �ȳ� ������ ���� �� BGM ���� ����
    }
    */

}
