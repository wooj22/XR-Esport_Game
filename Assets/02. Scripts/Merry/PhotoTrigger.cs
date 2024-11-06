using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PhotoTrigger : MonoBehaviour
{
    [Header("Trriger")]
    [SerializeField] float duration;
    [SerializeField] Image image;
    [SerializeField] TextMesh photoCount;
    [SerializeField] BoxCollider bc;

    [Header("Effects")]
    [SerializeField] List<ParticleSystem> vfxLists;

    [Header("Animations")]
    [SerializeField] Animator merryAni;
    [SerializeField] List<Animator> elseAnis;

    [Header("Rendering")]
    [SerializeField] Volume volume;
    [SerializeField] float speed;

    [Header("Managers")]
    [SerializeField] MerrySoundManager _merrySoundManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PhotoCounting());
        }
    }

    // ���� Ʈ���� ī����
    IEnumerator PhotoCounting()
    {
        bc.enabled = false;
        image.gameObject.SetActive(false);
        _merrySoundManager.PlaySFX("SFX_MerryOn");

        for (int i = 3; i >=1; i--)
        {
            photoCount.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        photoCount.text = "";

        // ���ͷ���
        StartCoroutine(PlayPhotoParticle());
        StartCoroutine(PlayPhotoAni());
        StartCoroutine(PostContol());

        yield return new WaitForSeconds(duration);
        image.gameObject.SetActive(true);
        bc.enabled = true;
    }

    // ���� ��ƼŬ ���
    IEnumerator PlayPhotoParticle()
    {
        _merrySoundManager.PlaySFX("SFX_MerryEffect");

        for (int i=0; i< vfxLists.Count; i++)
        {
            vfxLists[i].Play();
        }

        yield return new WaitForSeconds(duration);
        for (int i = 0; i < vfxLists.Count; i++)
        {
            vfxLists[i].Stop();
        }
    }

    // ���� �ִϸ��̼� ���
    IEnumerator PlayPhotoAni()
    {
        merryAni.SetBool("isMerry", true);
        for(int i=0; i< elseAnis.Count; i++)
        {
            elseAnis[i].SetBool("isMerry", true);
        }

        yield return new WaitForSeconds(duration);
        merryAni.SetBool("isMerry", false);
        for (int i = 0; i < elseAnis.Count; i++)
        {
            elseAnis[i].SetBool("isMerry", false);
        }
    }

    // ����Ʈ���μ��� ����
    IEnumerator PostContol()
    {
        while (volume.weight < 1f)
        {
            volume.weight = Mathf.MoveTowards(volume.weight, 1f, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        while (volume.weight > 0f)
        {
            volume.weight = Mathf.MoveTowards(volume.weight, 0f, speed * Time.deltaTime);
            yield return null;
        }
    }
}
