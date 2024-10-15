using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerSoundManager : MonoBehaviour
{
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip bgmClip;
    [SerializeField] List<AudioClip> sfxClipList;

    /// BGM
    public void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// SFX
    public void PlaySFX(string clipName)
    {
        AudioClip clipToPlay = sfxClipList.Find(clip => clip.name == clipName);
        sfxSource.PlayOneShot(clipToPlay);
    }
}
