using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerrySoundManager : MonoBehaviour
{
    [SerializeField] public AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip bgmClip;
    [SerializeField] List<AudioClip> sfxClipList;
    [SerializeField] float fadeVolumeTime = 5f;

    /// BGM
    public void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = 0f;
        bgmSource.Play();
        StartCoroutine(FadeInVolume());
    }

    public void StopBGM()
    {
        StartCoroutine(FadeOutVolume());
    }

    /// SFX
    public void PlaySFX(string clipName)
    {
        AudioClip clipToPlay = sfxClipList.Find(clip => clip.name == clipName);
        sfxSource.PlayOneShot(clipToPlay);
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    /// ���� ���̵���
    private IEnumerator FadeInVolume()
    {
        float targetVolume = 1f;
        float currentTime = 0f;

        while (currentTime < fadeVolumeTime)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / fadeVolumeTime);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    /// ���� ���̵�ƿ�
    private IEnumerator FadeOutVolume()
    {
        float startVolume = bgmSource.volume;
        float currentTime = 0f;

        while (currentTime < fadeVolumeTime)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeVolumeTime);
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop();
    }
}
