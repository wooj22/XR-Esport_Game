using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;  // BGM ����� �ҽ�
    [SerializeField] AudioSource sfxSource;  // SFX ����� �ҽ�
    
    [SerializeField] AudioClip bgmClip;      // BGM Ŭ��
    [SerializeField] AudioClip BalloonClip;      // SFX Ŭ�� : �Ϲ�ǳ��
    [SerializeField] AudioClip EventBalloonClip; // �̺�Ʈ ǳ�� 

    // [SerializeField] AudioSource guideSource; // �ȳ� ���� ����� �ҽ� 
    // [SerializeField] AudioClip guideClip;     // �ȳ� ���� Ŭ�� 
    // private float originalBgmVolume;          // BGM ���� ���� ���� ����
    // private System.Action onGuideComplete;    // �ȳ� ������ ���� �� ȣ��� �ݹ� �Լ�


    private void Start()
    {
        PlayBGM();
    }


    // BGM ���
    public void PlayBGM()
    {
        Debug.Log("BGM ����մϴ�.");
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }


    // SFX ��� : �Ϲ�ǳ�� ���� 
    public void Balloon_SFX()
    {
        sfxSource.PlayOneShot(BalloonClip);
    }
    public void EventBalloon_SFX()
    {
        sfxSource.PlayOneShot(EventBalloonClip);
    }



    /*
     
    // BGM ��� �� �ȳ� ���� ����
    public void PlayBGMWithGuide(System.Action guideCompleteCallback)
    {
        // originalBgmVolume = bgmSource.volume; // BGM�� ���� ���� ����
        PlayBGM();
        onGuideComplete = guideCompleteCallback; // �ȳ� ������ ���� �� ������ �ݹ� �Լ� ����
        Invoke("PlayGuide", 3f); // 3�� �� �ȳ� ���� ���
    }

    // �ȳ� ���� ���
    public void PlayGuide()
    {
        Debug.Log("�ȳ� ���� ����.");
        bgmSource.volume = originalBgmVolume * 0.2f; // BGM ���� ���̱�
        guideSource.clip = guideClip;
        guideSource.Play();
        Invoke("RestoreBGMVolume", guideClip.length); // �ȳ� ������ ���� �� BGM ���� ����
    }

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
    */



}
