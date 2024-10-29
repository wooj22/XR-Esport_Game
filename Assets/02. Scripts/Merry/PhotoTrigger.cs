using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTrigger : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMesh photoCount;
    [SerializeField] BoxCollider bc;
    [SerializeField] List<ParticleSystem> vfxLists;
    [SerializeField] MerrySoundManager _merrySoundManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PhotoCounting());
        }
    }

    // ���� ī����
    IEnumerator PhotoCounting()
    {
        bc.enabled = false;
        image.gameObject.SetActive(false);
        _merrySoundManager.PlaySFX("SFX_Particle");

        for (int i = 3; i >=1; i--)
        {
            photoCount.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        photoCount.text = "";

        PlayPhotoParticle();
        yield return new WaitForSeconds(10f);
        image.gameObject.SetActive(true);
        bc.enabled = true;
    }

    // ���� ��ƼŬ ���
    private void PlayPhotoParticle()
    {
        _merrySoundManager.PlaySFX("SFX_Fire");

        for (int i=0; i< vfxLists.Count; i++)
        {
            vfxLists[i].Play();
        }
    }
}
