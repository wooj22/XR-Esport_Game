using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip bgmClip;
    [SerializeField] AudioClip sfxClip;

    /// BGM
    public void PlayBGM()
    {
        Debug.Log("BGM 재생합니다.");
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    
    /// SFX
    public void PlaySFX()
    {
        sfxSource.PlayOneShot(sfxClip);
    }
    
}
