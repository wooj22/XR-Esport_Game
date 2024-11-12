using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;  // BGM 
    [SerializeField] AudioSource sfxSource;  // SFX : ���ۿ� ������ ��, ���ӿ���, ����Ŭ����, 10�� ī��Ʈ 
    [SerializeField] AudioSource plusSource; // ���� ��, ��� ���� ( �Ҹ��� ���� �ʴ� ���� )

    [SerializeField] AudioClip bgmClip;          // BGM Ŭ��
    [SerializeField] AudioClip HoleClip;         // SFX Ŭ�� : ���� ����� �� 
    [SerializeField] AudioClip HoleWarningClip;         // SFX Ŭ�� : ���� �ո� �� 

    [SerializeField] AudioClip WarningClip;  // ���̷ε�� �������� �� ��� ���� 

    [SerializeField] AudioClip GameClearClip; 
    [SerializeField] AudioClip GameOverClip;
    [SerializeField] AudioClip CountDownClip;
    [SerializeField] AudioClip LevelUpClip;

    private float originalBgmVolume;         
    private float originalSfxVolume;          
    private float originalPlusVolume;


    private void Start()
    {
        originalBgmVolume = bgmSource.volume; // ���� ���� ����
        originalSfxVolume = sfxSource.volume;
        originalPlusVolume = plusSource.volume;
    }


    // BGM ���
    public void PlayBGM()
    {
        Debug.Log("BGM ����մϴ�.");
        bgmSource.clip = bgmClip; bgmSource.loop = true;
        bgmSource.Play();
    }


    // SFX ��� : ���� ����� �� 
    public void Hole_SFX() { sfxSource.PlayOneShot(HoleClip); sfxSource.volume = originalSfxVolume * 1.5f; Invoke("RestoreVolume", HoleClip.length); }

    // SFX ��� : ���� �ո� �� 
    public void HoleWarining_SFX() { sfxSource.PlayOneShot(HoleWarningClip); sfxSource.volume = originalSfxVolume * 1.4f; Invoke("RestoreVolume", HoleWarningClip.length); }


    // [ ���� Ŭ����, ����, 10�� �� -> ���� ���� ���̰� ��� ]
    public void Play_GameClear()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM ���� ���̱�
        sfxSource.clip = GameClearClip;
        sfxSource.Play();
    }


    public void Play_GameOver()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM ���� ���̱�
        sfxSource.clip = GameOverClip;
        sfxSource.Play();
    }


    public void Play_CountDown()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM ���� ���̱�
        sfxSource.clip = CountDownClip;
        sfxSource.Play();
    }

    // ���� ��
    public void Play_LevelUp()
    {
        plusSource.clip = LevelUpClip;
        plusSource.Play();
    }


    // ������ ��� ���� 
    public void Play_DropWarning()
    {
        bgmSource.volume = originalBgmVolume * 0.5f; // BGM ���� ���̱�
        sfxSource.volume = originalSfxVolume * 0.5f; // SFX ���� ���̱�
        plusSource.volume = originalPlusVolume * 3f;

        plusSource.clip = WarningClip;
        plusSource.Play();

        Invoke("RestoreVolume", WarningClip.length); // �ȳ� ������ ���� �� BGM ���� ����
    }


    // ���� ����
    public void RestoreVolume()
    {
        bgmSource.volume = originalBgmVolume;
        sfxSource.volume = originalSfxVolume;
        plusSource.volume = originalPlusVolume;
    }

}
