using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] List<AudioClip> bgmClipList;
    [SerializeField] List<AudioClip> sfxClipList;

    /// BGM
    public void PlayBGM(int index)
    {
        Debug.Log(index);
        bgmSource.clip = bgmClipList[index];
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
